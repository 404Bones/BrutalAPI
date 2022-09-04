using UnityEngine;

namespace BrutalAPI
{
    public class MultiplyDamageItem : Item
    {
        public ConditionEffect[] effects = new ConditionEffect[0];
        public bool immediate = false;
        public bool useDealt = true;
        public bool extraInt = false;
        public int multiplier = 1;
        public override BaseWearableSO Wearable()
        {
            PerformEffectWithMultiplierDamageModifierSetterWearable w = ScriptableObject.CreateInstance<PerformEffectWithMultiplierDamageModifierSetterWearable>();
            w.BaseWearable(this);

            w._effects = effects.ConditionEffectInfoArray();
            w._immediateEffect = immediate;
            w._toMultiply = multiplier;
            w._useSimpleInt = extraInt;
            w._useDealt = useDealt; 

            return w;
        }
    }
}
