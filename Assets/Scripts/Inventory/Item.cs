using System;
using UnityEngine;

namespace RPG_Project
{
    [Serializable]
    public class Item
    {
        [SerializeField] string itemName;
        [SerializeField] ItemID id;

        string description;

        public string _itemName => itemName;
        public ItemID _id => id;

        public string _description => description;

        public Item(ItemData data)
        {
            itemName = data._itemName;
            id = data._id;

            description = data._description;
        }

        public virtual Command UseItem(BattleChar instigator)
        {
            return new Command(instigator);
        }
    }
}