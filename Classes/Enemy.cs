using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Text.RegularExpressions;

namespace BrutalAPI
{
    public class Enemy
    {
        const BindingFlags AllFlags = (BindingFlags)(-1);

        //Basics
        public string name = "Enemy";
        public string enemyID = "";
        public int health = 10;
        public int size = 1;
        public EntityIDs entityID;
        public ManaColorSO healthColor = Pigments.Purple;
        public EnemyLootItemProbability[] loot = new EnemyLootItemProbability[0];
        public UnitType unitType = UnitType.Unit;

        //Abilities
        public int priority = 0;
        public BaseAbilitySelectorSO abilitySelector = ScriptableObject.CreateInstance<AbilitySelector_ByRarity>();
        public BasePassiveAbilitySO[] passives = new BasePassiveAbilitySO[0];
        public Ability[] abilities = new Ability[0];
        public EnemyInFieldLayout prefab = new EnemyInFieldLayout();
        public Effect[] enterEffects = new Effect[0];
        public Effect[] exitEffects = new Effect[0];

        //Visuals
        public Sprite combatSprite = ResourceLoader.LoadSprite("DeformungCombat");
        public Sprite overworldAliveSprite = ResourceLoader.LoadSprite("DeformungOverworld");
        public Sprite overworldDeadSprite = ResourceLoader.LoadSprite("DeformungCorpse");

        //Audio
        public string hurtSound = "";
        public string deathSound = "";

        public void AddEnemy()
        {
            EnemySO e = ScriptableObject.CreateInstance(typeof(EnemySO)) as EnemySO;

            string enemyIDName = enemyID == "" ? name + "_EN" : enemyID;
            string ename = Regex.Replace(enemyIDName, @"\s+", "");
            e.name = ename;

            e._enemyName = name;
            e.health = health;
            e.size = size;
            e.healthColor = healthColor;
            e.enemyOverworldSprite = overworldAliveSprite;
            e.enemyOWCorpseSprite = overworldDeadSprite;
            e.enemySprite = combatSprite;
            e.abilitySelector = abilitySelector;
            e.passiveAbilities = passives;
            e.enterEffects = ExtensionMethods.ToEffectInfoArray(enterEffects);
            e.exitEffects = ExtensionMethods.ToEffectInfoArray(exitEffects);
            e.damageSound = hurtSound;
            e.deathSound = deathSound;
            prefab.SetDefaultParams();
            e.enemyTemplate = prefab;
            e.priority = ScriptableObject.CreateInstance(typeof(PrioritySO)) as PrioritySO;
            e.priority.priorityValue = priority;
            e.unitType = unitType;

            //Convert Ability to EnemyAbilityInfo
            EnemyAbilityInfo[] eai = new EnemyAbilityInfo[abilities.Length];
            for (int i = 0; i < abilities.Length; i++)
            {
                eai[i] = abilities[i].EnemyAbility();
            }
            e.abilities = eai;

            e.enemyLoot = new EnemyLoot() { _lootableItems = loot };

            LoadedAssetsHandler.LoadedEnemies.Add(ename, e);
            Debug.Log("Added enemy " + ename);
            BrutalAPI.moddedEnemies.Add(e);
        }
    }
}
