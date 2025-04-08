using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestUI : MonoBehaviour
{
    public GameObject questPanel;
    public TMP_Text questText;

    private void Start()
    {
        if (questText == null)
        {
            Debug.LogError("QuestText reference is missing in QuestUI!");
        }

        // Update the UI at the start
        UpdateUI();
    }

    private void FixedUpdate()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (questText == null) return;

        questText.text = ""; // Clear existing text

        // Loop through all active quests
        foreach (Quest quest in QuestManager.Instance.activeQuests)
        {
            questText.text += $"{quest.questName} - {(quest.isCompleted ? "Completed" : "In Progress")}\n";

            // If quest is not completed, show the required items
            if (!quest.isCompleted)
            {
                foreach (var item in quest.requiredItems)
                {
                    // Check how many of the item the player has in their inventory
                    int playerAmount = PlayerInventory.Instance.HasItem(item.Key, item.Value) ? PlayerInventory.Instance.GetItemAmount(item.Key) : 0;
                    questText.text += $"{item.Key}: {playerAmount}/{item.Value}\n";
                }
            }

            if (quest.isCompleted)
            {
                questText.text = "";
            }
        }
    }

    public void CheckQuestProgress()
    {
        // Loop through all active quests and check progress
        foreach (var quest in QuestManager.Instance.activeQuests)
        {
            bool isComplete = true;

            foreach (var item in quest.requiredItems)
            {
                if (!PlayerInventory.Instance.HasItem(item.Key, item.Value))
                {
                    isComplete = false;
                    break;
                }
            }

            if (isComplete && !quest.isCompleted)
            {
                quest.CompleteQuest(); // Mark the quest as complete
                Debug.Log($"{quest.questName} completed!");
            }
        }

        // After checking quest progress, update the UI
        UpdateUI();
    }
}
