using BepInEx;
using UnityEngine;
using UnityEngine.InputSystem;
using Album;
using System.Reflection;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;
using System;

namespace BrutalAPI
{
    [BepInPlugin("Bones404.BrutalAPI", "BrutalAPI", "1.0.0")]
    public class BrutalAPI : BaseUnityPlugin
    {
        const BindingFlags AllFlags = (BindingFlags)(-1);
        public static SelectableCharactersSO selCharsSO;
        public static CharacterAbility slapCharAbility;
        public static MainMenuController mainMenuController;

        public static List<CharacterSO> vanillaChars = new List<CharacterSO>();
        public static List<CharacterSO> moddedChars = new List<CharacterSO>();
        public static List<BaseWearableSO> moddedItems = new List<BaseWearableSO>();
        public static List<EnemySO> moddedEnemies = new List<EnemySO>();

        public static List<ZoneBGDataBaseSO> areas = new List<ZoneBGDataBaseSO>();

        public static AssetBundle brutalAPIassetBundle;

        public static List<PortalSignsDataBaseSO.PortalSignIcon> moddedPortalSigns = new List<PortalSignsDataBaseSO.PortalSignIcon>();

        public void Awake()
        {
            IDetour SignDBInitHook = new Hook(
                    typeof(PortalSignsDataBaseSO).GetMethod("InitializeSignDB", AllFlags),
                    typeof(BrutalAPI).GetMethod("SignDBInit", AllFlags));

            new ModDescription("Bones404.BrutalAPI", "API to facilitate modding Brutal Orchestra.\nRead the documentation on the official modding page .");
            foreach (SelectableCharactersSO i in Resources.FindObjectsOfTypeAll<SelectableCharactersSO>()) { selCharsSO = i; }
            foreach (MainMenuController i in Resources.FindObjectsOfTypeAll<MainMenuController>()) { mainMenuController = i; }
            foreach (string i in mainMenuController._informationHolder.GetZoneDBs())
            {
                ZoneBGDataBaseSO area = LoadedAssetsHandler.GetZoneDB(i) as ZoneBGDataBaseSO;
                areas.Add(area);
            }

            brutalAPIassetBundle = AssetBundle.LoadFromMemory(ResourceLoader.ResourceBinary("brutalapi"));

            Pigments.Setup();
            Passives.Setup();
            Slots.Setup();
            Mungbert.Add();
            Mungbertino.Add();
            ImmortalRoe.Add();
            Deformung.Add();
            DeformungEncounter.Add();

            Logger.LogInfo("BrutalAPI loaded successfully!");

            /* TODO:
            - Enemies
            - Sounds
            - Character Unlocks
            - Areas
            */
        }

        public void Update()
        {
            if (Keyboard.current.yKey.wasPressedThisFrame)
            {
                mainMenuController._informationHolder.Run.playerData.AddNewItem(moddedItems[0]);
            }
        }

        public static void AddSignType(SignType type, Sprite sprite)
        {
            moddedPortalSigns.Add(new PortalSignsDataBaseSO.PortalSignIcon() { signIcon = sprite, signType = type});
        }

        public static void SignDBInit(Action<PortalSignsDataBaseSO> orig, PortalSignsDataBaseSO self)
        {
            orig(self);
            foreach (PortalSignsDataBaseSO.PortalSignIcon sign in moddedPortalSigns)
            {
                self._signDB.Add(sign.signType, sign.signIcon);
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
