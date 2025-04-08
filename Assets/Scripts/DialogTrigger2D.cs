using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogTrigger2D : MonoBehaviour
{
    public GameObject dialogPanel;
    public Button closeButton;
    private bool isPlayerNear = false;
    private bool isDialogOpen = false;  // Track dialog state
    
    // Reference to QuestGiver
    public QuestGiver questGiver;
    public TMP_Text dialogText;  // UI Text component to show the dialog content

    void Start()
    {
        dialogPanel.SetActive(false);
        closeButton.onClick.AddListener(CloseDialog);
    }

    void Update()
    {
        //if (isPlayerNear)
        //{
        //    Debug.Log("Press E to interact");
        //}
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDialog();
        }
    }

    private void ToggleDialog()
    {
        isDialogOpen = !isDialogOpen;
        dialogPanel.SetActive(isDialogOpen);

        if (isDialogOpen && questGiver != null)
        {
            // Show quest description in dialog
            dialogText.text = questGiver.description;  // Show quest description

            foreach (Quest quest in QuestManager.Instance.activeQuests)
            {
                if (quest.isCompleted)
                {
                    dialogText.text = questGiver.description + "\n Completed!";
                }
            }
        }



    }

    private void CloseDialog()
    {
        isDialogOpen = false;
        dialogPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            CloseDialog();
        }
    }

    public bool IsDialogOpen() => isDialogOpen; // Check if dialog is open
}
