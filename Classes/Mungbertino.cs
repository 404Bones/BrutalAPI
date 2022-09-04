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
            mungbertino.slapAbility = true;
            mungbertino.passives = new BasePassiveAbilitySO[1] { Passives.Withering };
            mungbertino.hurtSound = "event:/Characters/Enemies/Mung/CHR_ENM_Mung_Dmg";
            mungbertino.deathSound = "event:/Characters/Enemies/Mung/CHR_ENM_Mung_Dth";

            mungbertino.AddLevel(6, new Ability[0] { }, 0);
            mungbertino.AddLevel(8, new Ability[0] { }, 1);
            mungbertino.AddLevel(9, new Ability[0] { }, 2);
            mungbertino.AddLevel(10, new Ability[0] { }, 3);
            mungbertino.AddCharacter();
        }
    }
}
