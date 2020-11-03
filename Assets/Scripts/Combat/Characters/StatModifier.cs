using System;
using UnityEngine;

namespace RPG_Project
{
    [Serializable]
    public class StatModifier
    {
        [SerializeField] StatModifierType modifierType;
        [SerializeField] int valueChange;

        public StatModifierType _modifierType => modifierType;
        public int _valueChange => valueChange;

        public StatModifier(StatModifierType modifierType, int valueChange)
        {
            this.modifierType = modifierType;
            this.valueChange = valueChange;
        }
    }
}