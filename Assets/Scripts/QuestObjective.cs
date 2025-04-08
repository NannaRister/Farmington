using UnityEngine;

public class QuestObjective : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName;
    public int requiredAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Add item to the player's inventory
            PlayerInventory.Instance.AddItem(itemName, requiredAmount);
            Debug.Log($"Picked up {requiredAmount} x {itemName}");

            // Optionally destroy the object in the world
            Destroy(gameObject);
        }
    }
}
