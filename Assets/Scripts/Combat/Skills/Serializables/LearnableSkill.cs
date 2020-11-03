using System;
using UnityEngine;

namespace RPG_Project
{
    [Serializable]
    public class LearnableSkill
    {
        [SerializeField] int level = 1;
        [SerializeField] SkillID id;

        public int _level => level;
        public SkillID _id => id;
    }
}