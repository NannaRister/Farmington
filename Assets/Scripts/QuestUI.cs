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

        UpdateUI();
    }

    private void FixedUpdate()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (questText == null) return;

        questText.text = "";

        foreach (Quest quest in QuestManager.Instance.activeQuests)
        {
            if (quest.isCompleted)
            {
                continue; // Skip completed quests
            }

            questText.text += $"{quest.questName} - In Progress\n";

            foreach (var item in quest.requiredItems)
            {
                int playerAmount = Inventory.Instance.GetItemAmount(item.Key);
                questText.text += $"{item.Key}: {playerAmount}/{item.Value}\n";
            }

            questText.text += "\n";
        }
    }

    public void CheckQuestProgress()
    {
        foreach (var quest in QuestManager.Instance.activeQuests)
        {
            if (quest.isCompleted)
                continue;

            bool isComplete = true;

            foreach (var item in quest.requiredItems)
            {
                if (!Inventory.Instance.HasItem(item.Key, item.Value))
                {
                    isComplete = false;
                    break;
                }
            }

            if (isComplete)
            {
                quest.CompleteQuest();
                Debug.Log($"{quest.questName} completed!");
            }
        }

        UpdateUI();
    }
}