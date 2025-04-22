using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;  // The panel holding the inventory UI
    public GameObject cropButtonPrefab;  // Prefab for crop selection buttons
    public Transform cropButtonParent;  // Parent object where crop buttons will be placed
    public Inventory inventory;  // Reference to the Inventory script

    void Start()
    {
        // Initially hide the inventory panel
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        // Toggle inventory with "I" key
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("I was pressed");
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel == null)
        {
            Debug.LogError("Inventory panel is not assigned!");
            return;
        }

        bool isActive = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isActive);

        if (!isActive) // If opening the inventory, update buttons
        {
            Debug.Log("Opening Inventory...");
            UpdateCropButtons();
        }
        else
        {
            Debug.Log("Closing Inventory...");
        }
    }

    public void UpdateCropButtons()
    {
        if (cropButtonParent == null || inventory == null || cropButtonPrefab == null)
        {
            Debug.LogError("Missing essential inventory UI references!");
            return;
        }

        // Clear old buttons
        foreach (Transform child in cropButtonParent)
        {
            Destroy(child.gameObject);
        }

        if (inventory.inventoryItems == null || inventory.inventoryItems.Count == 0)
        {
            Debug.LogWarning("Inventory is empty, no buttons to create.");
            return;
        }

        foreach (InventoryItem item in inventory.inventoryItems)
        {
            if (item.amount > 0)
            {
                GameObject button = Instantiate(cropButtonPrefab, cropButtonParent);
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = $"{item.itemName} x{item.amount}";
                    buttonText.enabled = false;
                }

                Image buttonImage = button.GetComponentInChildren<Image>();
                if (buttonImage != null)
                {
                    buttonImage.sprite = item.itemSprite;
                }

                // Mouse hover events for text
                EventTrigger trigger = button.AddComponent<EventTrigger>();
                EventTrigger.Entry entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
                entryEnter.callback.AddListener((eventData) => ShowText(buttonText));
                EventTrigger.Entry entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
                entryExit.callback.AddListener((eventData) => HideText(buttonText));
                trigger.triggers.Add(entryEnter);
                trigger.triggers.Add(entryExit);

                //crop selection event to the button
                Button btnComponent = button.GetComponent<Button>();
                if (btnComponent != null)
                {
                    btnComponent.onClick.AddListener(() => OnCropSelected(item.itemName));
                }
            }
        }
    }

    private void ShowText(TextMeshProUGUI text)
    {
        if (text != null)
            text.enabled = true;
    }

    private void HideText(TextMeshProUGUI text)
    {
        if (text != null)
            text.enabled = false;
    }

    private void OnCropSelected(string selectedCrop)
    {
        Debug.Log($"Selected crop: {selectedCrop}");
        inventory.selectedItem = selectedCrop;
    }
}
    //TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
    //if (buttonText != null)
    //{
    //    buttonText.text = $"{item.itemName} x{item.amount}";
    //}
    //else
    //{
    //    Debug.LogError(" Button is missing a Text component!");
    //}

    //Button btnComponent = button.GetComponent<Button>();
    //            if (btnComponent != null)
    //            {
    //                btnComponent.onClick.AddListener(() => inventory.SelectItem(item.itemName));
    //            }
    //            else
    //            {
    //                Debug.LogError(" Crop button is missing a Button component!");
    //            }
    //            // Assign the sprite to the Image component inside the button
    //            Image buttonImage = button.GetComponentInChildren<Image>();
    //            if (buttonImage != null)
    //            {
    //                buttonImage.sprite = item.itemSprite;  // Assuming crop has an itemSprite
    //            }
    //        }
    //        Debug.Log(" Crop buttons updated successfully.");
    //    }


    // Called when a crop button is selected


