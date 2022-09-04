using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BrutalAPI
{
    public class Enemy
    {
        private EnemySO e = ScriptableObject.CreateInstance(typeof(EnemySO)) as EnemySO;
        const BindingFlags AllFlags = (BindingFlags)(-1);

        //Basics
        public string name = "Enemy";
        public int health = 10;
        public int size = 1;
        public EntityIDs entityID;
        public ManaColorSO healthColor = Pigments.Purple;
        public EnemyLootItemProbability[] loot = new EnemyLootItemProbability[0];

        //Abilities
        public int priority = 0;
        public BaseAbilitySelectorSO abilitySelector = ScriptableObject.CreateInstance<AbilitySelector_ByRarity>();
        public BasePassiveAbilitySO[] passives = new BasePassiveAbilitySO[0];
        public Ability[] abilities = new Ability[0];
        public EnemyInFieldLayout prefab = new EnemyInFieldLayout();
        public ConditionEffect[] enterEffects = new ConditionEffect[0];
        public ConditionEffect[] exitEffects = new ConditionEffect[0];

        //Visuals
        public Sprite combatSprite = ResourceLoader.LoadSprite("DeformungCombat");
        public Sprite overworldAliveSprite = ResourceLoader.LoadSprite("DeformungOverworld");
        public Sprite overworldDeadSprite = ResourceLoader.LoadSprite("DeformungCorpse");

        //Audio
        public string hurtSound = "";
        public string deathSound = "";

        public void AddEnemy()
        {
            e.enemyName = name;
            e.health = health;
            e.size = size;
            e.healthColor = healthColor;
            e.enemyOverworldSprite = overworldAliveSprite;
            e.enemyOWCorpseSprite = overworldDeadSprite;
            e.enemySprite = combatSprite;
            e.abilitySelector = abilitySelector;
            e.passiveAbilities = passives;
            e.enterEffects = enterEffects.ConditionEffectInfoArray();
            e.exitEffects = exitEffects.ConditionEffectInfoArray();
            e.damageSound = hurtSound;
            e.deathSound = deathSound;
            e.enemyTemplate = prefab;

            //Convert Ability to EnemyAbilityInfo
            EnemyAbilityInfo[] eai = new EnemyAbilityInfo[abilities.Length];
            for (int i = 0; i < abilities.Length; i++)
            {
                eai[i] = abilities[i].EnemyAbility();
            }
            e.abilities = eai;

            e.enemyLoot = new EnemyLoot() { _lootableItems = loot };

            LoadedAssetsHandler.LoadedEnemies.Add(name + "_EN", e);
            Debug.Log("Added enemy " + name + "_EN");
            BrutalAPI.moddedEnemies.Add(e);
        }
    }
}
