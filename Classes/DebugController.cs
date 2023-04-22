using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Reflection;
using System;
using MonoMod.RuntimeDetour;

namespace BrutalAPI
{
    public class DebugController : MonoBehaviour
    {
        const BindingFlags AllFlags = (BindingFlags)(-1);
        public static DebugController _instance;

        bool showConsole = false;
        string input;
        string output;

        List<string> history = new List<string>();
        int historyID;

        List<DebugCommand> commandList;
        public static DebugCommand HELP;
        public static DebugCommand GIVE;
        public static DebugCommand ADDFOOL;
        public static DebugCommand SPAWN;
        public static DebugCommand DAMAGE;
        public static DebugCommand HEAL;
        public static DebugCommand LEVELUP;
        public static DebugCommand MONEY;
        public static DebugCommand TP;
        public static DebugCommand ENDCOMBAT;
        public static DebugCommand PIGMENT;

        public static DebugController Instance
        {
            get
            {
                if (DebugController._instance == null)
                {
                    Debug.Log("WHAT");
                    DebugController._instance = new GameObject("DebugController").AddComponent<DebugController>();
                }
                return DebugController._instance;
            }
        }


        public void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            IDetour OpenConsoleHook = new Hook(
                    typeof(Keyboard).GetMethod("OnTextInput", AllFlags),
                    typeof(DebugController).GetMethod("KeyPressed", AllFlags));

