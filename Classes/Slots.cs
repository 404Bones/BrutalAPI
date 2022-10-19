using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public static class Slots
    {
        public static BaseCombatTargettingSO Self;
        public static BaseCombatTargettingSO Front;
        public static BaseCombatTargettingSO FrontLeftRight;
        public static BaseCombatTargettingSO LeftRight;
        public static BaseCombatTargettingSO Sides;
        public static BaseCombatTargettingSO Right;
        public static BaseCombatTargettingSO Left;

        public static void Setup()
        {
            Self = LoadedAssetsHandler.GetEnemy("Mung_EN").abilities[1].ability.animationTarget;

            Sides = LoadedAssetsHandler.GetCharcater("Hans_CH").rankedData[0].rankAbilities[2].ability.animationTarget;

            Targetting_BySlot_Index frontTarget = ScriptableObject.CreateInstance(typeof(Targetting_BySlot_Index)) as Targetting_BySlot_Index;
            frontTarget.slotPointerDirections = new int[1] { 0 };
            Front = frontTarget;

            Targetting_BySlot_Index leftRightTarget = ScriptableObject.CreateInstance(typeof(Targetting_BySlot_Index)) as Targetting_BySlot_Index;
            leftRightTarget.slotPointerDirections = new int[2] { -1, 1 };
            LeftRight = leftRightTarget;

            Targetting_BySlot_Index frontLeftRightTarget = ScriptableObject.CreateInstance(typeof(Targetting_BySlot_Index)) as Targetting_BySlot_Index;
            frontLeftRightTarget.slotPointerDirections = new int[3] { -1, 0, 1 };
            FrontLeftRight = frontLeftRightTarget;

            Targetting_BySlot_Index leftTarget = ScriptableObject.CreateInstance(typeof(Targetting_BySlot_Index)) as Targetting_BySlot_Index;
            leftTarget.slotPointerDirections = new int[1] { -1 };
            Left = leftTarget;

            Targetting_BySlot_Index rightTarget = ScriptableObject.CreateInstance(typeof(Targetting_BySlot_Index)) as Targetting_BySlot_Index;
            rightTarget.slotPointerDirections = new int[1] { 1 };
            Right = rightTarget;
        }

        public static BaseCombatTargettingSO SlotTarget(int[] slots)
        {
            Targetting_BySlot_Index t = ScriptableObject.CreateInstance(typeof(Targetting_BySlot_Index)) as Targetting_BySlot_Index;
            t.slotPointerDirections = slots;
            return t;
        }

        public static BaseCombatTargettingSO LargeSlotTarget(int[] slots)
        {
            CustomOpponentTargetting_BySlot_Index t = ScriptableObject.CreateInstance(typeof(CustomOpponentTargetting_BySlot_Index)) as CustomOpponentTargetting_BySlot_Index;
            t._slotPointerDirections = new int[slots.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                t._slotPointerDirections[i] = 0;
            }
            t._frontOffsets = slots;
            return t;
        }
    }
}
