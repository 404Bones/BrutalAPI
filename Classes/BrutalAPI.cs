using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;
using System;
using Album;
using System.Xml;
using BepInEx.Bootstrap;
using System.Text.RegularExpressions;

namespace BrutalAPI
{
    [BepInPlugin("Bones404.BrutalAPI", "BrutalAPI", "1.0.2")]
    [BepInDependency("Bones404.Album", BepInDependency.DependencyFlags.SoftDependency)]
    public class BrutalAPI : BaseUnityPlugin
    {
        const BindingFlags AllFlags = (BindingFlags)(-1);

        public static SelectableCharactersSO selCharsSO;

        public static CharacterAbility slapCharAbility;
        public static MainMenuController mainMenuController;
        public static UnlockablesDatabase unlockablesDatabase;
        public static OverworldManagerBG overworldManager;
        public static SpecialBackgroundsDataBaseSO backgroundDatabase;

        public static List<CharacterSO> vanillaChars = new List<CharacterSO>();
        public static List<CharacterSO> moddedChars = new List<CharacterSO>();

        public static List<Item> moddedItems = new List<Item>();
        public static List<EnemySO> moddedEnemies = new List<EnemySO>();

        public static List<ZoneBGDataBaseSO> easyAreas = new List<ZoneBGDataBaseSO>();
        public static List<ZoneBGDataBaseSO> hardAreas = new List<ZoneBGDataBaseSO>();

        public static AssetBundle brutalAPIassetBundle;

        public static List<PortalSignsDataBaseSO.PortalSignIcon> moddedPortalSigns = new List<PortalSignsDataBaseSO.PortalSignIcon>();

        public static Dictionary<string, Assembly> assemblyDict = new Dictionary<string, Assembly>();
        public static SoundManager soundManager;

        public static bool includeExampleContent = true;
        public static char openDebugConsoleKey = '*';

        public DebugController debug;

        internal static List<LootItemProbability> fishingRodLoot
        {
            get
            {
                return new List<LootItemProbability>(((ExtraLootListEffect)((PerformEffectWearable)LoadedAssetsHandler.GetWearable("FishingRod_TW")).effects[0].effect)._lootableItems);
            }
            set
            {
                ((ExtraLootListEffect)((PerformEffectWearable)LoadedAssetsHandler.GetWearable("FishingRod_TW")).effects[0].effect)._lootableItems = value.ToArray();
            }
        }
        internal static List<LootItemProbability> canOfWormsLoot
        {
            get
            {
                return new List<LootItemProbability>(((ExtraLootListEffect)((PerformEffectWearable)LoadedAssetsHandler.GetWearable("CanOfWorms_SW")).effects[0].effect)._lootableItems);
            }
            set
            {
                ((ExtraLootListEffect)((PerformEffectWearable)LoadedAssetsHandler.GetWearable("CanOfWorms_SW")).effects[0].effect)._lootableItems = value.ToArray();
            }
        }
        internal static List<LootItemProbability> welsCatfishLoot
        {
            get
            {
                return new List<LootItemProbability>(((ExtraLootListEffect)((PerformEffectWithConsumeEffectWearable)LoadedAssetsHandler.GetWearable("WelsCatfish_ExtraW"))._consumptionEffects[0].effect)._lootableItems);
            }
            set
            {
                ((ExtraLootListEffect)((PerformEffectWithConsumeEffectWearable)LoadedAssetsHandler.GetWearable("WelsCatfish_ExtraW"))._consumptionEffects[0].effect)._lootableItems = value.ToArray();
            }
        }

        internal static List<LootItemProbability> _fishLootPool = new List<LootItemProbability>(fishingRodLoot);
        public static LootItemProbability[] FishLootPool
        {
            get
            {
                return _fishLootPool.ToArray();
            }
        }

        /* BIG TODO:
        - Blue Portals (Free Fools and NPC Dialogue)
        - Quests
        - Custom Ability Animations
        - Sounds
        - Achievements
        - Character Unlocks
        - Areas
        */

        /* SMALL TODO:
        */

        /* CHANGELOG
         - Changed how item pools work
         - Added fish pool
         - Character can now have a different ID and name
         - You can now add boss gate background images
         - TP command now has an extra parameter to reroll areas
         - Chance condition can now be used for items and effects
         */

