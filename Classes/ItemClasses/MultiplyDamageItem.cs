using UnityEngine;

namespace BrutalAPI
{
    public class MultiplyDamageItem : Item
    {
        public Effect[] effects = new Effect[0];
        public bool immediate = false;
        public bool useDealt = true;
        public bool extraInt = false;
        public int multiplier = 1;
        public override BaseWearableSO Wearable()
        {
            PerformEffectWithMultiplierDamageModifierSetterWearable w = ScriptableObject.CreateInstance<PerformEffectWithMultiplierDamageModifierSetterWearable>();
            w.BaseWearable(this);

            w._effects = ExtensionMethods.ToEffectInfoArray(effects);
            w._immediateEffect = immediate;
            w._toMultiply = multiplier;
            w._useSimpleInt = extraInt;
            w._useDealt = useDealt; 

            return w;
        }
    }
}
