using System;
using UnityEngine;

namespace RPG_Project
{
    [Serializable]
    public class BuffSkill : Skill
    {
        [SerializeField] bool affectsAllies;
        [SerializeField] bool affectsOpponents;

        [SerializeField] bool affectsWholeSide;

        [SerializeField] BuffEffect[] effects;

        public bool _affectsAllies => affectsAllies;
        public bool _affectsOpponents => affectsOpponents;

        public bool _affectsWholeSide => affectsWholeSide;

        public BuffEffect[] _effects => effects;

        public BuffSkill(BuffSkillData data) : base(data)
        {
            affectsAllies = data._affectsAllies;
            affectsOpponents = data._affectsOpponents;

            affectsWholeSide = data._affectsWholeSide;

            effects = data._effects;
        }

        public override Command GetCommand(BattleChar instigator)
        {
            return new BuffCommand(instigator, this);
        }
    }
}