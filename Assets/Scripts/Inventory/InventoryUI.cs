using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // Serialized fields
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform gridContent;
    [SerializeField] private InventorySlotUI slotPrefab;
    [SerializeField] private InspectionUI inspectionUI;
    
    [SerializeField] private GameObject combinationPanel;
    [SerializeField] private TMP_Text selectionText;
    [SerializeField] private Button combineButton;
    [SerializeField] private CombinationRecipe[] combinationRecipes;

    // Variables
    private float previousTimeScale;
    private CursorLockMode previousCursorLockMode;
    private bool previousCursorVisible;

    public bool IsOpen => panel != null && panel.activeSelf;

    // -----------------------
    // Methods
    private void Start()
    {
        panel.SetActive(false);
        combinationPanel.SetActive(false);
    }

    private void Update()
    {
        if (IsOpen && inspectionUI != null && inspectionUI.IsOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                inspectionUI.Close();
    
            return;
        }
    
        if (IsOpen)
        {
            if (Input.GetKeyDown(KeyCode.Tab) ||
                Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }
    
            return;
        }
    
        if (inspectionUI != null && inspectionUI.IsOpen)
            return;
    
        if (Input.GetKeyDown(KeyCode.Tab))
            Open();
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
            slot.SetEntry(entry, this);
        }
    }

    // -----------------------
    // Combination methods

    private readonly List<InventoryEntry> selectedEntries =
        new List<InventoryEntry>();

    private bool combining;

    public void StartCombination(InventoryEntry firstEntry)
    {
        selectedEntries.Clear();
        selectedEntries.Add(firstEntry);
        combining = true;

        combinationPanel.SetActive(true);
        combinationPanel.transform.SetAsLastSibling();

        RefreshCombinationUI();
    }

    public void HandleSlotClick(InventoryEntry entry)
    {
        if (!combining)
        {
            inspectionUI.ShowInventory(entry);
            return;
        }

        if (selectedEntries.Any(item => item.Id == entry.Id))
            return;

        if (selectedEntries.Count >= 3)
        {
            ShowCombinationMessage("Only two or three items can be combined.");
            return;
        }

        selectedEntries.Add(entry);
        RefreshCombinationUI();
    }

    public void CombineSelected()
    {
        CombinationRecipe recipe = combinationRecipes.FirstOrDefault(
            candidate => candidate.Matches(selectedEntries));

        if (recipe == null)
        {
            ShowCombinationMessage(
                "No combination recipe matches these items.");
            return;
        }

        if (InventoryManager.Instance.TryCombine(
            selectedEntries,
            recipe))
        {
            ShowCombinationMessage(
                $"{recipe.ResultName} was added to your inventory.");
            selectedEntries.Clear();
            combining = false;
            combinationPanel.SetActive(false);
            RefreshGrid();
        }
        else
        {
            ShowCombinationMessage(
                "The combination could not be completed.");
        }
    }

    public void CancelCombination()
    {
        selectedEntries.Clear();
        combining = false;
        combinationPanel.SetActive(false);
    }

    private void RefreshCombinationUI()
    {
        string names = string.Join(
            ", ",
            selectedEntries.Select(entry => entry.DisplayName));

        selectionText.text = names;
        combineButton.interactable = selectedEntries.Count >= 2;
    }

    private void ShowCombinationMessage(string message)
    {
        selectionText.text = message;
    }
}