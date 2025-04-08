using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public string questName;
    [TextArea] public string description;

    [System.Serializable]
    public class QuestItemRequirement
    {
        public string itemName;
        public int amount;
    }

    public List<QuestItemRequirement> itemRequirements; // List of items and their required amounts
    public int rewardAmount;

    private Quest activeQuest;

    // Give the quest to the player
    public void GiveQuest()
    {
        // Only give the quest if the player doesn't already have it
        if (activeQuest == null)
        {
            Dictionary<string, int> requiredItems = new Dictionary<string, int>();

            // Build the required items dictionary from the list of item names and amounts
            foreach (var req in itemRequirements)
            {
                if (!requiredItems.ContainsKey(req.itemName))
                {
                    requiredItems.Add(req.itemName, req.amount);
                }
                else
                {
                    Debug.LogWarning($"Duplicate item requirement '{req.itemName}' in quest '{questName}'");
                }
            }

            activeQuest = new Quest(questName, description, requiredItems, rewardAmount);
            QuestManager.Instance.AddQuest(activeQuest);

            Debug.Log("Quest given: " + activeQuest.questName);
        }
        else
        {
            Debug.Log("You already have this quest!");
        }
    }

    // Turn in the quest if completed
    public void TurnInQuest()
    {
        if (activeQuest != null)
        {
            // Check if the player has enough items to complete the quest
            if (activeQuest.CheckIfComplete())
            {
                activeQuest.CompleteQuest();
                Debug.Log("Quest '" + questName + "' completed! Reward given.");
                // Optionally reward the player (e.g., add gold)
                // PlayerInventory.Instance.AddGold(rewardAmount); // Implement this if needed
                activeQuest = null; // Clear the active quest after completing
            }
            else
            {
                Debug.Log("You haven't completed the quest yet or don't have the required items.");
            }
        }
        else
        {
            Debug.Log("You don't have an active quest to turn in.");
        }
    }

    public bool HasActiveQuest()
    {
        return activeQuest != null && !activeQuest.isCompleted;
    }
}
