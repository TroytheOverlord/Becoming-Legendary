using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item; 

   private void Start()
    {
        if (PlayerData.Instance.acquiredItems.Contains(item.itemName))
        {
            Destroy(gameObject);
        }
    }

private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        if (!PlayerData.Instance.acquiredItems.Contains(item.itemName))
        {
            PlayerData.Instance.acquiredItems.Add(item.itemName);
            Inventory playerInventory = FindObjectOfType<Inventory>();

            if (playerInventory != null)
            {
                playerInventory.AddItem(item);
                Debug.Log("Picked up: " + item.itemName);
            }
            else
            {
                Debug.LogError("Inventory not found in scene!");
            }
        }

        Destroy(gameObject);
    }
}

}
