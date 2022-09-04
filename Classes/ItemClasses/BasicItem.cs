using UnityEngine;

namespace BrutalAPI
{
    public class BasicItem : Item
    {
        public override BaseWearableSO Wearable()
        {
            BasicWearable w = ScriptableObject.CreateInstance<BasicWearable>();
            w.BaseWearable(this);
            w.wearableName = name;
            w.triggerOn = trigger;
            w.doesItemPopUp = namePopup;
            w.doesTriggerAttachedActionOnInitialization = false;
            w.getsConsumedOnUse = consumedOnUse;
            w.consumeConditions = consumeConditions;
            w.description = description;
            w.flavourText = flavorText;
            w.wearableImage = sprite;
            w.staticModifiers = equippedModifiers;
            w.conditions = triggerConditions;
            w.shopPrice = shopPrice;
            w.isShopItem = isShopItem;
            w.startsLocked = startsLocked;
            w.usesTheOnUnlockText = false;
            return w;
        }
    }
}
