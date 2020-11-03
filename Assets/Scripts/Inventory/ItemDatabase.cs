using System.Collections.Generic;
using UnityEngine;

namespace RPG_Project
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Database")]
    public class ItemDatabase : ScriptableObject
    {
        [SerializeField] ItemData[] data;
        Dictionary<ItemID, ItemData> database = null;

        public Item GetItem(ItemID id)
        {
            return database[id].GetItem();
        }

        public void BuildDatabase()
        {
            if (database == null)
            {
                var dict = new Dictionary<ItemID, ItemData>();

                foreach(var item in data)
                {
                    dict.Add(item._id, item);
                }

                database = dict;
            }
        }
    }
}