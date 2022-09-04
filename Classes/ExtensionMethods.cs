using System;
using System.Collections.Generic;
using UnityEngine;


namespace BrutalAPI
{
    public static class ExtensionMethods
    { 
        public static EffectInfo[] ConditionEffectInfoArray(this ConditionEffect[] effect)
        {
            EffectInfo[] ei = new EffectInfo[effect.Length];
            for (int i = 0; i < effect.Length; i++)
            {
                ei[i] = new EffectInfo();
                ei[i].entryVariable = effect[i]._entryVariable;
                ei[i].effect = effect[i]._effect;
                ei[i].targets = effect[i]._target;
                ei[i].condition = effect[i]._condition;
            }
            return ei;
        }

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
            w.wearableName = item.name;
            w.triggerOn = item.trigger;
            w.doesItemPopUp = item.namePopup;
            w.doesTriggerAttachedActionOnInitialization = false;
            w.getsConsumedOnUse = item.consumedOnUse;
            w.consumeConditions = item.consumeConditions;
            w.description = item.description;
            w.flavourText = item.flavorText;
            w.wearableImage = item.sprite;
            w.staticModifiers = item.equippedModifiers;
            w.conditions = item.triggerConditions;
            w.shopPrice = item.shopPrice;
            w.isShopItem = item.isShopItem;
            w.startsLocked = item.startsLocked;
            w.usesTheOnUnlockText = false;
        }

        public static string[] Add(this string[] stringArray, string addedString)
        {
            List<string> list = new List<string>();
            foreach (string i in stringArray)
            {
                list.Add(i);
            }
            list.Add(addedString);
            return list.ToArray();
        }

        public static void AddItem(this Item i)
        {
            var w = i.Wearable();
            
            LoadedAssetsHandler.LoadedWearables.Add(i.name, w);

            if(i.isShopItem) { BrutalAPI.mainMenuController._informationHolder.ItemPoolDB.ShopPool.Add(i.name); }
            else { BrutalAPI.mainMenuController._informationHolder.ItemPoolDB.TreasurePool.Add(i.name); }
            BrutalAPI.moddedItems.Add(w);

            Debug.Log("Added item " + i.name);
        }
    }
}
