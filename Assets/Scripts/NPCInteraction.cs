using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    private QuestGiver questGiver;

    private void Start()
    {
        questGiver = GetComponent<QuestGiver>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // If the NPC has an active quest, prompt for quest pickup
            if (questGiver != null)
            {
                if (questGiver.HasActiveQuest())
                {
                    Debug.Log("Press 'E' to turn in quest.");
                }
                else
                {
                    Debug.Log("Press 'E' to pick up quest.");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Press 'E' to interact with NPC.");
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F) && questGiver != null)
    //    {
    //        if (questGiver.HasActiveQuest())  // Check if the player already has the quest
    //        {
    //            questGiver.TurnInQuest();  // Turn in the quest if it's completed
    //        }
    //        else
    //        {
    //            questGiver.GiveQuest();  // Give the quest if the player doesn't have it
    //        }
    //    }
    //}
}
