using System;
using UnityEngine;

namespace RPG_Project
{
    [Serializable]
    public class BuffEffect
    {
        [SerializeField] StatID affectedStat;
        [SerializeField] StatModifier modifier;

        public StatID _affectedStat => affectedStat;
        public StatModifier _modifier => modifier;
    }
}