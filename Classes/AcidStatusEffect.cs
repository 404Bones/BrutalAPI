using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public class AcidStatusEffect : StatusEffect
    {
        public static void Add()
        {
            AcidStatusEffect acid = new AcidStatusEffect();

            acid.statusName = "Acid";
            acid.icon = ResourceLoader.LoadSprite("Acid", 32);
            acid.description = "Receive 3 indirect damage upon performing an ability, remove 1 stack of Acid.\n1 stack of Acid is removed at the end of each turn.";
            acid.applied_sound = "";
            acid.removed_sound = "";
            acid.updated_sound = "";
            acid.special_sound01 = "";
            acid.special_sound02 = "";
            acid.locAbilityData = new StringPairData("Acid", "Receive 3 indirect damage upon performing an ability, remove 1 stack of Acid.\n1 stack of Acid is removed at the end of each turn.");
            acid.locID = "en_US";
            acid.EffectType = (StatusEffectType)48512;
            acid.IsPositive = false;

            acid.AddTrigger(acid.EffectType, new CombatTrigger(acid.OnStatusTriggered, TriggerCalls.OnAbilityUsed));

            acid.AddStatus();
        }

        public void OnStatusTriggered(object sender, object args)
        {
            (sender as IUnit).Damage(3, null, DeathType.None, -1, false, false, true, DamageType.Ruptured);
            ReduceDuration(sender as IStatusEffector);
        }

        static public EffectSO ApplyAcidEffect()
        {
            ApplyStatusEffect e = ScriptableObject.CreateInstance<ApplyStatusEffect>();
            e.effectType = (StatusEffectType)48512;
            e.effectClass = typeof(AcidStatusEffect);
            return e;
        }
    }
}
