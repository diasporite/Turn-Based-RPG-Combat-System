using System.Collections.Generic;
using UnityEngine;

namespace RPG_Project
{
    public class Inventory : MonoBehaviour
    {
        int itemCap;

        [SerializeField] int money = 0;

        [SerializeField] ItemID[] itemIDs = new ItemID[10];

        [SerializeField] List<Item> inventory = new List<Item>();

        ItemDatabase database;

        public int _money
        {
            get => money;
            set => money = value;
        }

        public Item[] _inventory => inventory.ToArray();
        public List<Item> _inventoryList => inventory;

        public Item GetItem(int index)
        {
            return inventory[index];
        }

        public void BuildInventory()
        {
            itemCap = GameManager.instance._inventoryCap;
            database = GameManager.instance._itemDatabase;

            for (int i = 0; i < itemIDs.Length; i++)
            {
                if (i < itemCap && itemIDs[i] != ItemID.None)
                {
                    inventory.Add(database.GetItem(itemIDs[i]));
                }
            }
        }

        public void RemoveItem(Item item)
        {
            inventory.Remove(item);
        }
    }
}