        public void Awake()
        {
            IDetour UnlockThingsHook = new Hook(
                    typeof(SaveDataHandler).GetMethod("LoadSavedData", AllFlags),
                    typeof(BrutalAPI).GetMethod("UnlockThings", AllFlags));

            IDetour SignDBInitHook = new Hook(
                    typeof(PortalSignsDataBaseSO).GetMethod("InitializeSignDB", AllFlags),
                    typeof(BrutalAPI).GetMethod("SignDBInit", AllFlags));

            IDetour AssignOWManagerHook = new Hook(
                    typeof(OverworldManagerBG).GetMethod("Awake", AllFlags),
                    typeof(BrutalAPI).GetMethod("AssignOWManager", AllFlags));

            IDetour ChangeStunHook = new Hook(
                    typeof(CombatManager).GetMethod("InitializeCombat", (BindingFlags)(-1)),
                    typeof(BrutalAPI).GetMethod("ChangeStun", (BindingFlags)(-1)));

            IDetour ChangeGuttedHook = new Hook(
                    typeof(CombatManager).GetMethod("InitializeCombat", (BindingFlags)(-1)),
                    typeof(BrutalAPI).GetMethod("ChangeGutted", (BindingFlags)(-1)));

            //Add description if Album is installed
            foreach (var plugin in Chainloader.PluginInfos)
            {
                var metadata = plugin.Value.Metadata;
                if (metadata.GUID == "Bones404.Album")
                {
                    new ModDescription("Bones404.BrutalAPI", "API to facilitate modding Brutal Orchestra.\nRead the documentation on the official modding page.\n");
                    break;
                }
            }

            //Find selectable characters
            SelectableCharactersSO[] schSO = Resources.FindObjectsOfTypeAll<SelectableCharactersSO>();
            for (int i = 0; i < schSO.Length; i++)
            {
                selCharsSO = schSO[i];
            }

            //Find main menu controller
            MainMenuController[] mmc = Resources.FindObjectsOfTypeAll<MainMenuController>();
            for (int i = 0; i < mmc.Length; i++)
            {
                mainMenuController = mmc[i];
            }

            //Find overworld manager
            OverworldManagerBG[] ombg = Resources.FindObjectsOfTypeAll<OverworldManagerBG>();
            for (int i = 0; i < ombg.Length; i++)
            {
                overworldManager = ombg[i];
            }

            //Find background manager
            SpecialBackgroundsDataBaseSO[] bgdb = Resources.FindObjectsOfTypeAll<SpecialBackgroundsDataBaseSO>();
            for (int i = 0; i < bgdb.Length; i++)
            {
                backgroundDatabase = bgdb[i];
            }

            //Register easy areas
            string[] easyAreasArray = mainMenuController._informationHolder.GetZoneDBs();
            for (int i = 0; i < easyAreasArray.Length; i++)
            {
                ZoneBGDataBaseSO area = LoadedAssetsHandler.GetZoneDB(easyAreasArray[i]) as ZoneBGDataBaseSO;
                easyAreas.Add(area);
            }

            //Register hard areas
            string[] hardAreasArray = mainMenuController._informationHolder._runHardZoneDBs;
            for (int i = 0; i < hardAreasArray.Length; i++)
            {
                ZoneBGDataBaseSO area = LoadedAssetsHandler.GetZoneDB(hardAreasArray[i]) as ZoneBGDataBaseSO;
                hardAreas.Add(area);
            }

            unlockablesDatabase = mainMenuController._informationHolder._unlockableManager._unlockableDB;

            brutalAPIassetBundle = AssetBundle.LoadFromMemory(ResourceLoader.ResourceBinary("brutalapi"));

            if (!Directory.Exists(Paths.BepInExRootPath + "/plugins/brutalapi/") || !File.Exists(Paths.BepInExRootPath + "/plugins/brutalapi/brutalapi.config"))
            {
                Directory.CreateDirectory(Paths.BepInExRootPath + "/plugins/brutalapi/");
                StreamWriter streamWriter = File.CreateText(Paths.BepInExRootPath + "/plugins/brutalapi/brutalapi.config");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml("<config includeExampleContent='false' openDebugConsoleKey='*'> </config>");
                xmlDocument.Save(streamWriter);
                streamWriter.Close();
            }

            Pigments.Setup();
            Passives.Setup();
            Slots.Setup();
            soundManager = new GameObject("DebugController").AddComponent<SoundManager>();

            if (File.Exists(Paths.BepInExRootPath + "/plugins/brutalapi/brutalapi.config"))
            {
                FileStream fileStream = File.Open(Paths.BepInExRootPath + "/plugins/brutalapi/brutalapi.config", FileMode.Open);
                XmlDocument xmlDocument2 = new XmlDocument();
                xmlDocument2.Load(fileStream);
                includeExampleContent = bool.Parse(xmlDocument2.GetElementsByTagName("config")[0].Attributes["includeExampleContent"].Value);
                try
                {
                    openDebugConsoleKey = char.Parse(xmlDocument2.GetElementsByTagName("config")[0].Attributes["openDebugConsoleKey"].Value);
                }
                catch (FormatException)
                { Debug.LogError("openDebugConsoleKey is not a single character! Please check your config file in BepInEx/plugins/brutalapi/brutalapi.config"); }

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

            debug = new GameObject("DebugController").AddComponent<DebugController>();

            Logger.LogInfo("BrutalAPI loaded successfully!");
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

        public static void UnlockThings(Action<SaveDataHandler> orig, SaveDataHandler self)
        {
            orig(self);
            foreach (CharacterSO i in moddedChars)
            {
                self.SavedGameData._unlockedCharacters.Add(i._characterName + "_CH");
            }
            foreach (Item i in moddedItems)
            {
                self.SavedGameData._unlockedItems.Add(i.wName);
            }
        }

        public static void AssignOWManager(Action<OverworldManagerBG> orig, OverworldManagerBG self)
        {
            orig(self);
            overworldManager = self;
        }

        public static void ChangeArea(Areas areaID, bool rerollArea = false)
        {
            if (overworldManager._zoneBeingLoaded)
            {
                return;
            }
            overworldManager._zoneBeingLoaded = true;
            overworldManager._soundManager.ReleaseOverworldMusic();
            RunDataSO run = overworldManager._informationHolder.Run;
            ZoneDataBaseSO currentZoneDB = run.CurrentZoneDB;
            run._currentZoneID = (int)areaID;
            if (rerollArea)
                run.ResetZoneData();
            string nextSceneName = overworldManager._mainMenuSceneName;
            if (run.DoesCurrentZoneExist)
            {
                overworldManager._informationHolder.Run.zoneLoadingType = ZoneLoadingType.ZoneStart;
                overworldManager._soundManager.TryStopAmbience();
                nextSceneName = SceneManager.GetActiveScene().name;
                overworldManager._soundManager.PlayOneshotSound(overworldManager._soundManager.changeZone);
            }
            overworldManager.StartCoroutine(overworldManager.LoadNextZone(nextSceneName));
        }

        public static void ChangeStun(Action<CombatManager> orig, CombatManager self)
        {
            orig(self);
            StatusEffectInfoSO newstunned;
            self._stats.statusEffectDataBase.TryGetValue(StatusEffectType.Stunned, out newstunned);
            newstunned.icon = ResourceLoader.LoadSprite("StunIcon", 32);
            newstunned._description = "No abilities can be performed while Stunned.\nOn enemies, reduce the amount of Stun by 1 every time they attempt to perform an attack.\nReduce the amount of Stun on party members by 1 at the end of each turn.";
            newstunned._applied_SE_Event = self._stats.slotStatusEffectDataBase[SlotStatusEffectType.Shield]._special_SE_Event;
            newstunned._removed_SE_Event = self._stats.statusEffectDataBase[StatusEffectType.Ruptured].RemovedSoundEvent;
            self._stats.statusEffectDataBase[StatusEffectType.Stunned] = newstunned;
        }
        public static void ChangeGutted(Action<CombatManager> orig, CombatManager self)
        {
            orig(self);
            StatusEffectInfoSO newgutted;
            self._stats.statusEffectDataBase.TryGetValue(StatusEffectType.Gutted, out newgutted);
            newgutted.icon = ResourceLoader.LoadSprite("GuttedIcon", 32);
            newgutted._description = "While gutted maximum health is always equal to current health.\nHealing while gutted will increase maximum health.\nDecrease Gutted by 1 at the end of each turn.";
            newgutted._applied_SE_Event = self._stats.statusEffectDataBase[StatusEffectType.Ruptured].AppliedSoundEvent;
            newgutted._removed_SE_Event = self._stats.statusEffectDataBase[StatusEffectType.Ruptured].RemovedSoundEvent;
            self._stats.statusEffectDataBase[StatusEffectType.Gutted] = newgutted;
        }

        /// <summary>
        /// Restarts the game, probably should never use this...
        /// </summary>
        public static void Restart()
        {
            System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe"));
            Application.Quit();
        }
    }

    public enum Areas
    {
        FarShore,
        Oprheum,
        Garden
    }
}
