using System;
using System.Collections.Generic;
using UnityEngine;


namespace BrutalAPI
{
    public static class ExtensionMethods
    { 
        public static void SetDefaultParams(this EnemyInFieldLayout e)
        {
            e._rootTransform = e.gameObject.GetComponent<Transform>();
            e._locator = e.gameObject.transform.Find("Locator").gameObject;
            e._renderer = e._locator.transform.Find("Sprite").GetComponent<SpriteRenderer>();
            e._animator = e.gameObject.GetComponent<Animator>();
            e._UI3DLocation = e.gameObject.transform.Find("3DUILocation").transform;
            e._moveTime = 0.6f;
            e._gibsEvent = "event:/Combat/Gibs/CBT_Gibs";
            e.saveAnimationTime = 2;
            e._extraSounds = new string[0];
            e._basicColor = Color.black;
            e._hoverColor = Color.red;
            e._turnColor = Color.yellow;
            e._targetColor = Color.yellow;
        }

        public static void BaseWearable(this BaseWearableSO w, Item item)
        {
            //Base
            w._itemName = item.name;
            w._description = item.description;
            w._flavourText = item.flavorText;
            w.wearableImage = item.sprite;
            w.shopPrice = item.shopPrice;
            w.isShopItem = item.itemPools.HasFlag(ItemPools.Shop);
            w.startsLocked = item.startsLocked;

            //Trigger
            w.triggerOn = item.trigger;
            w.conditions = item.triggerConditions;

            //Consume Trigger
            w.consumeOnTrigger = item.consumeTrigger;
            w.consumeConditions = item.consumeConditions;
            w.getsConsumedOnUse = item.consumedOnUse;

            //Things
            w.doesItemPopUp = item.namePopup;
            w.doesTriggerAttachedActionOnInitialization = false;
            w.staticModifiers = item.equippedModifiers;
            w.usesTheOnUnlockText = true;
        }

        public static void Add(this string[] stringArray, string addedString)
        {
            List<string> list = new List<string>(stringArray);
            list.Add(addedString);
            stringArray = list.ToArray();
        }

        public static int CountColorPigment(this ManaBar manaBar, ManaColorSO mana)
        {
            int amount = 0;
            foreach (ManaBarSlot slot in manaBar.ManaBarSlots)
            {
                if (!slot.IsEmpty && slot.ManaColor == mana)
                    ++amount;
            }

            return amount;
        }

        public static EffectInfo[] ToEffectInfoArray(Effect[] effects)
        {
            EffectInfo[] effectInfoArray = new EffectInfo[effects.Length];
            for (int i = 0; i < effects.Length; i++)
            {
                EffectInfo ei = new EffectInfo();
                ei.entryVariable = effects[i]._entryVariable;
                ei.effect = effects[i]._effect;
                ei.targets = effects[i]._target;
                ei.condition = effects[i]._condition;
                effectInfoArray[i] = ei;
            }
            return effectInfoArray;
        }
    }
}
