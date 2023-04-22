using UnityEngine;

namespace BrutalAPI
{
    public static class Passives
    {
		public static BasePassiveAbilitySO Skittish;
		public static BasePassiveAbilitySO Unstable;
		public static BasePassiveAbilitySO Withering;
		public static BasePassiveAbilitySO Slippery;
		public static BasePassiveAbilitySO Overexert;
		public static BasePassiveAbilitySO Multiattack;
		public static BasePassiveAbilitySO Constricting;
		public static BasePassiveAbilitySO Formless;
		public static BasePassiveAbilitySO Pure;
		public static BasePassiveAbilitySO Absorb;
		public static BasePassiveAbilitySO Forgetful;
		public static BasePassiveAbilitySO Obscured;
		public static BasePassiveAbilitySO Confusion;
		public static BasePassiveAbilitySO Dying;
		public static BasePassiveAbilitySO Inanimate;
		public static BasePassiveAbilitySO Inferno;
		public static BasePassiveAbilitySO Enfeebled;
		public static BasePassiveAbilitySO Immortal;
		public static BasePassiveAbilitySO TwoFaced;
		public static BasePassiveAbilitySO Catalyst;
		public static BasePassiveAbilitySO Anchored;
		public static BasePassiveAbilitySO Delicate;
		public static BasePassiveAbilitySO Leaky;
		public static BasePassiveAbilitySO Transfusion;
		public static BasePassiveAbilitySO Abomination;
		public static BasePassiveAbilitySO Infestation;
		public static BasePassiveAbilitySO Masochism;
		public static BasePassiveAbilitySO Fleeting;
		public static BasePassiveAbilitySO Decay;
		public static BasePassiveAbilitySO UntetheredEssence;
		public static BasePassiveAbilitySO Cashout;
		public static BasePassiveAbilitySO Construct;
		public static BasePassiveAbilitySO BronzosBlessing;
		public static BasePassiveAbilitySO FinancialHyperinflation;
		public static BasePassiveAbilitySO Parasitism;
		public static BasePassiveAbilitySO Mutualism;

        public static void Setup()
        {
            foreach (CharacterSO character in BrutalAPI.vanillaChars)
            {
                switch (character.name)
                {
                    case "Thype_CH":
                        {
                            Withering = character.passiveAbilities[0];
                            break;
                        }
                    case "Rags_CH":
                        {
                            Catalyst = character.passiveAbilities[0];
                            break;
                        }
                    case "Mordrake_CH":
                        {
                            Infestation = character.passiveAbilities[0];
                            break;
                        }
                    case "Gospel_CH":
                        {
                            Inanimate = character.passiveAbilities[0];
                            break;
                        }
                    case "Dimitri_CH":
                        {
                            Inferno = character.passiveAbilities[0];
                            break;
                        }
                    case "Splig_CH":
                        {
                            TwoFaced = character.passiveAbilities[0];
                            UntetheredEssence = character.passiveAbilities[1];
                            break;
                        }
                    case "SmokeStacks_CH":
                        {
                            Leaky = character.passiveAbilities[0];
                            break;
                        }
                    case "Cranes_CH":
                        {
                            Pure = character.passiveAbilities[1];
                            break;
                        }
                    case "Bimini_CH":
                        {
                            Immortal = character.passiveAbilities[0];
                            break;
                        }
                    case "Hans_CH":
                        {
                            Delicate = character.passiveAbilities[0];
                            break;
                        }
                    case "LongLiver_CH":
                        {
                            Constricting = character.passiveAbilities[0];
                            break;
                        }
                    case "Agon_CH":
                        {
                            Dying = character.passiveAbilities[0];
                            break;
                        }
                    case "Anton_CH":
                        {
                            Skittish = character.passiveAbilities[0];
                            break;
                        }
                    case "Doll_CH":
                        {
                            Construct = character.passiveAbilities[0];
                            break;
                        }
                }
            }

            Unstable = LoadedAssetsHandler.GetEnemy("UnfinishedHeir_BOSS").passiveAbilities[3];
            Formless = LoadedAssetsHandler.GetEnemy("TriggerFingers_BOSS").passiveAbilities[0];
            Absorb = LoadedAssetsHandler.GetEnemy("Spoggle_Resonant_EN").passiveAbilities[0];
            Forgetful = LoadedAssetsHandler.GetEnemy("Ouroboros_Head_BOSS").passiveAbilities[1];
            Obscured = LoadedAssetsHandler.GetEnemy("WrigglingSacrifice_EN").passiveAbilities[3];
            Confusion = LoadedAssetsHandler.GetEnemy("Wringle_EN").passiveAbilities[0];
            Enfeebled = LoadedAssetsHandler.GetEnemy("PitifulCorpse_BOSS").passiveAbilities[1];
            Anchored = LoadedAssetsHandler.GetEnemy("Keko_EN").passiveAbilities[2];
            Transfusion = LoadedAssetsHandler.GetEnemy("JumbleGuts_Clotted_EN").passiveAbilities[1];
            Abomination = LoadedAssetsHandler.GetEnemy("OneManBand_EN").passiveAbilities[2];
            Masochism = LoadedAssetsHandler.GetEnemy("ChoirBoy_EN").passiveAbilities[0];
            Fleeting = LoadedAssetsHandler.GetEnemy("Keko_EN").passiveAbilities[0];
            Decay = LoadedAssetsHandler.GetEnemy("MudLung_EN").passiveAbilities[0];
            FinancialHyperinflation = LoadedAssetsHandler.GetEnemy("Bronzo5_EN").passiveAbilities[1];
            Cashout = LoadedAssetsHandler.GetEnemy("Bronzo_MoneyPile_EN").passiveAbilities[0];
            BronzosBlessing = ((AddPassiveEffect)LoadedAssetsHandler.GetEnemy("Bronzo1_EN").enterEffects[3].effect)._passiveToAdd;

            var chordophone = LoadedAssetsHandler.GetEnemy("Chordophone_EN");
            Multiattack = chordophone.passiveAbilities[0];
            Overexert = chordophone.passiveAbilities[1];
            Slippery = chordophone.passiveAbilities[2];

            ParasitePassiveAbility[] parasite = Resources.FindObjectsOfTypeAll<ParasitePassiveAbility>();
            for (int i = 0; i < parasite.Length; i++)
            {
                if (parasite[i]._isFriendly) { Mutualism = parasite[i]; }
                else { Parasitism = parasite[i]; }
            }
        }
    }
}
