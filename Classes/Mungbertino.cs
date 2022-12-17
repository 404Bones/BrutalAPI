using UnityEngine;

namespace BrutalAPI
{
    public static class Mungbertino
    {
        public static void Add()
        {
            //Mungbertino Basics
            Character mungbertino = new Character();

            mungbertino.name = "Mungbertino";
            mungbertino.healthColor = Pigments.Green;
            mungbertino.entityID = (EntityIDs)25915;
            mungbertino.frontSprite = ResourceLoader.LoadSprite("MungbertinoFront");
            mungbertino.backSprite = ResourceLoader.LoadSprite("MungbertinoBack");
            mungbertino.overworldSprite = ResourceLoader.LoadSprite("MungbertinoOverworld", 32, new Vector2(0.5f, 0));
            mungbertino.menuChar = false;
            mungbertino.usesAllAbilities = true;
            mungbertino.usesBaseAbility = true;
            mungbertino.passives = new BasePassiveAbilitySO[1] { Passives.Withering };
            mungbertino.hurtSound = "event:/Characters/Enemies/Mung/CHR_ENM_Mung_Dmg";
            mungbertino.deathSound = "event:/Characters/Enemies/Mung/CHR_ENM_Mung_Dth";

            Ability chewAbility = new Ability();
            chewAbility.name = "Chew";
            chewAbility.description = "Deal 4 damage to the Opposing enemy.";
            chewAbility.cost = new ManaColorSO[1] { Pigments.Red };
            chewAbility.sprite = ResourceLoader.LoadSprite("Chew");
            chewAbility.effects = new Effect[1];

            chewAbility.effects[0] = new Effect(ScriptableObject.CreateInstance<DamageEffect>(),
                4, IntentType.Damage_3_6, Slots.Front);
            chewAbility.visuals = LoadedAssetsHandler.GetEnemy("Mung_EN").abilities[0].ability.visuals;

            Ability braceAbility = new Ability();
            braceAbility.name = "Brace";
            braceAbility.description = "Heal the Left party member, Right party member and Self 2 health.";
            braceAbility.cost = new ManaColorSO[1] { Pigments.Blue };
            braceAbility.sprite = ResourceLoader.LoadSprite("Brace");
            braceAbility.effects = new Effect[1];

            braceAbility.effects[0] = new Effect(ScriptableObject.CreateInstance<HealEffect>(),
                2, IntentType.Heal_1_4, Slots.SlotTarget(new int[3] {-1, 0, 1}, true));

            Ability[] abilities = new Ability[2] {chewAbility, braceAbility };

            mungbertino.AddLevel(6, abilities, 0);
            mungbertino.AddLevel(8, abilities, 1);
            mungbertino.AddLevel(9, abilities, 2);
            mungbertino.AddLevel(10, abilities, 3);
            mungbertino.AddCharacter();
        }
    }
}
