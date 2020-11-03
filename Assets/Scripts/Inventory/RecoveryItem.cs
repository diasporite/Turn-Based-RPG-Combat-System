using System;
using UnityEngine;

namespace RPG_Project
{
    [Serializable]
    public class RecoveryItem : Item
    {
        [SerializeField] bool recoverHp;
        [SerializeField] bool recoverMp;

        [SerializeField] int recoverAmountHp;
        [SerializeField] int recoverAmountMp;

        public bool _recoverHp => recoverHp;
        public bool _recoverMp => recoverMp;

        public int _recoverAmountHp => recoverAmountHp;
        public int _recoverAmountMp => recoverAmountMp;

        public RecoveryItem(RecoveryItemData data) : base(data)
        {
            recoverHp = data._recoverHp;
            recoverMp = data._recoverMp;

            recoverAmountHp = data._recoverAmountHp;
            recoverAmountMp = data._recoverAmountMp;
        }

        public override Command UseItem(BattleChar instigator)
        {
            return new RecoverCommand(instigator, this);
        }
    }
}