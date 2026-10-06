using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Button slotButton;
    
    public void SetEntry(InventoryEntry entry, InspectionUI inspectionUI)
    {
        nameText.text = entry.DisplayName;
        iconImage.sprite = entry.Icon;
        iconImage.enabled = entry.Icon != null;
    
        slotButton.onClick.AddListener(() => inspectionUI.Show(entry));
    }
}