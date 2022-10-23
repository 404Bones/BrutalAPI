using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace BrutalAPI
{
    public class EnemyEncounter
    {
        public string encounterName = "NewEncounter";
        public FieldEnemy[][] variations = new FieldEnemy[1][];
        public EncounterDifficulty difficulty = EncounterDifficulty.Hard;
        public bool randomPlacement = false;
        public bool hardmodeEncounter = true;
        public int area = (int)Areas.FarShore;
        public int rarity = 10;
        public SignType signType = SignType.None;

        public string roarEvent = "";
        public string musicEvent = "";

        public void AddEncounter()
        {
            //Add to bundle selector
            EnemyEncounterSelectorSO selector;
            
            if (hardmodeEncounter)
            {
                switch (difficulty)
                {
                    case EncounterDifficulty.Easy:
                        selector = BrutalAPI.hardAreas[area]._easyEnemyBundleSelector;
                        break;
                    case EncounterDifficulty.Medium:
                        selector = BrutalAPI.hardAreas[area]._mediumEnemyBundleSelector;
                        break;
                    case EncounterDifficulty.Hard:
                        selector = BrutalAPI.hardAreas[area]._hardEnemyBundleSelector;
                        break;
                    case EncounterDifficulty.Boss:
                        selector = BrutalAPI.hardAreas[area]._bossBundleSelector;
                        break;
                    default:
                        Debug.LogError("Could not find bundle selector for area " + BrutalAPI.hardAreas[area].ZoneName);
                        selector = new EnemyEncounterSelectorSO();
                        break;
                }
            } else
            {
                switch (difficulty)
                {
                    case EncounterDifficulty.Easy:
                        selector = BrutalAPI.easyAreas[area]._easyEnemyBundleSelector;
                        break;
                    case EncounterDifficulty.Medium:
                        selector = BrutalAPI.easyAreas[area]._mediumEnemyBundleSelector;
                        break;
                    case EncounterDifficulty.Hard:
                        selector = BrutalAPI.easyAreas[area]._hardEnemyBundleSelector;
                        break;
                    case EncounterDifficulty.Boss:
                        selector = BrutalAPI.easyAreas[area]._bossBundleSelector;
                        break;
                    default:
                        Debug.LogError("Could not find bundle selector for area " + BrutalAPI.easyAreas[area].ZoneName);
                        selector = new EnemyEncounterSelectorSO();
                        break;
                }
            }
            
            global::EnemyEncounter encounter = new global::EnemyEncounter();
            encounter._bundleName = encounterName;
            encounter._priority = rarity;

            selector._enemyEncounters = selector._enemyEncounters.Append(encounter).ToArray();

            //Add to LoadedAssetsHandler
            BaseBundleGeneratorSO generator;
            if (randomPlacement) //Random placement
            {
                generator = ScriptableObject.CreateInstance<RandomEnemyBundleSO>();

                RandomEnemyGroup[] groups = new RandomEnemyGroup[variations.Length];
                //Set enemy names for each variation
                for (int i = 0; i < variations.Length; i++)
                {
                    //Get all names of enemies in this variation
                    List<string> eNames = new List<string>();
                    foreach (FieldEnemy item in variations[i])
                        eNames.Add(item.enemyName);

                    groups[i] = new RandomEnemyGroup();
                    groups[i]._enemyNames = eNames.ToArray();
                }

                ((RandomEnemyBundleSO)generator)._enemyBundles = groups;
            }
            //Specific placement
            else
            {
                generator = ScriptableObject.CreateInstance<SpecificEnemyBundleSO>();

                SpecificEnemyGroup[] groups = new SpecificEnemyGroup[variations.Length];
                //Set group per variation
                for (int i = 0; i < groups.Length; i++)
                {
                    //Set info for each enemy in variation
                    SpecificEnemyInfo[] info = new SpecificEnemyInfo[variations[i].Length];
                    for (int j = 0; j < info.Length; j++)
                    {
                        info[j].enemyName = variations[i][j].enemyName;
                        info[j].enemySlot = variations[i][j].enemySlot;
                    }
                }
                ((SpecificEnemyBundleSO)generator)._enemyBundles = groups;
            }

            generator._usesCustomRoomPrefab = false;
            generator._usesDialogueEvent = false;
            generator._usesSpecialEnvironment = false;
            generator._roarReference = new RoarData(roarEvent);
            generator._musicEventReference = musicEvent;
            generator._preCombatDialogueEventReference = "";
            generator._bossType = BossType.None;
            generator._customRoomPrefab = "";
            generator._bundleSignType = signType;

            LoadedAssetsHandler.LoadedEnemyBundles.Add(encounterName, generator);

            if (randomPlacement)
            {
                BaseBundleGeneratorSO bundle = LoadedAssetsHandler.GetEnemyBundle(encounterName);
                RandomEnemyBundleSO ranbu = bundle as RandomEnemyBundleSO;
                if (ranbu == null || ranbu.Equals(null))
                {
                    Debug.LogError("Random bundle is null");
                }
                foreach (RandomEnemyGroup item in ranbu._enemyBundles)
                {
                    foreach (string name in item.EnemyNames)
                    {
                        EnemySO enemy = LoadedAssetsHandler.GetEnemy(name);
                        if (enemy == null || enemy.Equals(null))
                            Debug.LogError(string.Format($"Enemy {0} is null :(", name));
                    }
                }
            }

            Debug.Log("Added " + encounterName + " encounter");
        }

    }
    public enum EncounterDifficulty
    {
        Easy,
        Medium,
        Hard,
        Boss
    }

    public struct FieldEnemy
    {
        public string enemyName;
        public int enemySlot;
    }
}