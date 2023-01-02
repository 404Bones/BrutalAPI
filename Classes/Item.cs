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
        public TriggerCalls trigger = TriggerCalls.Count;
        public EffectorConditionSO[] triggerConditions = new EffectorConditionSO[0];

        //Consumption
        public bool consumedOnUse = false;
        public TriggerCalls consumeTrigger = TriggerCalls.Count;
        public EffectorConditionSO[] consumeConditions = new EffectorConditionSO[0];  

        //Effects
        public WearableStaticModifierSetterSO[] equippedModifiers = new WearableStaticModifierSetterSO[0];

        //Game data
        public ItemPools itemPools;
        public int shopPrice = 0;
        public bool startsLocked = false;
        public int fishRarity = 10;

        //Backwards Compatibility
        [Obsolete("Use itemPools.")]
        public bool isShopItem;

        internal string wName;

        public abstract BaseWearableSO Wearable();

        public void AddItem()
        {
            var w = Wearable();
            
            //Is treasure
            if(itemPools.HasFlag(ItemPools.Treasure))
            {
                wName = Regex.Replace(name + "_TW", @"\s+", "");
            }
            //Is shop
            else if (itemPools.HasFlag(ItemPools.Shop))
            {
                wName = Regex.Replace(name + "_SW", @"\s+", "");
            }
            //Is fish
            else if (itemPools.HasFlag(ItemPools.Fish))
            {
                wName = Regex.Replace(name + "_FW", @"\s+", "");
            }
            //Is extra (no pool)
            else if (itemPools.HasFlag(ItemPools.Extra))
            {
                wName = Regex.Replace(name + "_EW", @"\s+", "");
            } 

            //Backwards compatibility (evil and fucked up)
            else if (isShopItem)
            {
                itemPools |= ItemPools.Shop;
            }
            else if (!isShopItem)
            {
                itemPools |= ItemPools.Treasure;
            }

            w.name = wName;

            if (itemPools.HasFlag(ItemPools.Shop))
            {
                var list = new List<string>(BrutalAPI.mainMenuController._informationHolder.ItemPoolDB._ShopPool);
                list.Add(wName);
                BrutalAPI.mainMenuController._informationHolder.ItemPoolDB._ShopPool = list.ToArray();
            }
            if ((itemPools.HasFlag(ItemPools.Treasure)))
            {
                var list = new List<string>(BrutalAPI.mainMenuController._informationHolder.ItemPoolDB._TreasurePool);
                list.Add(wName);
                BrutalAPI.mainMenuController._informationHolder.ItemPoolDB._TreasurePool = list.ToArray();
            }
            if ((itemPools.HasFlag(ItemPools.Fish)))
            {
                BrutalAPI.fishingRodLoot.Add(new LootItemProbability() { itemName = wName, probability = fishRarity });
                BrutalAPI.canOfWormsLoot.Add(new LootItemProbability() { itemName = wName, probability = fishRarity });
                BrutalAPI.welsCatfishLoot.Add(new LootItemProbability() { itemName = wName, probability = fishRarity });
                BrutalAPI._fishLootPool.Add(new LootItemProbability() { itemName = wName, probability = fishRarity });
            }

            if (!LoadedAssetsHandler.LoadedWearables.ContainsKey(wName))
                LoadedAssetsHandler.LoadedWearables.Add(wName, w);

            BrutalAPI.moddedItems.Add(this);

            Debug.Log("Added item " + wName);
        }
    }

    [Flags]
    public enum ItemPools
    {
        Treasure = 1,
        Shop = 2,
        Fish = 4,
        Extra = 8
    }
}
