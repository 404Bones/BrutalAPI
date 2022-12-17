using UnityEngine;

namespace BrutalAPI
{
    public static class Conditions
    {
        public static PercentageEffectCondition Chance(int num)
        {
            PercentageEffectCondition c = ScriptableObject.CreateInstance<PercentageEffectCondition>();
            c.percentage = num;
            return c;
        }
    }
}
