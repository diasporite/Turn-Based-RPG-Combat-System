using UnityEngine;

namespace RPG_Project
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Combat/Character")]
    public class CharData : ScriptableObject
    {
        [SerializeField] string charName;

        [SerializeField] CharID id;

        [SerializeField] TypeID type1;
        [SerializeField] TypeID type2;

        [Header("Moveset")]
        [SerializeField] LearnableSkill[] learnableSkills;

        [Header("Base Stats")]
        [Range(10, 250)]
        [SerializeField] int baseExpAtLv;
        [Range(10, 250)]
        [SerializeField] int baseExpReward;

        [Range(10, 250)]
        [SerializeField] int baseMoneyReward;

        [Range(20, 250)]
        [SerializeField] int baseHp;
        [Range(20, 250)]
        [SerializeField] int baseMp;

        [Range(20, 250)]
        [SerializeField] int baseAtk;
        [Range(20, 250)]
        [SerializeField] int baseDef;

        [Range(20, 250)]
        [SerializeField] int baseSpd;
        [Range(20, 250)]
        [SerializeField] int baseLck;

        [SerializeField] Sprite sprite;

        #region Getters
        public string _charName => charName;

        public CharID _id => id;

        public TypeID _type1 => type1;
        public TypeID _type2 => type2;

        public LearnableSkill[] _skillset => learnableSkills;

        public int _expReward => baseExpReward;

        public int _hp => baseHp;
        public int _mp => baseMp;

        public int _atk => baseAtk;
        public int _def => baseDef;

        public int _spd => baseSpd;
        public int _lck => baseLck;

        public Sprite _sprite => sprite;

        public int _bst => baseHp + baseMp + baseAtk + baseDef + baseSpd + baseLck; // DEBUG ONLY

        public int GetStat(StatID stat, int lv)
        {
            switch (stat)
            {
                case StatID.ExperienceReward:
                    return Mathf.RoundToInt(0.06f * baseExpReward * lv + 3);
                case StatID.MoneyReward:
                    return Mathf.RoundToInt(0.045f * baseMoneyReward * lv + 3);
                case StatID.Health:
                    return Mathf.RoundToInt(0.17f * baseHp * lv + 40);
                case StatID.Magic:
                    return Mathf.RoundToInt(0.05f * baseMp * lv + 40);
                case StatID.Attack:
                    return Mathf.RoundToInt(0.03f * baseAtk * lv + 5);
                case StatID.Defence:
                    return Mathf.RoundToInt(0.03f * baseDef * lv + 5);
                case StatID.Speed:
                    return Mathf.RoundToInt(0.03f * baseSpd * lv + 5);
                case StatID.Luck:
                    return Mathf.RoundToInt(0.03f * baseLck * lv + 5);
                default:
                    return 1;
            }
        }

        public int[] GetExpToReachLv()
        {
            int[] expAtLv = new int[GameManager.instance._levelCap];

            for(int i = 0; i < expAtLv.Length; i++)
            {
                expAtLv[i] = GetExpToSpecificLv(i);
            }

            return expAtLv;
        }

        public int GetExpToSpecificLv(int lv)
        {
            return Mathf.RoundToInt(0.72f * baseExpAtLv * lv * lv + 1.88f * lv);
        }
        #endregion
    }
}