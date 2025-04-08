using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public bool HasItem(string itemName, int amount)
    {
        return inventory.ContainsKey(itemName) && inventory[itemName] >= amount;
    }

    public void RemoveItem(string itemName, int amount)
    {
        if (HasItem(itemName, amount))
        {
            inventory[itemName] -= amount;
            if (inventory[itemName] <= 0)
            {
                inventory.Remove(itemName);
            }
        }
    }

    public void AddItem(string itemName, int amount)
    {
        if (!inventory.ContainsKey(itemName))
        {
            inventory[itemName] = 0;
        }
        inventory[itemName] += amount;
    }

    public int GetItemAmount(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            return inventory[itemName];
        }
        return 0;
    }
}
