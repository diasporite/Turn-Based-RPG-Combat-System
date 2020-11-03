using System;
using UnityEngine;

namespace RPG_Project
{
    [Serializable]
    public class DamageItem : Item
    {
        [SerializeField] TypeID damageType;
        [SerializeField] int damage;

        public TypeID _damageType => damageType;
        public int _damage => damage;

        public DamageItem(DamageItemData data) : base(data)
        {
            damageType = data._damageType;
            damage = data._damage;
        }

        public override Command UseItem(BattleChar instigator)
        {
            return new AttackCommand(instigator, this);
        }
    }
}