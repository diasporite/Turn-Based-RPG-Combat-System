using System;
using UnityEngine;

namespace RPG_Project
{
    [Serializable]
    public class PointStat : Stat
    {
        [SerializeField] int pointValue;

        public int _pointValue
        {
            get => pointValue;

            set
            {
                pointValue = value;

                if (pointValue < 0) pointValue = 0;
                if (pointValue > _currentStatValue) pointValue = _currentStatValue;
            }
        }

        public int _allStatValues
        {
            set
            {
                statValue = value;
                pointValue = value;
            }
        }

        public float _pointsFraction => (float)pointValue / statValue;

        #region Constructor
        public PointStat(int statValue, int pointValue) : base(statValue)
        {
            InitPointValues(pointValue);
        }

        protected override void InitValues(int value)
        {
            valueCap = GameManager.instance._pointsCap;

            _statValue = value;
            currentStatValue = value;
        }

        void InitPointValues(int pointValue)
        {
            this.pointValue = pointValue;

            if (pointValue < 0) pointValue = 0;
            if (pointValue > statValue) pointValue = statValue;
        }
        #endregion

        public void ChangeCurrentPoints(int points)
        {
            pointValue += points;
            if (pointValue < 0) pointValue = 0;
            if (pointValue > statValue) pointValue = statValue;
        }

        public void ChangeCurrentPointsFraction(int fraction)
        {
            int change = Mathf.RoundToInt(fraction * statValue);

            ChangeCurrentPoints(change);
        }
    }
}