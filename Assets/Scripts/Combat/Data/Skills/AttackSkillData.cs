using UnityEngine;

namespace RPG_Project
{
    [CreateAssetMenu(fileName = "New Attack", menuName = "Combat/Skills/Attack")]
    public class AttackSkillData : SkillData
    {
        [SerializeField] bool attackAll;

        [Range(10, 250)]
        [SerializeField] int power;
        [Range(10, 100)]
        [SerializeField] int accuracy = 100;

        public bool _attackAll => attackAll;

        public int _power => power;
        public int _accuracy => accuracy;

        public override Skill GetSkill()
        {
            return new AttackSkill(this);
        }
    }
}