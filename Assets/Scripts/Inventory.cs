using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public int amount;
    public Sprite itemSprite;

    public InventoryItem(string name, int amt, Sprite sprite)
    {
        itemName = name;
        amount = amt;
        itemSprite = sprite;
    }
}

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public string selectedItem = "Wheat Seeds"; // Default selected item
    public Dictionary<string, Sprite> itemSprites; // Store sprites for items

    void Start()
    {
    }

    public void AddItem(string itemName, int amount)
    {
        InventoryItem existingItem = inventoryItems.Find(item => item.itemName == itemName);
        if (existingItem != null)
        {
            existingItem.amount += amount;
        }
        else
        {
            inventoryItems.Add(new InventoryItem(itemName, amount, itemSprites[itemName]));
        }
    }

    public bool RemoveItem(string itemName)
    {
        InventoryItem existingItem = inventoryItems.Find(item => item.itemName == itemName);
        if (existingItem != null && existingItem.amount > 0)
        {
            existingItem.amount--;
            if (existingItem.amount == 0)
            {
                inventoryItems.Remove(existingItem);
            }
            return true;
        }
        return false;
    }

    public void SelectItem(string itemName)
    {
        if (inventoryItems.Exists(item => item.itemName == itemName))
        {
            selectedItem = itemName;  // Store the item's name as a string
            Debug.Log($"Selected item: {selectedItem}");
        }
        else
        {
            Debug.LogWarning($"Item '{itemName}' not found in inventory.");
        }
    }
}
