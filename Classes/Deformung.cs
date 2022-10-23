using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public static class Deformung
    {
        public static void Add()
        {
            Enemy e = new Enemy();
            e.name = "Deformung";
            e.health = 55;
            e.size = 2;
            e.entityID = (EntityIDs)49101;
            e.healthColor = Pigments.Red;
            e.priority = 0;
            e.prefab = BrutalAPI.brutalAPIassetBundle.LoadAsset<GameObject>("assets/deformung/deformung variant.prefab").AddComponent<EnemyInFieldLayout>();
            e.prefab.SetDefaultParams();
            e.prefab._gibs = BrutalAPI.brutalAPIassetBundle.LoadAsset<GameObject>("assets/deformung/deformunggibs.prefab").GetComponent<ParticleSystem>();
            e.combatSprite = ResourceLoader.LoadSprite("DeformungCombat", 32);
            e.overworldAliveSprite = ResourceLoader.LoadSprite("DeformungOverworld", 64, new Vector2(0.5f, 0));
            e.overworldDeadSprite = ResourceLoader.LoadSprite("DeformungCorpse", 64, new Vector2(0.5f, 0));
            e.hurtSound = "event:/Characters/Enemies/Mung/CHR_ENM_Mung_Dmg";
            e.deathSound = "event:/Characters/Enemies/Mung/CHR_ENM_Mung_Dth";  
            e.loot = new EnemyLootItemProbability[1] {new EnemyLootItemProbability() { isItemTreasure = true, amount = 1, probability = 100 } };
            e.abilitySelector = ScriptableObject.CreateInstance<AbilitySelector_ByRarity>();
            e.passives = new BasePassiveAbilitySO[] { Passives.Multiattack, Passives.Overexert, Passives.Slippery };
            e.passives[0]._passiveName = "Multiattack(3)";
            ((IntegerSetterPassiveAbility)e.passives[0]).integerValue = 3;

            e.passives[1]._passiveName = "Overexert(8)";
            e.passives[1]._enemyDescription = "Upon receiving 8 or more direct damage, cancel 1 of this enemy's actions.";
            ((OverexertPassiveAbility)e.passives[1]).conditions[0] = ScriptableObject.CreateInstance<IntegerReferenceOverEqualValueEffectorCondition>();
            ((IntegerReferenceOverEqualValueEffectorCondition)((OverexertPassiveAbility)e.passives[1]).conditions[0]).compareValue = 8;

            e.abilities = new Ability[5];

            Ability hysteria = new Ability();
            hysteria.name = "Hysteria";
            hysteria.description = "Deal 2 damage to the Opposing party members. Generate 3 blue pigment.";
            hysteria.rarity = 7;
            hysteria.effects = new Effect[2]
            {
                new Effect(ScriptableObject.CreateInstance<DamageEffect>(), 2, IntentType.Damage_1_2, Slots.Front),
                new Effect(ScriptableObject.CreateInstance<GenerateColorManaEffect>(), 3, IntentType.Mana_Generate, Slots.Self)
            };
            hysteria.visuals = LoadedAssetsHandler.LoadEnemy("Mung_EN").abilities[1].ability.visuals;
            ((GenerateColorManaEffect)hysteria.effects[1]._effect).mana = Pigments.Blue;
            ((GenerateColorManaEffect)hysteria.effects[1]._effect).usePreviousExitValue = false;
            hysteria.animationTarget = Slots.Self;

            Ability flip = new Ability();
            flip.name = "Flip";
            flip.description = "Deal a Painful amount of damage to the Left Opposing party member. Move the Right Opposing party member to the right.";
            flip.rarity = 10;
            flip.effects = new Effect[2]
            {
                new Effect(ScriptableObject.CreateInstance<DamageEffect>(), 5, IntentType.Damage_3_6, Slots.LargeSlotTarget(new int[1] { 0 })),
                new Effect(ScriptableObject.CreateInstance<SwapToOneSideEffect>(), 0, IntentType.Swap_Right, Slots.LargeSlotTarget(new int[1] { 1 }))
            };
            ((SwapToOneSideEffect)flip.effects[1]._effect)._swapRight = true;

            Ability flop = new Ability();
            flop.name = "Flop";
            flop.description = "Deal a Painful amount of damage to the Right Opposing party member. Move the Left Opposing party member to the left.";
            flop.rarity = 10;
            flop.effects = new Effect[2]
            {
                new Effect(ScriptableObject.CreateInstance<DamageEffect>(), 5, IntentType.Damage_3_6, Slots.LargeSlotTarget(new int[1] { 1 })),
                new Effect(ScriptableObject.CreateInstance<SwapToOneSideEffect>(), 0, IntentType.Swap_Left, Slots.LargeSlotTarget(new int[1] { 0 }))
            };
            ((SwapToOneSideEffect)flop.effects[1]._effect)._swapRight = false;

            Ability bellow = new Ability();
            bellow.name = "Bellow";
            bellow.description = "Deal a Painful amount of damage to the Opposing party members. Move the Opposing party members away from this enemy.";
            bellow.rarity = 10;
            bellow.effects = new Effect[3]
            {
                new Effect(ScriptableObject.CreateInstance<DamageEffect>(), 4, IntentType.Damage_3_6, Slots.Front),
                new Effect(ScriptableObject.CreateInstance<SwapToOneSideEffect>(), 0, IntentType.Swap_Right, Slots.LargeSlotTarget(new int[1] { 1 })),
                new Effect(ScriptableObject.CreateInstance<SwapToOneSideEffect>(), 0, IntentType.Swap_Left, Slots.LargeSlotTarget(new int[1] { 0 }))
            };
            ((SwapToOneSideEffect)bellow.effects[1]._effect)._swapRight = true;
            ((SwapToOneSideEffect)bellow.effects[2]._effect)._swapRight = false;
            bellow.visuals = LoadedAssetsHandler.LoadEnemy("Flarblet_EN").abilities[2].ability.visuals;
            bellow.animationTarget = Slots.Front;

            Ability snap = new Ability();
            snap.name = "Snap";
            snap.description = "Deal a Painful amount of damage to the Left and Right party members. Move the Left and Right party members closer to this enemy.";
            snap.rarity = 10;
            snap.effects = new Effect[3]
            {
                new Effect(ScriptableObject.CreateInstance<DamageEffect>(), 4, IntentType.Damage_3_6, Slots.LeftRight),
                new Effect(ScriptableObject.CreateInstance<SwapToOneSideEffect>(), 0, IntentType.Swap_Right, Slots.Left),
                new Effect(ScriptableObject.CreateInstance<SwapToOneSideEffect>(), 0, IntentType.Swap_Left, Slots.Right)
            };
            ((SwapToOneSideEffect)snap.effects[1]._effect)._swapRight = true;
            ((SwapToOneSideEffect)snap.effects[2]._effect)._swapRight = false;
            snap.visuals = LoadedAssetsHandler.LoadEnemy("Spoggle_Spitfire_EN").abilities[1].ability.visuals;
            snap.animationTarget = Slots.LeftRight;

            e.abilities[0] = hysteria;
            e.abilities[1] = flip;
            e.abilities[2] = flop;
            e.abilities[3] = bellow;
            e.abilities[4] = snap;
            e.AddEnemy();
        }
    }
}
