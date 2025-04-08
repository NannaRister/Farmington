using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<Quest> activeQuests = new List<Quest>();
    [SerializeField] private QuestUI questUI; // Optional UI display

    private void Awake()
    {
        // Singleton pattern to ensure only one QuestManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Optional: persist between scenes

        if (questUI == null)
        {
            Debug.LogWarning("QuestUI reference not assigned to QuestManager.");
        }
    }

    /// <summary>
    /// Add a quest to the active quest list
    /// </summary>
    public void AddQuest(Quest quest)
    {
        if (!activeQuests.Exists(q => q.questName == quest.questName))
        {
            activeQuests.Add(quest);
            Debug.Log("Quest added: " + quest.questName);
            questUI?.UpdateUI(); // Refresh UI
        }
        else
        {
            Debug.LogWarning("Quest already active: " + quest.questName);
        }
    }

    /// <summary>
    /// Tries to complete a quest if all objectives are fulfilled
    /// </summary>
    public void TryCompleteQuest(string questName)
    {
        Quest quest = activeQuests.Find(q => q.questName == questName);

        if (quest == null)
        {
            Debug.LogWarning("Quest not found: " + questName);
            return;
        }

        if (quest.isCompleted)
        {
            Debug.Log("Quest already completed: " + quest.questName);
            return;
        }

        if (quest.CheckIfComplete())
        {
            quest.CompleteQuest();
            Debug.Log("Quest completed: " + quest.questName);
            questUI?.UpdateUI(); // Refresh UI
        }
        else
        {
            Debug.Log("You haven't met all requirements to complete the quest: " + quest.questName);
        }
    }

    /// <summary>
    /// Manually mark quest as complete and remove from active list (optional)
    /// </summary>
    public void CompleteQuest(string questName)
    {
        Quest quest = activeQuests.Find(q => q.questName == questName);

        if (quest != null && !quest.isCompleted)
        {
            quest.CompleteQuest();
            Debug.Log("Quest forcefully completed: " + quest.questName);
            questUI?.UpdateUI(); // Refresh UI
        }
    }

    /// <summary>
    /// Optionally remove a quest from the list (e.g., after hand-in)
    /// </summary>
    public void RemoveQuest(string questName)
    {
        Quest quest = activeQuests.Find(q => q.questName == questName);
        if (quest != null)
        {
            activeQuests.Remove(quest);
            Debug.Log("Quest removed: " + quest.questName);
            questUI?.UpdateUI();
        }
    }
}
