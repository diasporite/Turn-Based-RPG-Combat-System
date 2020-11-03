using UnityEngine;

namespace RPG_Project
{
    [CreateAssetMenu(fileName = "New Heal Skill", menuName = "Combat/Skills/Heal")]
    public class HealSkillData : SkillData
    {
        [SerializeField] bool healAll;

        [SerializeField] bool healByAmount = true;
        [SerializeField] int healPower = 100;
        [Range(0f, 1f)]
        [SerializeField] float healProportion = 0.25f;

        public bool _healAll => healAll;

        public bool _healByAmount => healByAmount;
        public int _healPower => healPower;
        public float _healProportion => healProportion;

        public override Skill GetSkill()
        {
            return new HealSkill(this);
        }
    }
}