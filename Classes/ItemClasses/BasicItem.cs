using UnityEngine;

namespace BrutalAPI
{
    public class BasicItem : Item
    {
        public override BaseWearableSO Wearable()
        {
            BasicWearable w = ScriptableObject.CreateInstance<BasicWearable>();
            w.BaseWearable(this);
            w._itemName = name;
            w.triggerOn = trigger;
            w.doesItemPopUp = namePopup;
            w.doesTriggerAttachedActionOnInitialization = false;
            w.getsConsumedOnUse = consumedOnUse;
            w.consumeConditions = consumeConditions;
            w._description = description;
            w._flavourText = flavorText;
            w.wearableImage = sprite;
            w.staticModifiers = equippedModifiers;
            w.conditions = triggerConditions;
            w.shopPrice = shopPrice;
            w.isShopItem = itemPools.HasFlag(ItemPools.Shop);
            w.startsLocked = startsLocked;
            w.usesTheOnUnlockText = false;
            return w;
        }
    }
}
