using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Quest
{
    public string questName;
    public string description;
    public bool isCompleted;
    public Dictionary<string, int> requiredItems;  // Items and the amount required for quest objectives
    public int rewardAmount;

    public Quest(string name, string desc, Dictionary<string, int> items, int reward)
    {
        questName = name;
        description = desc;
        requiredItems = items;
        rewardAmount = reward;
        isCompleted = false;
    }

    // Check if the player has all required items for the quest
    public bool CheckIfComplete()
    {
        foreach (var item in requiredItems)
        {
            if (!Inventory.Instance.HasItem(item.Key, item.Value))
            {
                return false; // If any required item is missing, the quest is incomplete
            }
        }
        return true;
    }

    // Complete the quest if all requirements are met
    public void CompleteQuest()
    {
        if (CheckIfComplete())
        {
            isCompleted = true;
            Debug.Log(questName + " completed!");

            // Remove the items from the player's inventory
            foreach (var item in requiredItems)
            {
                Inventory.Instance.RemoveItem(item.Key, item.Value);
            }
        }
        else
        {
            Debug.Log("You don't have all the required items to complete the quest.");
        }
    }

    // Complete a specific objective related to items
    public void CompleteObjective(string itemName)
    {
        if (requiredItems.ContainsKey(itemName))
        {
            requiredItems.Remove(itemName);
            Debug.Log("Objective " + itemName + " completed!");

            // Check if the quest is now complete
            if (CheckIfComplete())
            {
                CompleteQuest();
            }
        }
        else
        {
            Debug.LogWarning("Objective not found in quest!");
        }
    }
}
