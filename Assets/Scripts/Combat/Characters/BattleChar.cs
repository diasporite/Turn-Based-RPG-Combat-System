using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG_Project
{
    [Serializable]
    public class BattleChar
    {
        [SerializeField] string charName;

        [SerializeField] CharData data;

        [SerializeField] CharID id;

        [SerializeField] Sprite sprite;

        bool dead = false;

        int levelCap;

        int skillCap;

        int healthCap;
        int statCap;

        float buffCap;
        float debuffCap;

        SkillDatabase skillDatabase;

        [SerializeField] BattleStatus battleStatus;

        [Header("Skillset")]
        [SerializeField] LearnableSkill[] skillset;
        [SerializeField] List<Skill> currentSkillset = new List<Skill>();

        [Header("Typing")]
        [SerializeField] TypeID type1;
        [SerializeField] TypeID type2;

        [Header("Progression Stats")]
        [SerializeField] int lv;
        [SerializeField] int exp;
        [SerializeField] int expReward;
        [SerializeField] int[] expToReachLv;

        [SerializeField] int moneyReward;

        [SerializeField] PointStat health;
        [SerializeField] PointStat magic;

        [SerializeField] Stat attack;
        [SerializeField] Stat defence;
        [SerializeField] Stat speed;
        [SerializeField] Stat luck;

        [Header("DEBUG ONLY")]
        [SerializeField] int bst;   // DEBUG PURPOSES ONLY

        [SerializeField] bool learntNewSkill = false;

        readonly Dictionary<StatID, Stat> statsDict = new Dictionary<StatID, Stat>();

        #region Getters
        public string _charName => charName;

        public CharID _id => id;

        public Sprite _sprite => sprite;

        public bool _dead
        {
            get => dead;
            set => dead = value;
        }

        public BattleStatus _battleStatus
        {
            get => battleStatus;
            set => battleStatus = value;
        }

        public TypeID _type1 => type1;
        public TypeID _type2 => type2;

        public Skill[] _currentSkillset => currentSkillset.ToArray();
        public List<Skill> _currentSkillsetList => currentSkillset;

        public int _lv => lv;
        public int _expReward => expReward;

        public int _moneyReward => moneyReward;

        public PointStat _health => health;
        public PointStat _magic => magic;

        public Stat _attack => attack;
        public Stat _defence => defence;
        public Stat _speed => speed;
        public Stat _luck => luck;

        public bool _learntNewSkill
        {
            get => learntNewSkill;
            set => learntNewSkill = value;
        }

        public bool CanUseSkill(Skill skill)
        {
            return magic._pointValue >= skill._mpCost;
        }

        public bool IsValueStat(StatID stat)
        {
            return statsDict.ContainsKey(stat);
        }

        public Stat GetStat(StatID stat)
        {
            if (statsDict.ContainsKey(stat)) return statsDict[stat];

            return null;
        }
        #endregion

        #region Constructors
        public BattleChar(CharData data, int lv)
        {
            this.data = data;

            charName = data._charName;

            id = data._id;

            sprite = data._sprite;

            dead = false;

            levelCap = GameManager.instance._levelCap;
            skillCap = GameManager.instance._skillCap;

            healthCap = GameManager.instance._pointsCap;
            statCap = GameManager.instance._statCap;

            buffCap = GameManager.instance._buffCap;
            debuffCap = GameManager.instance._debuffCap;

            skillDatabase = GameManager.instance._skillDatabase;

            type1 = data._type1;
            type2 = data._type2;

            skillset = data._skillset;

            expToReachLv = data.GetExpToReachLv();

            if (lv > levelCap) lv = levelCap;
            if (lv <= 0) lv = 1;

            if (lv >= levelCap - 1) exp = expToReachLv[expToReachLv.Length - 1];
            else exp = expToReachLv[lv - 1];
            this.lv = lv;

            BuildSkillset(lv);

            InitStats(lv);

            bst = data._bst; // DEBUG ONLY
        }
        #endregion

        #region InitMethods
        void InitStats(int lv)
        {
            expReward = data.GetStat(StatID.ExperienceReward, lv);
            moneyReward = data.GetStat(StatID.MoneyReward, lv);

            int healthValue = data.GetStat(StatID.Health, lv);
            int magicValue = data.GetStat(StatID.Magic, lv);

            int attackValue = data.GetStat(StatID.Attack, lv);
            int defenceValue = data.GetStat(StatID.Defence, lv);
            int speedValue = data.GetStat(StatID.Speed, lv);
            int luckValue = data.GetStat(StatID.Luck, lv);

            health = new PointStat(healthValue, healthValue);
            magic = new PointStat(magicValue, magicValue);

            attack = new Stat(attackValue);
            defence = new Stat(defenceValue);
            speed = new Stat(speedValue);
            luck = new Stat(luckValue);

            statsDict.Add(StatID.Health, health);
            statsDict.Add(StatID.Magic, magic);

            statsDict.Add(StatID.Attack, attack);
            statsDict.Add(StatID.Defence, defence);
            statsDict.Add(StatID.Speed, speed);
            statsDict.Add(StatID.Luck, luck);
        }
        #endregion

        public bool TakeDamage(int damage)
        {
            health.ChangeCurrentPoints(-damage);

            dead = health._pointValue <= 0;

            if (dead) battleStatus = BattleStatus.Dead;

            return dead;
        }

        #region ProgressionMethods
        // bool for whether level up
        public bool GainExperience(int amount)
        {
            var currentLv = lv;

            exp += Mathf.Abs(amount);

            var newLv = CalculateLv();

            if (currentLv != newLv)
            {
                LevelUp(newLv);
                return true;
            }

            learntNewSkill = false;

            return false;
        }

        public int GetExpToNext()
        {
            if (lv == levelCap) return 0;

            return expToReachLv[lv] - exp;
        }

        public int GetExpToNext(int lv)
        {
            if (lv == levelCap) return 0;

            return expToReachLv[lv] - exp;
        }

        public int CalculateLv()
        {
            for(int i = 0; i < expToReachLv.Length - 1; i++)
            {
                if (exp >= expToReachLv[i] && exp < expToReachLv[i + 1])
                {
                    return i + 1;
                }
            }

            return levelCap;
        }

        public void LevelUp(int newLv)
        {
            lv = newLv;

            CalculateStats(lv);

            // Check for new skills
            learntNewSkill = BuildSkillset(lv);
        }

        public void CalculateStats(int lv)
        {
            expReward = data.GetStat(StatID.ExperienceReward, lv);
            moneyReward = data.GetStat(StatID.MoneyReward, lv);

            // Recover HP and MP when levelling up
            health._allStatValues = data.GetStat(StatID.Health, lv);
            magic._allStatValues = data.GetStat(StatID.Magic, lv);

            attack._statValue = data.GetStat(StatID.Attack, lv);
            defence._statValue = data.GetStat(StatID.Defence, lv);

            speed._statValue = data.GetStat(StatID.Speed, lv);
            luck._statValue = data.GetStat(StatID.Luck, lv);
        }
        #endregion

        #region SkillsetMethods
        public bool BuildSkillset()
        {
            int skillsetCount = currentSkillset.Count;

            foreach (var learnable in skillset)
            {
                Skill skill = skillDatabase.GetSkill(learnable._id);

                if (learnable._level <= lv && !currentSkillset.Contains(skill))
                {
                    AddSkill(skill);
                }
            }

            return skillsetCount < currentSkillset.Count;
        }

        public bool BuildSkillset(int level)
        {
            int skillsetCount = currentSkillset.Count;

            foreach(var learnable in skillset)
            {
                Skill skill = skillDatabase.GetSkill(learnable._id);

                if (learnable._level <= level && !currentSkillset.Contains(skill))
                {
                    AddSkill(skill);
                }
            }

            return skillsetCount < currentSkillset.Count;
        }

        public void AddSkill(Skill skill)
        {
            currentSkillset.Add(skill);
            if (currentSkillset.Count > skillCap) currentSkillset.RemoveAt(0);
        }
        #endregion
    }
}