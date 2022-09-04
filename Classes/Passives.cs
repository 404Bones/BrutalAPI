using System;
using System.Collections.Generic;
using System.Text;

namespace BrutalAPI
{
    class Passives
    {
        public static BasePassiveAbilitySO Withering;

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
        }
    }
}
