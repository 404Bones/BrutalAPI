using UnityEngine;
using UnityEngine.UIElements;

namespace BrutalAPI
{
   public static class Mungbert
   {
        public static void Add()
        {
            //Mungbert Basics
            Character mungbert = new Character();

            mungbert.name = "Mungbert";
            mungbert.healthColor = Pigments.Green;
            mungbert.entityID = (EntityIDs)25914;
            mungbert.frontSprite = ResourceLoader.LoadSprite("MungbertFront");
            mungbert.backSprite = ResourceLoader.LoadSprite("MungbertBack");
            mungbert.overworldSprite = ResourceLoader.LoadSprite("MungbertOverworld", 32, new Vector2(0.5f, 0));
            mungbert.lockedSprite = ResourceLoader.LoadSprite("MungbertLocked");
            mungbert.unlockedSprite = ResourceLoader.LoadSprite("MungbertUnlocked");
            mungbert.menuChar = true;
            mungbert.hurtSound = "event:/Characters/Enemies/Mung/CHR_ENM_Mung_Dmg";
            mungbert.deathSound = "event:/Characters/Enemies/Mung/CHR_ENM_Mung_Dth";
            mungbert.osmanUnlock = (UnlockableID)45814;
            mungbert.heavenUnlock = (UnlockableID)45813;

            //Cry level 1
            Ability cryAbility0 = new Ability();
            cryAbility0.name = "Sob";
            cryAbility0.description = "Deal 3 damage to the Opposing enemy. Produce 1 blue pigment.";
            cryAbility0.cost = new ManaColorSO[3] { Pigments.Yellow, Pigments.Blue, Pigments.Blue };
            cryAbility0.sprite = ResourceLoader.LoadSprite("CryAbility");
            cryAbility0.effects = new Effect[2];
            cryAbility0.effects[0] = new Effect(ScriptableObject.CreateInstance<GenerateColorManaEffect>(),
                1, IntentType.Mana_Generate, Slots.Front);
            ((GenerateColorManaEffect)cryAbility0.effects[0]._effect).mana = Pigments.Blue;
            cryAbility0.effects[1] = new Effect(ScriptableObject.CreateInstance<DamageEffect>(),
                3, IntentType.Damage_3_6, Slots.Front);
            cryAbility0.visuals = LoadedAssetsHandler.GetEnemy("Mung_EN").abilities[1].ability.visuals;
            cryAbility0.animationTarget = Slots.Self;

            //Attack level 1
            Ability attackAbility0 = new Ability();
            attackAbility0.name = "Incompetent Cleave";
            attackAbility0.description = "Deal 5 damage to the Opposing enemy.";
            attackAbility0.cost = new ManaColorSO[3] { Pigments.Yellow, Pigments.Red, Pigments.Red };
            attackAbility0.sprite = ResourceLoader.LoadSprite("FishSlapAbility");
            attackAbility0.effects = new Effect[1];
            attackAbility0.effects[0] = new Effect(ScriptableObject.CreateInstance<DamageEffect>(),
                5, IntentType.Damage_3_6, Slots.Front);
            attackAbility0.animationTarget = Slots.Front;

            //Summon level 1
            Ability summonAbility0 = new Ability();
            summonAbility0.name = "Call allies";
            summonAbility0.description = "Call for the help of a Mungbertino with 6 health.";
            summonAbility0.cost = new ManaColorSO[3] { Pigments.Green, Pigments.Green, Pigments.Green };
            summonAbility0.sprite = ResourceLoader.LoadSprite("CallAlliesAbility");
            summonAbility0.effects = new Effect[1];
            var summonEffect = ScriptableObject.CreateInstance<CopyAndSpawnCustomCharacterAnywhereEffect>();
            summonEffect._characterCopy = "Mungbertino_CH";
            summonEffect._nameAddition = NameAdditionLocID.NameAdditionNone;
            summonEffect._rank = 0;
            summonEffect._extraModifiers = new WearableStaticModifierSetterSO[0];
            summonAbility0.effects[0] = new Effect( summonEffect, 1, IntentType.Other_Spawn, Slots.Self);
            summonAbility0.visuals = LoadedAssetsHandler.GetEnemy("MunglingMudLung_EN").abilities[2].ability.visuals;
            summonAbility0.animationTarget = Slots.Self;

            //Cry level 2
            Ability cryAbility1 = cryAbility0.Duplicate();
            cryAbility1.name = "Weep";
            cryAbility1.cost = new ManaColorSO[3] { Pigments.Yellow, Pigments.Yellow, Pigments.Blue };
            cryAbility1.description = "Deal 4 damage to the Opposing enemy. Produce 2 blue pigment.";
            cryAbility1.effects[0]._entryVariable = 2;
            cryAbility1.effects[1]._entryVariable = 4;

            //Attack level 2
            Ability attackAbility1 = attackAbility0.Duplicate();
            attackAbility1.name = "Decent Cleave";
            attackAbility1.description = "Deal 7 damage to the Opposing and Left enemies.";
            attackAbility1.effects[0]._target = Slots.SlotTarget(new int[2] {0, -1});
            attackAbility1.effects[0]._entryVariable = 7;
            attackAbility1.effects[0]._intent = IntentType.Damage_7_10;

            //Summon level 2
            Ability summonAbility1 = summonAbility0.Duplicate();
            summonAbility1.name = "Call Friends";
            summonAbility1.description = "Call for the help of a Mungbertino with 8 health.";
            ((CopyAndSpawnCustomCharacterAnywhereEffect)summonAbility1.effects[0]._effect)._rank = 1;

            //Cry level 3
            Ability cryAbility2 = cryAbility1.Duplicate();
            cryAbility2.name = "Wallow";
            cryAbility2.description = "Deal 5 damage to the Opposing enemy. Produce 2 blue pigment.";
            cryAbility2.cost = new ManaColorSO[3] { Pigments.Yellow, Pigments.Yellow, Pigments.Yellow };
            cryAbility2.effects[0]._entryVariable = 2;
            cryAbility2.effects[1]._entryVariable = 5;

            //Attack level 3
            Ability attackAbility2 = attackAbility1.Duplicate();
            attackAbility2.name = "Great Cleave";
            attackAbility2.description = "Deal 8 damage to the Opposing, Left and Far Left enemies.";
            attackAbility2.cost = new ManaColorSO[3] { Pigments.Red, Pigments.Red, Pigments.Red };
            attackAbility2.effects[0]._target = Slots.SlotTarget(new int[3] { 0, -1 , -2});
            attackAbility2.effects[0]._entryVariable = 8;

            //Summon level 3
            Ability summonAbility2 = summonAbility1.Duplicate();
            summonAbility2.name = "Call Family";
            summonAbility2.cost = new ManaColorSO[2] { Pigments.Green, Pigments.Green };
            summonAbility2.description = "Call for the help of a Mungbertino with 9 health.";
            ((CopyAndSpawnCustomCharacterAnywhereEffect)summonAbility2.effects[0]._effect)._rank = 2;

            //Cry level 4
            Ability cryAbility3 = cryAbility2.Duplicate();
            cryAbility3.name = "Break Down";
            cryAbility3.description = "Deal 6 damage to the Opposing enemy. Produce 3 blue pigment.";
            cryAbility2.cost = new ManaColorSO[2] { Pigments.Yellow, Pigments.Yellow};
            cryAbility3.effects[0]._entryVariable = 3;
            cryAbility3.effects[1]._entryVariable = 6;

            //Attack level 4
            Ability attackAbility3 = attackAbility2.Duplicate();
            attackAbility3.name = "Flawless Cleave";
            attackAbility3.description = "Deal 10 damage to the Opposing, Left and Far Left enemies.";
            attackAbility3.effects[0]._target = Slots.SlotTarget(new int[3] { 0, -1, -2 });
            attackAbility3.effects[0]._entryVariable = 10;
            
            //Summon level 4
            Ability summonAbility3 = summonAbility2.Duplicate();
            summonAbility3.name = "Call Friends";
            summonAbility3.description = "Call for the help of a Mungbertino with 10 health.";
            ((CopyAndSpawnCustomCharacterAnywhereEffect)summonAbility3.effects[0]._effect)._rank = 3;

            mungbert.AddLevel(12, new Ability[3] { attackAbility0, cryAbility0, summonAbility0 }, 0);
            mungbert.AddLevel(14, new Ability[3] { attackAbility1, cryAbility1, summonAbility1 }, 1);
            mungbert.AddLevel(16, new Ability[3] { attackAbility2, cryAbility2, summonAbility2 }, 2);
            mungbert.AddLevel(18, new Ability[3] { attackAbility3, cryAbility3, summonAbility3 }, 3);
            mungbert.AddCharacter();
        }
    }
}
