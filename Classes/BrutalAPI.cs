using BepInEx;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;
using System;
using Album;
using System.Xml;
using BepInEx.Bootstrap;

namespace BrutalAPI
{
    [BepInPlugin("Bones404.BrutalAPI", "BrutalAPI", "0.1.0")]
    [BepInDependency("Bones404.Album", BepInDependency.DependencyFlags.SoftDependency)]
    public class BrutalAPI : BaseUnityPlugin
    {

        const BindingFlags AllFlags = (BindingFlags)(-1);

        public static SelectableCharactersSO selCharsSO;

        public static CharacterAbility slapCharAbility;
        public static MainMenuController mainMenuController;
        public static UnlockablesDatabase unlockablesDatabase;

        public static List<CharacterSO> vanillaChars = new List<CharacterSO>();
        public static List<CharacterSO> moddedChars = new List<CharacterSO>();

        public static List<BaseWearableSO> moddedItems = new List<BaseWearableSO>();
        public static List<EnemySO> moddedEnemies = new List<EnemySO>();

        public static List<ZoneBGDataBaseSO> easyAreas = new List<ZoneBGDataBaseSO>();
        public static List<ZoneBGDataBaseSO> hardAreas = new List<ZoneBGDataBaseSO>();

        public static AssetBundle brutalAPIassetBundle;

        public static List<PortalSignsDataBaseSO.PortalSignIcon> moddedPortalSigns = new List<PortalSignsDataBaseSO.PortalSignIcon>();

        public static bool includeExampleContent = true;

        public void Awake()
        {
            IDetour UnlockCharactersHook = new Hook(
                    typeof(SaveDataHandler).GetMethod("LoadSavedData", AllFlags),
                    typeof(BrutalAPI).GetMethod("UnlockCharacters", AllFlags));

            IDetour SignDBInitHook = new Hook(
                    typeof(PortalSignsDataBaseSO).GetMethod("InitializeSignDB", AllFlags),
                    typeof(BrutalAPI).GetMethod("SignDBInit", AllFlags));

            //Add description if Album is installed
            foreach (var plugin in Chainloader.PluginInfos)
            {
                var metadata = plugin.Value.Metadata;
                if (metadata.GUID == "Bones404.Album")
                {
                    new ModDescription("Bones404.BrutalAPI", "API to facilitate modding Brutal Orchestra.\nRead the documentation on the official modding page.");
                    break;
                }
            }
            
            foreach (SelectableCharactersSO i in Resources.FindObjectsOfTypeAll<SelectableCharactersSO>()) { selCharsSO = i; }
            foreach (MainMenuController i in Resources.FindObjectsOfTypeAll<MainMenuController>()) { mainMenuController = i; }
            foreach (string i in mainMenuController._informationHolder.GetZoneDBs())
            {
                ZoneBGDataBaseSO area = LoadedAssetsHandler.GetZoneDB(i) as ZoneBGDataBaseSO;
                easyAreas.Add(area);
            }
            foreach (string i in mainMenuController._informationHolder._runHardZoneDBs)
            {
                ZoneBGDataBaseSO area = LoadedAssetsHandler.GetZoneDB(i) as ZoneBGDataBaseSO;
                hardAreas.Add(area);
            }

            unlockablesDatabase = mainMenuController._informationHolder._unlockableManager._unlockableDB;

            brutalAPIassetBundle = AssetBundle.LoadFromMemory(ResourceLoader.ResourceBinary("brutalapi"));

            if (!Directory.Exists(Paths.BepInExRootPath + "/plugins/brutalapi/") || !File.Exists(Paths.BepInExRootPath + "/plugins/brutalapi/brutalapi.config"))
            {
                Directory.CreateDirectory(Paths.BepInExRootPath + "/plugins/brutalapi/");
                StreamWriter streamWriter = File.CreateText(Paths.BepInExRootPath + "/plugins/brutalapi/brutalapi.config");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml("<config includeExampleContent='false'> </config>");
                xmlDocument.Save(streamWriter);
                streamWriter.Close();
            }

            Pigments.Setup();
            Passives.Setup();
            Slots.Setup();

            if (File.Exists(Paths.BepInExRootPath + "/plugins/brutalapi/brutalapi.config"))
            {
                FileStream fileStream = File.Open(Paths.BepInExRootPath + "/plugins/brutalapi/brutalapi.config", FileMode.Open);
                XmlDocument xmlDocument2 = new XmlDocument();
                xmlDocument2.Load(fileStream);
                includeExampleContent = bool.Parse(xmlDocument2.GetElementsByTagName("config")[0].Attributes["includeExampleContent"].Value);
                fileStream.Close();
            }

            if (includeExampleContent)
            {
                Mungbert.Add();
                Mungbertino.Add();
                ImmortalRoe.Add();
                Deformung.Add();
                DeformungEncounter.Add();
                MungRootBeer.Add();
            }

            Logger.LogInfo("BrutalAPI loaded successfully!");

            foreach (LocalisationData i in Resources.FindObjectsOfTypeAll<LocalisationData>())
            {
                Debug.Log(i.localisationID);
            }

            /* TODO:
            - Custom Ability Animations
            - Sounds
            - Achievements
            - Character Unlocks
            - Areas
            - Quests
            - Flavor Characters
            */
        }

        public void Update()
        {
            if (Keyboard.current.yKey.wasPressedThisFrame)
            {
                mainMenuController._informationHolder.Run.playerData.AddNewItem(LoadedAssetsHandler.GetWearable("Mung Root Beer"));
            }
        }

        public static void AddSignType(SignType type, Sprite sprite)
        {
            moddedPortalSigns.Add(new PortalSignsDataBaseSO.PortalSignIcon() { signIcon = sprite, signType = type });
        }

        public static void SignDBInit(Action<PortalSignsDataBaseSO> orig, PortalSignsDataBaseSO self)
        {
            orig(self);
            foreach (PortalSignsDataBaseSO.PortalSignIcon sign in moddedPortalSigns)
            {
                self._signDB.Add(sign.signType, sign.signIcon);
            }
        }

        public static void UnlockCharacters(Action<SaveDataHandler> orig, SaveDataHandler self)
        {
            orig(self);
            foreach (CharacterSO i in moddedChars)
            {
                self.SavedGameData._unlockedCharacters.Add(i._characterName + "_CH");
            }
        }
    }

    public enum Areas
    {
        FarShore,
        Oprheum,
        Garden
    }
}
