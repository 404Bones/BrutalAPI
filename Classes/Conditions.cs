using UnityEngine;
using System;

namespace BrutalAPI
{
    public static class Conditions
    {
        public static dynamic Chance<T>(int num)
        {
            if (typeof(T) == typeof(EffectorConditionSO))
            {
                PercentageEffectorCondition c = ScriptableObject.CreateInstance<PercentageEffectorCondition>();
                c.triggerPercentage = num;
                return c;
            }
            else if (typeof(T) == typeof(EffectConditionSO))
            {
                PercentageEffectCondition c = ScriptableObject.CreateInstance<PercentageEffectCondition>();
                c.percentage = num;
                return c;
            } 
            else
            {
                return new Exception("Return type of Chance() not EffectConditionSO or EffectorConditionSO.");
            }
        }

        public static PercentageEffectCondition Chance(int num)
        {
            PercentageEffectCondition c = ScriptableObject.CreateInstance<PercentageEffectCondition>();
            c.percentage = num;
            return c;
        }
    }
}
