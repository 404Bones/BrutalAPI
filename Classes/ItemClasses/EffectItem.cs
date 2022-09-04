using UnityEngine;

namespace BrutalAPI
{
    public class EffectItem : Item
    {
        public ConditionEffect[] effects = new ConditionEffect[0];
        public bool immediate = false;
        public bool addResultToEffect = false;

        public override BaseWearableSO Wearable()
        {
            PerformEffectWearable w = ScriptableObject.CreateInstance<PerformEffectWearable>();
            w.BaseWearable(this);
            w.effects = effects.ConditionEffectInfoArray();
            w._immediateEffect = immediate;

            return w;
        }
    }
}
