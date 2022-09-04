using System.Collections.Generic;
using UnityEngine;

namespace BrutalAPI
{
    public static class Pigments
    {
        public static ManaColorSO Red;
        public static ManaColorSO Yellow;
        public static ManaColorSO Blue;
        public static ManaColorSO Purple;
        public static ManaColorSO Gray;
        public static ManaColorSO Green;

        public static void Setup()
        {
            CharacterDataBase charDataBase = LoadedAssetsHandler.GetCharacterDB();
            List<string> charNamelist = charDataBase._characters;

            foreach (string i in charNamelist)
            {
                BrutalAPI.vanillaChars.Add(LoadedAssetsHandler.GetCharcater(i));
            }

            foreach (CharacterSO i in BrutalAPI.vanillaChars)
            {
                switch (i.name)
                {
                    case "Hans_CH":
                        {
                            BrutalAPI.slapCharAbility = i.basicCharAbility;
                            break;
                        }

                    case "Nowak_CH":
                        {
                            Purple = i.healthColor;
                            break;
                        }
                    case "Dimitri_CH":
                        {
                            Yellow = i.healthColor;
                            break;
                        }
                    case "Burnout_CH":
                        {
                            Red = i.healthColor;
                            break;
                        }
                    case "Cranes_CH":
                        {
                            Blue = i.healthColor;
                            break;
                        }
                    case "Gospel_CH":
                        {
                            Gray = i.healthColor;
                            break;
                        }
                }
            }

            Green = ScriptableObject.CreateInstance(typeof(ManaColorSO)) as ManaColorSO;
            Green.canGenerateMana = true;
            Green.dealsCostDamage = true;
            Green.pigmentType = PigmentType.Green;
            Green.manaSprite = ResourceLoader.LoadSprite("GreenMana", 32);
            Green.manaUsedSprite = ResourceLoader.LoadSprite("GreenManaUsed");
            Green.manaCostSelectedSprite = ResourceLoader.LoadSprite("GreenManaCostSelected");
            Green.manaCostSprite = ResourceLoader.LoadSprite("GreenManaCostUnselected");
            Green.manaSoundEvent = Red.manaSoundEvent;
            Green.healthSprite = ResourceLoader.LoadSprite("GreenManaHealth");
        } 
    }
}
