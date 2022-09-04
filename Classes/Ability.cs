﻿using UnityEngine;

namespace BrutalAPI
{
    public class Ability
    {
        public string name = "Ability";
        public string description = "Ability Description";
        public Sprite sprite = null;
        public AttackVisualsSO visuals = null;
        public Effect[] effects;
        public ManaColorSO[] cost;
        public int priority = 0;
        public int rarity = 10;
        public bool canBeRerolled = true;

        public CharacterAbility CharacterAbility()
        {
            CharacterAbility ability = new CharacterAbility();
            AbilitySO a = ScriptableObject.CreateInstance(typeof(AbilitySO)) as AbilitySO;
            ability.ability = a;
            ability.cost = cost;
            a.abilityName = name;
            a.description = description;
            a.priority = ScriptableObject.CreateInstance(typeof(PrioritySO)) as PrioritySO;
            a.priority.priorityValue = priority;
            a.abilitySprite = sprite;

            a.effects = new EffectInfo[effects.Length];
            a.intents = new IntentTargetInfo[effects.Length];

            for (int i = 0; i < effects.Length; i++)
            {
                EffectInfo ei = new EffectInfo();
                ei.entryVariable = effects[i]._entryVariable;
                ei.effect = effects[i]._effect;
                ei.targets = effects[i]._target;
                a.effects[i] = ei;

                a.intents[i] = new IntentTargetInfo();
                a.intents[i].targets = effects[i]._target;
                a.intents[i].targetIntents = new IntentType[1] { effects[i]._intent };
            }
            return ability;
        }
        public EnemyAbilityInfo EnemyAbility()
        {
            EnemyAbilityInfo ability = new EnemyAbilityInfo();
            AbilitySO a = ScriptableObject.CreateInstance(typeof(AbilitySO)) as AbilitySO;
            ability.ability = a;
            ability.rarity = ScriptableObject.CreateInstance<RaritySO>();
            ability.rarity.rarityValue = rarity;
            ability.rarity.canBeRerolled = canBeRerolled;
            a.abilityName = name;
            a.description = description;
            a.priority = ScriptableObject.CreateInstance(typeof(PrioritySO)) as PrioritySO;
            a.priority.priorityValue = priority;
            a.abilitySprite = sprite;

            a.effects = new EffectInfo[effects.Length];
            a.intents = new IntentTargetInfo[effects.Length];

            for (int i = 0; i < effects.Length; i++)
            {
                EffectInfo ei = new EffectInfo();
                ei.entryVariable = effects[i]._entryVariable;
                ei.effect = effects[i]._effect;
                ei.targets = effects[i]._target;
                a.effects[i] = ei;

                a.intents[i] = new IntentTargetInfo();
                a.intents[i].targets = effects[i]._target;
                a.intents[i].targetIntents = new IntentType[1] { effects[i]._intent };
            }
            return ability;
        }

        public Ability Duplicate()
        {
            Ability a = new Ability();
            a.name = name;
            a.sprite = sprite;
            a.description = description;
            a.priority = priority;
            a.visuals = visuals;
            a.effects = new Effect[effects.Length];
            a.rarity = rarity;
            a.canBeRerolled = canBeRerolled;
            for (int i = 0; i < effects.Length; i++)
            {
                a.effects[i] = new Effect(effects[i]);
            }
            a.cost = cost;
            return a;
        }
        
    }
    public struct Effect
    {
        public Effect(EffectSO effect, int entryVariable, IntentType intent, BaseCombatTargettingSO target)
        {
            _effect = effect;
            _entryVariable = entryVariable;
            _intent = intent;
            _target = target;
        }

        public Effect(Effect effect)
        {
            _effect = effect._effect;
            _entryVariable = effect._entryVariable;
            _intent = effect._intent;
            _target = effect._target;
        }

        public EffectSO _effect;
        public int _entryVariable;
        public IntentType _intent;
        public BaseCombatTargettingSO _target;
    }
}
