using UnityEngine;

namespace RPG_Project
{
    public class ItemData : ScriptableObject
    {
        [SerializeField] string itemName;
        [SerializeField] ItemID id;

        [TextArea(4, 7)]
        [SerializeField] string description;

        public string _itemName => itemName;
        public ItemID _id => id;

        public string _description => description;

        public virtual Item GetItem()
        {
            return new Item(this);
        }
    }
}