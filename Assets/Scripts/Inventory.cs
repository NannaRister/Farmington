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
    public static Inventory Instance;
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public string selectedItem = "Wheat Seeds"; // Default selected item
    public Dictionary<string, Sprite> itemSprites; // Store sprites for items

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Avoid duplicates
        }
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
            Sprite sprite = itemSprites != null && itemSprites.ContainsKey(itemName) ? itemSprites[itemName] : null;
            inventoryItems.Add(new InventoryItem(itemName, amount, itemSprites[itemName]));
        }
    }
    public bool HasItem(string itemName, int amount)
    {
        InventoryItem item = inventoryItems.Find(i => i.itemName == itemName);
        return item != null && item.amount >= amount;
    }

    public bool RemoveItem(string itemName, int amount)
    {
        InventoryItem item = inventoryItems.Find(i => i.itemName == itemName);
        if (item != null && item.amount >= amount)
        {
            item.amount -= amount;
            if (item.amount <= 0)
            {
                inventoryItems.Remove(item);
            }
            return true;
        }
        return false;
    }
    public int GetItemAmount(string itemName)
    {
        InventoryItem item = inventoryItems.Find(i => i.itemName == itemName);
        return item != null ? item.amount : 0;
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
