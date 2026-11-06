using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Button slotButton;    
    public void SetEntry(
        InventoryEntry entry,
        InventoryUI inventoryUI,
        bool clickable = true)
    {
        nameText.text = entry.DisplayName;
        iconImage.sprite = entry.Icon;
        iconImage.enabled = entry.Icon != null;

        slotButton.enabled = clickable;
    
        if (clickable)
        {
            slotButton.onClick.AddListener(
                () => inventoryUI.HandleSlotClick(entry));
        }
    }
}