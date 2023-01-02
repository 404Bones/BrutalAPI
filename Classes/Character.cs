using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BrutalAPI
{
    public class Character
    {
        public CharacterSO c = ScriptableObject.CreateInstance(typeof(CharacterSO)) as CharacterSO;
        public SelectableCharacterData charData = new SelectableCharacterData("Belm", ResourceLoader.LoadSprite("BasicPartyMemberUnlocked"), ResourceLoader.LoadSprite("BasicPartyMemberLocked"));
        const BindingFlags AllFlags = (BindingFlags)(-1);

        //Basics
        public string name = "Belm";
        public string characterID = "";
        public EntityIDs entityID;
        public ManaColorSO healthColor = Pigments.Purple;

        //Abilities
        public bool usesBaseAbility = true;
        public Ability baseAbility = null;
        public bool usesAllAbilities = false;
        public BasePassiveAbilitySO[] passives = new BasePassiveAbilitySO[0];
        public CharacterRankedData[] levels = new CharacterRankedData[4];

        //Visuals
        public Sprite frontSprite = ResourceLoader.LoadSprite("BasicPartyMemberFront");
        public Sprite backSprite = ResourceLoader.LoadSprite("BasicPartyMemberBack");
        public Sprite overworldSprite = ResourceLoader.LoadSprite("BasicPartyMemberOverworld");
        public Sprite lockedSprite = ResourceLoader.LoadSprite("BasicPartyMemberLocked");
        public Sprite unlockedSprite = ResourceLoader.LoadSprite("BasicPartyMemberUnlocked");
        public ExtraCharacterCombatSpritesSO extraSprites;
        public bool walksInOverworld = true;
        public bool isSecret = false;
        public bool menuChar = true;
        public bool isSupport = false;
        public bool appearsInShops = true;
        public List<int> ignoredAbilities = new List<int>();

        //Audio
        public string hurtSound = "";
        public string deathSound = "";
        public string dialogueSound = "";

        //Unlocks
        public UnlockableID heavenUnlock = UnlockableID.None;
        public UnlockableID osmanUnlock = UnlockableID.None;

        public void AddLevel(int health, Ability[] abilities, int level)
        {
            CharacterRankedData data = new CharacterRankedData();
            data.rankAbilities = new CharacterAbility[abilities.Length];
            data.health = health;
            for (int i = 0; i < abilities.Length; i++)
            {
                data.rankAbilities[i] = abilities[i].CharacterAbility();
            }
            levels[level] = data;
        }

        public void AddCharacter()
        {
            charData.LoadedCharacter = c;
            charData._characterName = characterID == "" ? name + "_CH" : characterID;
            if (menuChar)
            {
                charData._portrait = unlockedSprite;
                charData._noPortrait = lockedSprite;
            }
            charData._isSecret = isSecret;

            c.name = charData._characterName;
            c._characterName = name;
            c.characterEntityID = entityID;
            c.healthColor = healthColor;
            c.usesBasicAbility = usesBaseAbility;
            c.basicCharAbility = baseAbility == null ? BrutalAPI.slapCharAbility : baseAbility.CharacterAbility();
            c.usesAllAbilities = usesAllAbilities;
            c.rankedData = levels;
            c.passiveAbilities = passives;
            c.characterSprite = frontSprite;
            c.characterBackSprite = backSprite;
            c.extraCombatSprites = extraSprites;
            c.characterOWSprite = overworldSprite;
            c.movesOnOverworld = walksInOverworld;
            c.damageSound = hurtSound;
            c.deathSound = deathSound;
            c.speakerDataName = name;
            c.dxSound = dialogueSound;

            //Add character to menu
            if (menuChar)
            {
                SelectableCharacterData[] oldData = BrutalAPI.selCharsSO._characters;
                List<SelectableCharacterData> charList = new List<SelectableCharacterData>();
                foreach (SelectableCharacterData i in oldData)
                {
                    charList.Add(i);
                }
                charList.Add(charData);
                BrutalAPI.selCharsSO._characters = charList.ToArray();

                //Selection Bias
                if (isSupport) BrutalAPI.selCharsSO._supportCharacters.Add(new CharacterRefString(charData._characterName), new CharacterIgnoredAbilities() { ignoredAbilities = ignoredAbilities });
                else BrutalAPI.selCharsSO._dpsCharacters.Add(new CharacterRefString(charData._characterName), new CharacterIgnoredAbilities() { ignoredAbilities = ignoredAbilities });
            }

            //Remove character from shops
            if (!appearsInShops)
            {
                for (int i = 0; i < BrutalAPI.hardAreas.Count; i++)
                {
                    BrutalAPI.hardAreas[i]._omittedCharacters.Add(charData._characterName);
                }
                for (int i = 0; i < BrutalAPI.easyAreas.Count; i++)
                {
                    BrutalAPI.easyAreas[i]._omittedCharacters.Add(charData._characterName);
                }
            }

            //Unlock character
            if (!LoadedAssetsHandler.LoadedCharacters.ContainsKey(charData._characterName))
            {
                LoadedAssetsHandler.LoadedCharacters.Add(charData._characterName, c);
            }

            //Add unlocks
            if (heavenUnlock != UnlockableID.None)
            {
                BrutalAPI.unlockablesDatabase._heavenIDs.Add(entityID, heavenUnlock);
            }
            if (osmanUnlock != UnlockableID.None)
            {
                BrutalAPI.unlockablesDatabase._osmanIDs.Add(entityID, osmanUnlock);
            }

            Debug.Log("Added character " + c._characterName);
            BrutalAPI.moddedChars.Add(c);
        }
    }
}
