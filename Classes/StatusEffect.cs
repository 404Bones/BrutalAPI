using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
	public abstract class StatusEffect : IStatusEffect, ITriggerEffect<IStatusEffector>
	{
		//Status information

		public string statusName = "";
		public Sprite icon;
		public string description = "";
		public string applied_sound = "";
		public string removed_sound = "";
		public string updated_sound = "";
		public string special_sound01 = "";
		public string special_sound02 = "";
		public StringPairData locAbilityData;
		public string locID = "";
		public StatusEffectType EffectType { get; set; }
		public int Restrictor { get; set; } = 0;
		public int Duration { get; set; } = 1;
		public bool IsPositive { get; set; }
		public StatusEffectInfoSO EffectInfo
        {
            get
            {
				StatusEffectInfoSO s = ScriptableObject.CreateInstance<StatusEffectInfoSO>();
				s._statusName = statusName;
				s.icon = icon;
				s.statusEffectType = EffectType;
				s._description = description;
				s._applied_SE_Event = applied_sound;
				s._removed_SE_Event = removed_sound;
				s._special01_SE_Event = special_sound01;
				s._special02_SE_Event = special_sound02;
				s._locAbilityData = locAbilityData;
				s._locID = locID;
				return s;
			}

            set
            {
				statusName = value._statusName;
				icon = value.icon;
				EffectType = value.statusEffectType;
				description = value._description;
				applied_sound = value._applied_SE_Event;
				removed_sound = value._removed_SE_Event;
				special_sound01 = value._special01_SE_Event;
				special_sound02 = value._special02_SE_Event;
				locAbilityData = value._locAbilityData;
				locID = value._locID;
			}
        }

		//Virtual Methods

		public virtual int StatusContent { get { return Duration; } set { Duration = value; } }
		virtual public bool AddContent(IStatusEffect content)
        {
			Duration += content.StatusContent;
			Restrictor += content.Restrictor;
			return true;
		}
		virtual public bool CanBeRemoved
		{
			get
			{
				return Restrictor <= 0;
			}
		}
		virtual public bool TryAddContent(int amount)
        {
			if (Duration <= 0)
			{
				return false;
			}
			Duration += amount;
			return true;
		}
		virtual public int JustRemoveAllContent()
        {
			int dur = Duration;
			Duration = 0;
			return dur;
		}
		public virtual string DisplayText
		{
			get
			{
				string text = "";
				if (Duration > 0)
				{
					text += Duration.ToString();
				}
				if (Restrictor > 0)
				{
					text = text + "(" + Restrictor.ToString() + ")";
				}
				return text;
			}
		}
		virtual public void DettachRestrictor(IStatusEffector effector)
		{
			int res = Restrictor;
			Restrictor = res - 1;
			if (!TryRemoveStatusEffect(effector))
			{
				effector.StatusEffectValuesChanged(EffectType, 0);
			}
		}
		virtual public bool TryRemoveStatusEffect(IStatusEffector effector)
		{
			if (Duration <= 0 && CanBeRemoved)
			{
				effector.RemoveStatusEffect(EffectType);
				return true;
			}
			return false;
		}
		virtual public bool CanReduceDuration
		{
			get
			{
				BooleanReference booleanReference = new BooleanReference(true);
				CombatManager.Instance.ProcessImmediateAction(new CheckHasStatusFieldReductionBlockIAction(booleanReference), false);
				return !booleanReference.value;
			}
		}
        public virtual void OnTriggerAttached(IStatusEffector caller)
        {
			List<CombatTrigger> triggers = BrutalAPI.statusTriggers[EffectType];
			for (int i = 0; i < triggers.Count; i++)
            {
				AddTrigger(triggers[i]._method, triggers[i]._trigger, caller);
            }
        }
		public virtual void OnTriggerDettached(IStatusEffector caller)
        {
			List<CombatTrigger> triggers = BrutalAPI.statusTriggers[EffectType];
			for (int i = 0; i < triggers.Count; i++)
			{
				RemoveTrigger(triggers[i]._method, triggers[i]._trigger, caller);
			}
		}

		//Public Methods

		public void ReduceDuration(IStatusEffector effector)
		{
			if (!CanReduceDuration)
			{
				return;
			}
			int dur = Duration;
			Duration = Mathf.Max(0, Duration - 1);
			if (!TryRemoveStatusEffect(effector) && dur != Duration)
			{
				effector.StatusEffectValuesChanged(EffectType, Duration - dur);
			}
		}

		public void AddStatus()
        {
			BrutalAPI.moddedStatusEffects.Add(EffectInfo);
        }

        public void SetEffectInformation(StatusEffectInfoSO effectInfo)
        {
			EffectInfo = effectInfo;
        }

        public void OnSubActionTrigger(object sender, object args, bool stateCheck)
        {
            throw new NotImplementedException();
        }

		public void AddTrigger(Action<object, object> method, TriggerCalls trigger, IStatusEffector caller)
        {
			CombatManager.Instance.AddObserver(new Action<object, object>(method), trigger.ToString(), caller);
		}

		public void AddTrigger(StatusEffectType effectType, CombatTrigger trigger)
		{
			if (BrutalAPI.statusTriggers.ContainsKey(effectType))
            {
				List<CombatTrigger> list;
				BrutalAPI.statusTriggers.TryGetValue(effectType, out list);
				list.Add(trigger);
				BrutalAPI.statusTriggers[effectType] = list;
			}
			else
            {
				BrutalAPI.statusTriggers.Add(effectType, new List<CombatTrigger>() { trigger });
			}
		}

		public void RemoveTrigger(Action<object, object> method, TriggerCalls trigger, IStatusEffector caller)
		{
			CombatManager.Instance.RemoveObserver(new Action<object, object>(method), trigger.ToString(), caller);
		}

	}

    class ApplyStatusEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
			exitAmount = 0;
			if (entryVariable <= 0)
			{
				return false;
			}
			StatusEffectInfoSO effectInformation;
			stats.statusEffectDataBase.TryGetValue(effectType, out effectInformation);
			for (int i = 0; i < targets.Length; i++)
			{
				if (targets[i].HasUnit)
				{
					IStatusEffect statusEffect = (IStatusEffect)Activator.CreateInstance(effectClass);

					statusEffect.SetEffectInformation(effectInformation);

					((StatusEffect)statusEffect).StatusContent = entryVariable;
					((StatusEffect)statusEffect).Restrictor = 0;

					//3, 0

					if (targets[i].Unit.ApplyStatusEffect(statusEffect, entryVariable))
					{
						exitAmount += entryVariable;
					}
				}
			}
			return exitAmount > 0;
		}

		public StatusEffectType effectType = StatusEffectType.Scars;

		public Type effectClass = typeof(ApplyScarsEffect);
	}

	public struct CombatTrigger
    {
		public CombatTrigger(Action<object, object> method, TriggerCalls trigger)
        {
			_method = method;
			_trigger = trigger;
        }

		public Action<object, object> _method;
		public TriggerCalls _trigger;
	}
}
