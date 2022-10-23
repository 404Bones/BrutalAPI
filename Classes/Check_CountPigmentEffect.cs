using System;
using System.Collections.Generic;
using System.Text;

namespace BrutalAPI
{
    public class Check_CountPigmentEffect : EffectSO
    {
        public ManaColorSO _colourPigment = null;
        public bool _includeGenerator = false;
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = stats.MainManaBar.CountColorPigment(_colourPigment);
            if (_includeGenerator)
                exitAmount += stats.YellowManaBar.CountColorPigment(_colourPigment);

            return exitAmount > 0;
        }
    }

}
