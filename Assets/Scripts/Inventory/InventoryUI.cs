using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform gridContent;
    [SerializeField] private InventorySlotUI slotPrefab;
    [SerializeField] private InspectionUI inspectionUI;

    private float previousTimeScale;
    private CursorLockMode previousCursorLockMode;
    private bool previousCursorVisible;

    public bool IsOpen => panel != null && panel.activeSelf;

    private void Start()
    {
        panel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) &&
            (inspectionUI == null || !inspectionUI.IsOpen))
        {
            if (IsOpen)
                Close();
            else
                Open();
        }
    }

    private void Open()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("No InventoryManager is active in the scene.");
            return;
        }

        RefreshGrid();

        previousTimeScale = Time.timeScale;
        previousCursorLockMode = Cursor.lockState;
        previousCursorVisible = Cursor.visible;

        panel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Close()
    {
        panel.SetActive(false);

        Time.timeScale = previousTimeScale;
        Cursor.lockState = previousCursorLockMode;
        Cursor.visible = previousCursorVisible;
    }

    private void RefreshGrid()
    {
        foreach (Transform child in gridContent)
            Destroy(child.gameObject);

        foreach (InventoryEntry entry in InventoryManager.Instance.Items)
        {
            InventorySlotUI slot = Instantiate(slotPrefab, gridContent);
            slot.SetEntry(entry);
        }
    }
}