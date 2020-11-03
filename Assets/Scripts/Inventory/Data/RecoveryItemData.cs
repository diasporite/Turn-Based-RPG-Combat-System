using UnityEngine;

namespace RPG_Project
{
    [CreateAssetMenu(fileName = "New Recovery Item", menuName = "Inventory/Item/Recovery")]
    public class RecoveryItemData : ItemData
    {
        [SerializeField] bool recoverHp;
        [SerializeField] bool recoverMp;

        [SerializeField] int recoverAmountHp;
        [SerializeField] int recoverAmountMp;

        public bool _recoverHp => recoverHp;
        public bool _recoverMp => recoverMp;

        public int _recoverAmountHp => recoverAmountHp;
        public int _recoverAmountMp => recoverAmountMp;

        public override Item GetItem()
        {
            return new RecoveryItem(this);
        }
    }
}