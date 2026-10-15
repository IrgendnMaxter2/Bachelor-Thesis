using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectionUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text equipButtonText;
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject pickupButton;
    [SerializeField] private GameObject combineButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private InventoryUI inventoryUI;
    
    private InventoryEntry inspectedEntry;

    private PickupItem inspectedItem;
    private float previousTimeScale;
    private CursorLockMode previousCursorLockMode;
    private bool previousCursorVisible;

    public bool IsOpen => panel != null && panel.activeSelf;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void ShowItem(PickupItem item)
    {
        if (item == null)
            return;
    
        inspectedItem = item;
        inspectedEntry = null;
    
        if (pickupButton != null)
            pickupButton.SetActive(true);
    
        if (combineButton != null)
            combineButton.SetActive(false);

        if (equipButton != null)
            equipButton.gameObject.SetActive(false);
    
        ShowDetails(item.displayName, item.description, item.icon);
    }
    
    public void ShowInventory(InventoryEntry entry)
    {
        if (entry == null)
            return;
    
        inspectedItem = null;
        inspectedEntry = entry;
    
        if (pickupButton != null)
            pickupButton.SetActive(false);
    
        if (combineButton != null)
            combineButton.SetActive(true);

        if (equipButton != null)
            equipButton.gameObject.SetActive(true);

        RefreshEquipButton();
    
        ShowDetails(entry.DisplayName, entry.Description, entry.Icon);
    }
    
    private void ShowDetails(string itemName, string itemDescription, Sprite itemIcon)
    {
        titleText.text = itemName;
        descriptionText.text = itemDescription;
    
        iconImage.sprite = itemIcon;
        iconImage.gameObject.SetActive(itemIcon != null);
    
        previousTimeScale = Time.timeScale;
        previousCursorLockMode = Cursor.lockState;
        previousCursorVisible = Cursor.visible;
    
        panel.SetActive(true);
        panel.transform.SetAsLastSibling();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void CombineItems()
    {
        if (inspectedEntry == null)
            return;

        inventoryUI.StartCombination(inspectedEntry);
        Close();
    }

    public void ConfirmPickup()
    {
        if (inspectedItem == null)
        {
            Close();
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("No InventoryManager is active in the scene.");
            return;
        }

        InventoryManager.Instance.Add(inspectedItem);
        inspectedItem.OnPickup();
        Close();
    }

    public void ToggleEquippedItem()
    {
        if (inspectedEntry == null)
            return;
    
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("No InventoryManager is active in the scene.");
            return;
        }
    
        if (InventoryManager.Instance.IsEquipped(inspectedEntry.Id))
        {
            InventoryManager.Instance.Unequip();
        }
        else if (!InventoryManager.Instance.TryEquip(inspectedEntry.Id))
        {
            Debug.LogWarning($"Could not equip inventory item '{inspectedEntry.Id}'.");
            return;
        }
    
        RefreshEquipButton();
    }
    
    private void RefreshEquipButton()
    {
        if (equipButtonText == null || inspectedEntry == null ||
            InventoryManager.Instance == null)
        {
            return;
        }
    
        equipButtonText.text =
            InventoryManager.Instance.IsEquipped(inspectedEntry.Id)
                ? "Unequip"
                : "Equip";
    }

    public void Close()
    {
        if (!IsOpen)
            return;
    
        panel.SetActive(false);
        inspectedItem = null;
        inspectedEntry = null;
    
        Time.timeScale = previousTimeScale;
        Cursor.lockState = previousCursorLockMode;
        Cursor.visible = previousCursorVisible;
    }
}