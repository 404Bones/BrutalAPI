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

        private static Dictionary<ManaColorSO[], ManaColorSO> splitCosts = new Dictionary<ManaColorSO[], ManaColorSO>();

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
                            Purple.manaSprite = ResourceLoader.LoadSprite("ManaPurple", 32);
                            Purple.manaUsedSprite = ResourceLoader.LoadSprite("ManaUsedPurple");
                            Purple.manaCostSelectedSprite = ResourceLoader.LoadSprite("ManaCostSelectedPurple");
                            Purple.manaCostSprite = ResourceLoader.LoadSprite("ManaCostPurple");
                            break;
                        }
                    case "Dimitri_CH":
                        {
                            Yellow = i.healthColor;
                            Yellow.manaSprite = ResourceLoader.LoadSprite("ManaYellow", 32);
                            Yellow.manaUsedSprite = ResourceLoader.LoadSprite("ManaUsedYellow");
                            Yellow.manaCostSelectedSprite = ResourceLoader.LoadSprite("ManaCostSelectedYellow");
                            Yellow.manaCostSprite = ResourceLoader.LoadSprite("ManaCostYellow");
                            break;
                        }
                    case "Burnout_CH":
                        {
                            Red = i.healthColor;
                            Red.manaSprite = ResourceLoader.LoadSprite("ManaRed", 32);
                            Red.manaUsedSprite = ResourceLoader.LoadSprite("ManaUsedRed");
                            Red.manaCostSelectedSprite = ResourceLoader.LoadSprite("ManaCostSelectedRed");
                            Red.manaCostSprite = ResourceLoader.LoadSprite("ManaCostRed");
                            break;
                        }
                    case "Cranes_CH":
                        {
                            Blue = i.healthColor;
                            Blue.manaSprite = ResourceLoader.LoadSprite("ManaBlue", 32);
                            Blue.manaUsedSprite = ResourceLoader.LoadSprite("ManaUsedBlue");
                            Blue.manaCostSelectedSprite = ResourceLoader.LoadSprite("ManaCostSelectedBlue");
                            Blue.manaCostSprite = ResourceLoader.LoadSprite("ManaCostBlue");
                            break;
                        }
                    case "Gospel_CH":
                        {
                            Gray = i.healthColor;
                            Gray.manaSprite = ResourceLoader.LoadSprite("ManaGray", 32);
                            Gray.manaUsedSprite = ResourceLoader.LoadSprite("ManaUsedGray");
                            Gray.manaCostSelectedSprite = ResourceLoader.LoadSprite("ManaCostSelectedGray");
                            Gray.manaCostSprite = ResourceLoader.LoadSprite("ManaCostGray");
                            break;
                        }
                }
            }

            Green = ScriptableObject.CreateInstance<ManaColorSO>();
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

        public static ManaColorSO SplitPigment(ManaColorSO mana1, ManaColorSO mana2)
        {
            ManaColorSO newMana = ScriptableObject.CreateInstance<ManaColorSO>();
            var array = new ManaColorSO[2] { mana1, mana2 };

            if (splitCosts.ContainsKey(array))
            {
                splitCosts.TryGetValue(array, out newMana);
                return newMana;
            }

            newMana.canGenerateMana = mana1.canGenerateMana;
            newMana.healthColor = mana1.healthColor;
            newMana.pigmentType = mana1.pigmentType | mana2.pigmentType;
            newMana.manaSoundEvent = mana1.manaSoundEvent;

            //Sprites
            newMana.manaSprite = CombinedSprite(mana1.manaSprite, mana2.manaSprite);
            newMana.manaUsedSprite = CombinedSprite(mana1.manaUsedSprite, mana2.manaUsedSprite);
            newMana.manaCostSelectedSprite = CombinedSprite(mana1.manaCostSelectedSprite, mana2.manaCostSelectedSprite);
            newMana.manaCostSprite = CombinedSprite(mana1.manaCostSprite, mana2.manaCostSprite);
            newMana.healthSprite = mana1.healthSprite;

            splitCosts.Add(array, newMana);

            return newMana;
        }

        private static Sprite CombinedSprite(Sprite spr1, Sprite spr2)
        {
            Texture2D finalTex = new Texture2D(spr1.texture.width, spr1.texture.height, TextureFormat.ARGB32, false)
            {
                anisoLevel = 1,
                filterMode = 0
            };

            finalTex.SetPixels(0, 0, finalTex.width / 2, finalTex.height, spr1.texture.GetPixels(0, 0, finalTex.width / 2, finalTex.height));
            finalTex.SetPixels(finalTex.width / 2, 0, finalTex.width / 2, finalTex.height, spr2.texture.GetPixels(finalTex.width / 2, 0, finalTex.width / 2, finalTex.height));
            finalTex.Apply();
            return Sprite.Create(finalTex, new Rect(0, 0, spr1.texture.width, spr1.texture.height), new Vector2(0.5f, 0.5f));         
        }
    }
}
