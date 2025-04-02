using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
   public Dictionary<string, Item> items = new Dictionary<string, Item>();
    public Inventory playerInventory;

    public void AddItem(Item newItem)
    {
        if (items.ContainsKey(newItem.itemName))
        {
            items[newItem.itemName].itemQunatity++;
        }
        else
        {
            newItem.itemQunatity = 1;
            items.Add(newItem.itemName, newItem);
        }
    }

    public Item GetItem(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            return items[itemName];
        }
        return null;
    }

    public void RemoveItem(Item item)
    {
        if (items.ContainsKey(item.itemName))
        {
            items[item.itemName].itemQunatity--;

            if (items[item.itemName].itemQunatity <= 0)
            {
                items.Remove(item.itemName);
            }
        }
    }
}
