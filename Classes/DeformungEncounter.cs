using UnityEngine;

namespace BrutalAPI
{
    public static class DeformungEncounter
    {
        public static void Add()
        {
            EnemyEncounter encounter = new EnemyEncounter();
            encounter.encounterName = "Deformung_Hard_Miniboss";
            encounter.area = (int)Areas.FarShore;
            encounter.randomPlacement = true;
            encounter.hardmodeEncounter = true;
            encounter.difficulty = EncounterDifficulty.Easy;
            encounter.rarity = 1000;
            encounter.variations = new FieldEnemy[2][];
            encounter.signType = (SignType)1412;

            //Encounter enemies
            encounter.variations[0] = new FieldEnemy[1];
            encounter.variations[0][0] = new FieldEnemy() { enemyName = "Deformung_EN", enemySlot = 1 };

            encounter.variations[1] = new FieldEnemy[1];
            encounter.variations[1][0] = new FieldEnemy() { enemyName = "Deformung_EN", enemySlot = 2 };

            BrutalAPI.AddSignType(encounter.signType, ResourceLoader.LoadSprite("DeformungOverworld", 32));
            encounter.AddEncounter();
        }
    }
}
