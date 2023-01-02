using UnityEngine;

namespace BrutalAPI
{
    public static class MungRootBeer
    {
        public static void Add()
        {
            EffectItem i = new EffectItem();
            i.name = "Mung Root Beer";
            i.flavorText = "Tastes like carbonated butter.";
            i.description = "\"Slap\" for this party member is replaced with \"Mung Me Up\", an ability which gives this party member shield based on the amount of blue pigment stored.";
            i.sprite = ResourceLoader.LoadSprite("MungRootBeer");
            i.namePopup = false;
            i.consumedOnUse = false;
            i.itemPools = ItemPools.Shop;
            i.startsLocked = false;
            i.immediate = true;
            i.unlockableID = (UnlockableID)45814;

            Ability drinkUp = new Ability();
            drinkUp.name = "Mung Me Up";
            drinkUp.description = "Apply shield to this character depending on the amount of blue pigment stored.";
            drinkUp.cost = new ManaColorSO[1] { Pigments.Yellow };
            drinkUp.sprite = ResourceLoader.LoadSprite("MungMeUp");
            drinkUp.effects = new Effect[2];

            drinkUp.effects[0] = new Effect(ScriptableObject.CreateInstance<Check_CountPigmentEffect>(),
                0, null, Slots.Self);
            ((Check_CountPigmentEffect)drinkUp.effects[0]._effect)._colourPigment = Pigments.Blue;

            drinkUp.effects[1] = new Effect(ScriptableObject.CreateInstance<ApplyShieldSlotEffect>(),
                1, IntentType.Field_Shield, Slots.Self);
            ((ApplyShieldSlotEffect)drinkUp.effects[1]._effect)._usePreviousExitValue = true;

            i.equippedModifiers = new WearableStaticModifierSetterSO[1];
            i.equippedModifiers[0] = ScriptableObject.CreateInstance<BasicAbilityChange_Wearable_SMS>();
            ((BasicAbilityChange_Wearable_SMS)i.equippedModifiers[0])._basicAbility = drinkUp.CharacterAbility();

            i.AddItem();
        }
    }
}
