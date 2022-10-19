using UnityEngine;

namespace BrutalAPI
{
    class Passives
    {
        public static BasePassiveAbilitySO Withering;
        public static BasePassiveAbilitySO Slippery;
        public static BasePassiveAbilitySO Overexert;
        public static BasePassiveAbilitySO Multiattack;

        public static void Setup()
        {
            foreach (CharacterSO character in BrutalAPI.vanillaChars)
            {
                switch(character.name)
                {
                    case "Thype_CH":
                        {
                            Withering = character.passiveAbilities[0];
                            break;
                        }
                }
            }

            var chordophone = LoadedAssetsHandler.GetEnemy("Chordophone_EN");
            Multiattack = chordophone.passiveAbilities[0];
            Overexert = chordophone.passiveAbilities[1];
            Slippery = chordophone.passiveAbilities[2];
        }
    }
}
