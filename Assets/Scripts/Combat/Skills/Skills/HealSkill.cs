using System;
using UnityEngine;

namespace RPG_Project
{
    [Serializable]
    public class HealSkill : Skill
    {
        [SerializeField] bool healAll;

        [SerializeField] bool healByAmount = true;
        [SerializeField] int healPower = 100;
        [SerializeField] float healProportion = 1.25f;

        public bool _healAll => healAll;

        public bool _healByAmount => healByAmount;
        public int _healPower => healPower;
        public float _healProportion => healProportion;

        public HealSkill(HealSkillData data) : base(data)
        {
            healAll = data._healAll;

            healByAmount = data._healByAmount;
            healPower = data._healPower;
            healProportion = data._healProportion;
        }

        public override Command GetCommand(BattleChar instigator)
        {
            return new RecoverCommand(instigator, this);
        }
    }
}