using System;
using UnityEngine;

namespace RPG_Project
{
    [Serializable]
    public class AttackSkill : Skill
    {
        [SerializeField] bool attackAll;

        [SerializeField] int power;
        [SerializeField] int accuracy;

        public bool _attackAll => attackAll;

        public int _power => power;
        public int _accuracy => accuracy;

        public AttackSkill(AttackSkillData data) : base(data)
        {
            attackAll = data._attackAll;

            power = data._power;
            accuracy = data._accuracy;
        }

        public override Command GetCommand(BattleChar instigator)
        {
            return new AttackCommand(instigator, this);
        }
    }
}