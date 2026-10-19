using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItemHUD : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text equippedText;

    private InventoryManager subscribedManager;

    private void Start()
    {
        if (panel == null || itemIcon == null || equippedText == null)
        {
            Debug.LogError(
                "Assign the panel, item icon, and equipped text on EquippedItemHUD.",
                this);
            enabled = false;
            return;
        }

        subscribedManager = InventoryManager.Instance;

        if (subscribedManager == null)
        {
            Debug.LogError("No InventoryManager is active in the scene.", this);
            enabled = false;
            return;
        }

        subscribedManager.EquippedItemChanged += Refresh;
        Refresh(subscribedManager.EquippedItem);
    }

    private void OnDestroy()
    {
        if (subscribedManager != null)
            subscribedManager.EquippedItemChanged -= Refresh;
    }

    private void Refresh(InventoryEntry equippedItem)
    {
        bool hasEquippedItem = equippedItem != null;
        panel.SetActive(hasEquippedItem);

        if (!hasEquippedItem)
            return;

        itemIcon.sprite = equippedItem.Icon;
        itemIcon.enabled = equippedItem.Icon != null;
        equippedText.text = $"Equipped: {equippedItem.DisplayName}";
    }
}