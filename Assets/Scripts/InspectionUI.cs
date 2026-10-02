using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectionUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image iconImage;

    private PickupItem inspectedItem;
    private float previousTimeScale;
    private CursorLockMode previousCursorLockMode;
    private bool previousCursorVisible;

    public bool IsOpen => panel != null && panel.activeSelf;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void Show(PickupItem item)
    {
        if (item == null)
            return;

        inspectedItem = item;
        titleText.text = item.displayName;
        descriptionText.text = item.description;

        iconImage.sprite = item.icon;
        iconImage.gameObject.SetActive(item.icon != null);

        previousTimeScale = Time.timeScale;
        previousCursorLockMode = Cursor.lockState;
        previousCursorVisible = Cursor.visible;

        panel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

    public void Close()
    {
        if (!IsOpen)
            return;

        panel.SetActive(false);
        inspectedItem = null;

        Time.timeScale = previousTimeScale;
        Cursor.lockState = previousCursorLockMode;
        Cursor.visible = previousCursorVisible;
    }
}