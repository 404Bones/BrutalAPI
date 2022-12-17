using UnityEngine;

namespace BrutalAPI
{
    public class EffectItem : Item
    {
        public Effect[] effects = new Effect[0];
        public bool immediate = false;
        public bool addResultToEffect = false;

        public override BaseWearableSO Wearable()
        {
            PerformEffectWearable w = ScriptableObject.CreateInstance<PerformEffectWearable>();
            w.BaseWearable(this);
            w.effects = ExtensionMethods.ToEffectInfoArray(effects);
            w._immediateEffect = immediate;

            return w;
        }
    }
}
