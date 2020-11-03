using UnityEngine;

namespace RPG_Project
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField] float generalCoefficient = 0.6f;
        [SerializeField] float levelCoefficient = 0.1f;
        [SerializeField] float powerCoefficient = 0.5f;

        [SerializeField] float critMultiplier = 1.414f;

        [SerializeField] float weaknessMultiplier = 1.414f;
        [SerializeField] float resistanceMultiplier = 0.707f;

        [SerializeField] float stabMultiplier = 1.414f;

        [Range(1 ,1000)]
        [SerializeField] int baseCritRate = 32;

        GameManager game;
        BattleManager battle;

        TypeDatabase types;

        public float _generalCoefficient => generalCoefficient;
        public float _levelCoefficient => levelCoefficient;
        public float _powerCoefficient => powerCoefficient;

        public float _critMultiplier => critMultiplier;

        public float _weaknessMultiplier => weaknessMultiplier;
        public float _resistanceMultiplier => resistanceMultiplier;

        public float _stabMultiplier => stabMultiplier;

        public int _baseCritRate => baseCritRate;

        private void Awake()
        {
            game = GetComponent<GameManager>();
            battle = GetComponent<BattleManager>();
        }

        private void Start()
        {
            types = game._typeDatabase;
        }

        #region Modifiers
        float LevelModifier(BattleChar instigator)
        {
            return levelCoefficient * instigator._lv;
        }

        float STAB(BattleChar instigator, Skill skill)
        {
            if (skill._skillType == instigator._type1 ||
                skill._skillType == instigator._type2) return stabMultiplier;

            return 1;
        }

        float StatModifier(BattleChar instigator, BattleChar target)
        {
            return (float)instigator._attack._currentStatValue / target._defence._currentStatValue;
        }

        float PowerModifier(AttackSkill skill)
        {
            return powerCoefficient * skill._power;
        }
        #endregion

        #region DamageCalculations
        public int RawDamage(BattleChar instigator, BattleChar target, AttackSkill skill)
        {
            float k = generalCoefficient * Random.Range(0.9f, 1.1f);

            float damage = k * LevelModifier(instigator) * STAB(instigator, skill) *
                StatModifier(instigator, target) * PowerModifier(skill);

            return Mathf.RoundToInt(damage);
        }

        public float Effectiveness(Skill skill, BattleChar target)
        {
            return types.TypeModifier(skill._skillType, target._type1, weaknessMultiplier, resistanceMultiplier) * 
                types.TypeModifier(skill._skillType, target._type2, weaknessMultiplier, resistanceMultiplier);
        }

        public float Effectiveness(TypeID attacking, BattleChar target)
        {
            return types.TypeModifier(attacking, target._type1, weaknessMultiplier, resistanceMultiplier) * 
                types.TypeModifier(attacking, target._type2, weaknessMultiplier, resistanceMultiplier);
        }

        public float Critical(BattleChar instigator)
        {
            int i = Random.Range(0, 1000);

            int threshold = baseCritRate + instigator._luck._currentStatValue;

            if (i < threshold) return critMultiplier;

            return 1;
        }
        #endregion

        public bool SuccessfulHit(BattleChar instigator, BattleChar[] targets, AttackSkill skill)
        {
            float speedRatio;
            int threshold;

            int i = Random.Range(0, 100);

            speedRatio = (float)instigator._speed._currentStatValue / AverageSpeed(targets);

            if (speedRatio < 1 && speedRatio >= 0.5f)
                threshold = Mathf.RoundToInt(speedRatio * skill._accuracy);
            else if (speedRatio < 0.5f)
                threshold = Mathf.RoundToInt(speedRatio * skill._accuracy);
            else threshold = Mathf.RoundToInt(skill._accuracy);

            return i < threshold;
        }

        int AverageSpeed(BattleChar[] targets)
        {
            int speed = 0;

            foreach (var target in targets)
            {
                speed += target._speed._currentStatValue;
            }

            return speed / targets.Length;
        }
    }
}