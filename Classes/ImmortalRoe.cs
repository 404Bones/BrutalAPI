using UnityEngine;

namespace BrutalAPI
{
    public static class ImmortalRoe
    {
        public static void Add()
        {
            EffectItem i = new EffectItem();
            i.name = "Immortal Roe";
            i.flavorText = "Smells like the sea and beer?";
            i.description = "On death, create a Mungbertino on the wearer's position. This item is consumed on use.";
            i.sprite = ResourceLoader.LoadSprite("ImmortalRoe");
            i.trigger = TriggerCalls.OnDeath;
            i.namePopup = true;
            i.consumedOnUse = true;
            i.isShopItem = false;
            i.startsLocked = false;
            i.immediate = false;
            CopyAndSpawnCustomCharacterAnywhereEffect e = ScriptableObject.CreateInstance<CopyAndSpawnCustomCharacterAnywhereEffect>();
            e._characterCopy = "Mungbertino_CH";
            e._nameAddition = NameAdditionLocID.NameAdditionNone;
            e._rank = 0;
            e._extraModifiers = new WearableStaticModifierSetterSO[0];
            e._permanentSpawn = true;
            i.effects = new Effect[1] { new Effect(e, 1, null, Slots.Self) };
            i.unlockableID = (UnlockableID)45813;
            i.AddItem();
        }
    }
}