            //Help command
            HELP = new DebugCommand("help", "Shows available commands and information about them.", "help (command)", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 1)
                {
                    string commands = "";
                    for (int i = 0; i < commandList.Count; i++)
                    {
                        if (i == commandList.Count - 1)
                            commands += commandList[i].commandId;
                        else
                            commands += commandList[i].commandId + ", ";
                    }

                    WriteLine("Available commands: " + commands);
                    return;
                } 
                else if (parameters != null && parameters.Length == 1)
                {
                    for (int i = 0; i < commandList.Count; i++)
                    {
                        if (commandList[i].commandId == parameters[0])
                        {
                            WriteLine(commandList[i].commandDescription + " Usage: " + commandList[i].commandFormat);
                            return;
                        }
                    }
                    WriteLine("Unknown command");
                    return;
                } else
                { WriteLine("Syntax error: " + HELP.commandFormat); return; }
            });

            //Give command
            GIVE = new DebugCommand("give", "Adds an item to your inventory.", "give <item_id>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 1)
                { WriteLine("Syntax error: " + GIVE.commandFormat); return; }

                if (!LoadedAssetsHandler.LoadedWearables.ContainsKey(parameters[0]))
                { WriteLine("Unknown Item"); return; }

                BrutalAPI.mainMenuController._informationHolder.Run.playerData.AddNewItem(LoadedAssetsHandler.GetWearable(parameters[0]));
                { WriteLine("Added item " + parameters[0] + " to your inventory"); return; }
            });

            //AddFool command
            ADDFOOL = new DebugCommand("addfool", "Adds a fool to your party.", "addfool <fool_id> <rank> <abilityOne> <abilityTwo>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 4)
                { WriteLine("Syntax error: " + ADDFOOL.commandFormat); return; }

                if (!LoadedAssetsHandler.LoadedCharacters.ContainsKey(parameters[0]))
                { WriteLine("Unknown Character"); return; }

                int rank;
                int ability1;
                int ability2;

                if (!int.TryParse(parameters[1], out rank))
                { WriteLine("Rank is not a number"); return; }

                if (!int.TryParse(parameters[2], out ability1))
                { WriteLine("Ability 1 is not a number"); return; }

                if (!int.TryParse(parameters[3], out ability2))
                { WriteLine("Ability 2 is not a number"); return; }

                if (rank > 3)
                { WriteLine("A party member can't have a rank above 3"); return; }

                BrutalAPI.mainMenuController._informationHolder.Run.playerData.AddNewCharacter(LoadedAssetsHandler.GetCharcater(parameters[0]), rank, ability1, ability2);
                { WriteLine("Added fool " + parameters[0] + " to your party"); return; }
            });

            //Spawn command
            SPAWN = new DebugCommand("spawn", "Spawns an enemy in battle.", "spawn <enemy_id> <slot>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 2)
                { WriteLine("Syntax error: " + SPAWN.commandFormat); return; }

                int slot;
                string enemyid = parameters[0];

                if (Resources.Load(Tools.PathUtils.enemiesResPath + "/" + enemyid) as EnemySO == null &&
                !LoadedAssetsHandler.LoadedEnemies.ContainsKey(enemyid))
                { WriteLine("Unknown Enemy"); return; }

                if (!int.TryParse(parameters[1], out slot))
                { WriteLine("Slot is not a number"); return; }

                if (!CombatManager.Instance._combatInitialized)
                { WriteLine("Can't spawn an enemy when out of combat"); return; }

                CombatManager.Instance.AddPriorityRootAction(new SpawnEnemyAction(LoadedAssetsHandler.GetEnemy(enemyid), slot, true, true, SpawnType.Basic));
                { WriteLine("Spawned enemy " + parameters[0] + " in slot " + slot.ToString()); return; }
            });

            //Damage command
            DAMAGE = new DebugCommand("damage", "Damages an enemy or fool.", "damage <damage> <slot> <targetsAllies>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 3)
                { WriteLine("Syntax error: " + DAMAGE.commandFormat); return; }

                int slot;
                int damage;
                bool targetAllies;

                if (!int.TryParse(parameters[0], out damage))
                { WriteLine("Damage is not a number"); return; }

                if (!int.TryParse(parameters[1], out slot))
                { WriteLine("Slot is not a number"); return; }

                if (!bool.TryParse(parameters[2], out targetAllies))
                { WriteLine("TargetsAllies is not true or false"); return; }

                if (!CombatManager.Instance._combatInitialized)
                { WriteLine("Can't damage when out of combat"); return; }

                GenericTargetting_BySlot_Index target = ScriptableObject.CreateInstance<GenericTargetting_BySlot_Index>();
                target.slotPointerDirections = new int[1] {slot};
                target.getAllies = targetAllies;

                Effect killdmg = new Effect(ScriptableObject.CreateInstance<DamageEffect>(), damage, IntentType.Damage_Death, target);

                CombatManager.Instance.AddPriorityRootAction(new EffectAction(ExtensionMethods.ToEffectInfoArray(new Effect[1] {killdmg}),
                    CombatManager.Instance._stats.CharactersOnField.First().Value));
                
                { WriteLine("Damaged entity in slot " + slot.ToString() + " for " + damage.ToString() + " damage"); return; }
            });

            //Heal command
            HEAL = new DebugCommand("heal", "Heals an enemy or fool.", "heal <amount> <slot> <targetsAllies>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 3)
                { WriteLine("Syntax error: " + HEAL.commandFormat); return; }

                int slot;
                int amount;
                bool targetAllies;

                if (!int.TryParse(parameters[0], out amount))
                { WriteLine("Amount is not a number"); return; }

                if (!int.TryParse(parameters[1], out slot))
                { WriteLine("Slot is not a number"); return; }

                if (!bool.TryParse(parameters[2], out targetAllies))
                { WriteLine("TargetsAllies is not true or false"); return; }

                if (!CombatManager.Instance._combatInitialized)
                { WriteLine("Can't heal when out of combat"); return; }

                GenericTargetting_BySlot_Index target = ScriptableObject.CreateInstance<GenericTargetting_BySlot_Index>();
                target.slotPointerDirections = new int[1] { slot };
                target.getAllies = targetAllies;

                Effect heal = new Effect(ScriptableObject.CreateInstance<HealEffect>(), amount, IntentType.Heal_11_20, target);

                CombatManager.Instance.AddPriorityRootAction(new EffectAction(ExtensionMethods.ToEffectInfoArray(new Effect[1] { heal }),
                    CombatManager.Instance._stats.CharactersOnField.First().Value));

                { WriteLine("Healed entity in slot " + slot.ToString() + " for " + amount.ToString() + " HP"); return; }
            });

            //Level up command
            LEVELUP = new DebugCommand("lvlup", "Changes the level of a fool.", "lvlup <partySlot>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 1)
                { WriteLine("Syntax error: " + LEVELUP.commandFormat); return; }

                int slot;

                if (!int.TryParse(parameters[0], out slot))
                { WriteLine("Slot is not a number"); return; }

                BrutalAPI.mainMenuController._informationHolder.Run.playerData.RankUpCharacter(slot, 0);
                IMinimalCharacterInfo character = BrutalAPI.mainMenuController._informationHolder.Run.playerData.GetCharacterFromPartySlot(slot);

                { WriteLine("Leveled up " + character.Character._characterName); return; }
            });

            //Money command
            MONEY = new DebugCommand("money", "Adds coins.", "money <amount>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 1)
                { WriteLine("Syntax error: " + MONEY.commandFormat); return; }

                int amount;

                if (!int.TryParse(parameters[0], out amount))
                { WriteLine("Money is not a number"); return; }

                BrutalAPI.mainMenuController._informationHolder.Run.playerData.AddCurrency(amount);

                { WriteLine("Added " + amount.ToString() + " coins"); return; }
            });

            //Teleport command
            TP = new DebugCommand("tp", "Teleports you to an area.", "tp <areaID> <restartArea>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 2)
                { WriteLine("Syntax error: " + TP.commandFormat); return; }

                int ID;
                bool reroll;

                if (!int.TryParse(parameters[0], out ID))
                { WriteLine("Area ID is not a number"); return; }

                if (!bool.TryParse(parameters[1], out reroll))
                { WriteLine("Restart Area is not true or false"); return; }

                BrutalAPI.ChangeArea((Areas)ID, reroll);

                { WriteLine("Teleported to " + BrutalAPI.mainMenuController._informationHolder.Run.CurrentZoneDB.ZoneName); return; }
            });

            //ENDCOMBAT
            ENDCOMBAT = new DebugCommand("endcombat", "Instantly ends the current combat.", "endcombat", (string[] parameters) =>
            {
                if (parameters.Length < 0)
                { WriteLine("Syntax error: " + ENDCOMBAT.commandFormat); return; }

                var firstChar = CombatManager.Instance._stats.CharactersOnField.First().Value;
                CombatEndEffect combatEndEffect = ScriptableObject.CreateInstance<CombatEndEffect>();
                combatEndEffect._ignoreLoot = false;
                Effect effect = new Effect(combatEndEffect, 0, null, Slots.Front);

                CombatManager.Instance.AddPriorityRootAction(new EffectAction(ExtensionMethods.ToEffectInfoArray(new Effect[] { effect }), firstChar));
                WriteLine("Ended combat"); return;
            });

            //PIGMENT
            PIGMENT = new DebugCommand("genpigment", "Generates the input pigment.", "genpigment <pigmentID> <pigmentID> <pigmentID>...", (string[] parameters) =>
            {
                if (!CombatManager.Instance._combatInitialized)
                { WriteLine("Can't generate pigment when out of combat"); return; }

                if (parameters == null || parameters.Length == 0)
                { WriteLine("Syntax error: " + PIGMENT.commandFormat); return; }

                Effect[] manaEffects = new Effect[parameters.Length];
                var firstChar = CombatManager.Instance._stats.CharactersOnField.First().Value;

                for (int i = 0; i < parameters.Length; i++)
                {
                    GenerateColorManaEffect colorManaEffect = ScriptableObject.CreateInstance<GenerateColorManaEffect>();

                    int ID;
                    if (!int.TryParse(parameters[i], out ID))
                    { WriteLine("ID is not a number"); return; }

                    ManaColorSO manaColor;
                    if (!Pigments.PigmentDatabase.TryGetValue((PigmentType)ID, out manaColor))
                    { WriteLine("ID is not a valid pigment ID"); return; }

                    colorManaEffect.mana = manaColor;
                    Effect effect = new Effect(colorManaEffect, 1, null, Slots.Self);
                    manaEffects[i] = effect;

                }
                
                CombatManager.Instance.AddPriorityRootAction(new EffectAction(ExtensionMethods.ToEffectInfoArray(manaEffects), firstChar));

                WriteLine("Generated " + parameters.Length + " pigment"); return;
            });


            commandList = new List<DebugCommand>
            {
                HELP,
                GIVE,
                ADDFOOL,
                SPAWN,
                DAMAGE,
                HEAL,
                LEVELUP,
                MONEY,
                TP,
                ENDCOMBAT,
                PIGMENT
            };

            Debug.Log("Debug Console Ready");
        }

        public void Update()
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
                HandleInput();

            if (Keyboard.current.upArrowKey.wasPressedThisFrame && history.Count > 0) 
            {
                input = history[historyID];
                historyID -= 1;
                historyID = Mathf.Clamp(historyID, 0, history.Count - 1);
            }
                
            if (Keyboard.current.downArrowKey.wasPressedThisFrame && history.Count > 0)
            {
                input = history[historyID];
                historyID += 1;
                historyID = Mathf.Clamp(historyID, 0, history.Count - 1);  
            }
        }

        public static void KeyPressed(Action<Keyboard, char> orig, Keyboard self, char c)
        {
            orig(self, c);

            if (c == BrutalAPI.openDebugConsoleKey)
                DebugController.Instance.showConsole = !DebugController.Instance.showConsole;
        }

        public void OnGUI()
        {
            if (!showConsole) return;

            GUI.Box(new Rect(0, 0, Screen.width, 110), "");
            GUI.Box(new Rect(0, 110, Screen.width, 30), "");
            
            input = GUI.TextField(new Rect(10f, 115f, Screen.width - 20f, 20f), input);
            GUI.TextArea(new Rect(10f, 5f, Screen.width - 20f, 105f), output);
        }

        public void HandleInput()
        {
            history.Add(input);
            historyID = history.Count - 1;

            string[] commandstr = input.Split(' ');
            string[] parameters = commandstr.Skip(1).ToArray();

            for (int i = 0; i < commandList.Count; i++)
            {
                if (commandstr[0] == commandList[i].commandId)
                {
                    if (parameters.Length <= 0 && commandstr[0] != "help" && commandstr[0] != "wincombat")
                        WriteLine("No parameters given for the specified command");
                    else
                        commandList[i].Invoke(parameters);
                    goto ExecutedCommand;
                }
            }
            WriteLine("Unknown command");

            ExecutedCommand:
                input = "";
        }

        public void WriteLine(string s)
        {
            output += "\n" + s;
        }
    }
}
