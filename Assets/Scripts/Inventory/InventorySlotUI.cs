using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;

    public void SetEntry(InventoryEntry entry)
    {
        nameText.text = entry.DisplayName;
        iconImage.sprite = entry.Icon;
        iconImage.enabled = entry.Icon != null;
    }
}