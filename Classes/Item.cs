using System;
using UnityEngine;

namespace BrutalAPI
{
    public abstract class Item
    {
        //Basic stuff
        public string name = "Item";
        public string flavorText = "What does it do?";
        public string description = "This is the description for a new item";
        public Sprite sprite = ResourceLoader.LoadSprite("");
        public bool namePopup = false;

        //Trigger
        public TriggerCalls trigger = TriggerCalls.IsAlive;
        public EffectorConditionSO[] triggerConditions = new EffectorConditionSO[0];

        //Consumption
        public bool consumedOnUse = false;
        public TriggerCalls cosumeTrigger = TriggerCalls.IsAlive;
        public EffectorConditionSO[] consumeConditions = new EffectorConditionSO[0];  

        //Effects
        public WearableStaticModifierSetterSO[] equippedModifiers = new WearableStaticModifierSetterSO[0];

        //Game data
        public bool isShopItem = false;
        public int shopPrice = 0;
        public bool startsLocked = false;

        public abstract BaseWearableSO Wearable();
    }

    public struct ConditionEffect
    {
        public ConditionEffect(EffectSO effect, int entryVariable, EffectConditionSO condition, BaseCombatTargettingSO target)
        {
            _effect = effect;
            _entryVariable = entryVariable;
            _condition = condition;
            _target = target;
        }

        public ConditionEffect(ConditionEffect effect)
        {
            _effect = effect._effect;
            _entryVariable = effect._entryVariable;
            _condition = effect._condition;
            _target = effect._target;
        }

        public EffectSO _effect;
        public int _entryVariable;
        public EffectConditionSO _condition;
        public BaseCombatTargettingSO _target;
    }
}
