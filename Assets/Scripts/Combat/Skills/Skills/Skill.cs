using System;
using UnityEngine;

namespace RPG_Project
{
    [Serializable]
    public class Skill
    {
        [SerializeField] protected SkillData data;

        [SerializeField] protected string skillName;
        [SerializeField] protected SkillID id;

        [SerializeField] protected TypeID skillType;

        [SerializeField] protected int mpCost;

        string description;

        public SkillData _data => data;

        public string _skillName => skillName;
        public SkillID _id => id;

        public TypeID _skillType => skillType;

        public int _mpCost => mpCost;

        public string _description => description;

        public Skill(SkillData data)
        {
            this.data = data;

            skillName = data._skillName;
            id = data._id;

            skillType = data._skillType;

            mpCost = data._mpCost;

            description = data._description;
        }

        public virtual Command GetCommand(BattleChar instigator)
        {
            return new Command(instigator);
        }
    }
}