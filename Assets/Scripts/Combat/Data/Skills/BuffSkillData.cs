using UnityEngine;

namespace RPG_Project
{
    [CreateAssetMenu(fileName = "New Buff", menuName = "Combat/Skills/Buffs")]
    public class BuffSkillData : SkillData
    {
        [SerializeField] bool affectsPlayer;
        [SerializeField] bool affectsEnemy;

        [SerializeField] bool affectsWholeSide;

        [SerializeField] BuffEffect[] effects;

        public bool _affectsAllies => affectsPlayer;
        public bool _affectsOpponents => affectsEnemy;

        public bool _affectsWholeSide => affectsWholeSide;

        public BuffEffect[] _effects => effects;

        public override Skill GetSkill()
        {
            return new BuffSkill(this);
        }
    }
}