using UnityEngine;

namespace RPG_Project
{
    [CreateAssetMenu(fileName = "New Damage Item", menuName = "Inventory/Item/Damage")]
    public class DamageItemData : ItemData
    {
        [SerializeField] TypeID damageType;
        [SerializeField] int damage;

        public TypeID _damageType => damageType;
        public int _damage => damage;

        public override Item GetItem()
        {
            return new DamageItem(this);
        }
    }
}