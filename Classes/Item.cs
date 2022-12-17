using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
        public UnlockableID unlockableID = UnlockableID.None;

        //Trigger
        public TriggerCalls trigger = TriggerCalls.IsAlive;
        public EffectorConditionSO[] triggerConditions = new EffectorConditionSO[0];

        //Consumption
        public bool consumedOnUse = false;
        public TriggerCalls consumeTrigger = TriggerCalls.IsAlive;
        public EffectorConditionSO[] consumeConditions = new EffectorConditionSO[0];  

        //Effects
        public WearableStaticModifierSetterSO[] equippedModifiers = new WearableStaticModifierSetterSO[0];

        //Game data
        public bool isShopItem = false;
        public int shopPrice = 0;
        public bool startsLocked = false;

        public abstract BaseWearableSO Wearable();

        public void AddItem()
        {
            var w = Wearable();
            var wname = Regex.Replace(name + (isShopItem ? "_SW" : "_TW"), @"\s+", "");
            w.name = wname;

            if (isShopItem)
            {
                var list = new List<string>(BrutalAPI.mainMenuController._informationHolder.ItemPoolDB._ShopPool);
                list.Add(wname);
                BrutalAPI.mainMenuController._informationHolder.ItemPoolDB._ShopPool = list.ToArray();
            }
            else
            {
                var list = new List<string>(BrutalAPI.mainMenuController._informationHolder.ItemPoolDB._TreasurePool);
                list.Add(wname);
                BrutalAPI.mainMenuController._informationHolder.ItemPoolDB._TreasurePool = list.ToArray();
            }

            if (!LoadedAssetsHandler.LoadedWearables.ContainsKey(wname))
                LoadedAssetsHandler.LoadedWearables.Add(wname, w);

            BrutalAPI.moddedItems.Add(this);

            Debug.Log("Added item " + wname);
        }
    }
}
