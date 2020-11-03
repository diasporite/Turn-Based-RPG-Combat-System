using UnityEngine;

namespace RPG_Project
{
    public class SkillData : ScriptableObject
    {
        [SerializeField] string skillName;
        [SerializeField] SkillID id;

        [SerializeField] TypeID skillType;

        [SerializeField] int mpCost;

        [TextArea(4, 7)]
        [SerializeField] string description;

        public string _skillName => skillName;
        public SkillID _id => id;

        public TypeID _skillType => skillType;

        public int _mpCost => mpCost;

        public string _description => description;

        public virtual Skill GetSkill()
        {
            return new Skill(this);
        }
    }
}