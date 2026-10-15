using UnityEngine;
using System.Collections.Generic;

public sealed class InventoryEntry
{
    public string Id { get; }
    public string DisplayName { get; }
    public string Description { get; }
    public Sprite Icon { get; }

    public InventoryEntry(PickupItem item)
    {
        Id = string.IsNullOrEmpty(item.id) ? item.displayName : item.id;
        DisplayName = item.displayName;
        Description = item.description;
        Icon = item.icon;
    }

    // Constructor for combining items
    public InventoryEntry(
        string id,
        string displayName,
        string description,
        Sprite icon)
    {
        Id = id;
        DisplayName = displayName;
        Description = description;
        Icon = icon;
    }
}

// --------------------
// Manager for the inventory
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private readonly List<InventoryEntry> items = new List<InventoryEntry>();

    public IReadOnlyList<InventoryEntry> Items => items.AsReadOnly();

    
    // Equipped item management
    public InventoryEntry EquippedItem { get; private set; }

    public bool IsEquipped(string id)
    {
        return EquippedItem != null && EquippedItem.Id == id;
    }

    public bool TryEquip(string id)
    {
        InventoryEntry entry = items.Find(item => item.Id == id);

        if (entry == null)
            return false;

        EquippedItem = entry;
        return true;
    }

    public void Unequip()
    {
        EquippedItem = null;
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Add(PickupItem item)
    {
        if (item == null)
            return;

        InventoryEntry entry = new InventoryEntry(item);
        items.Add(entry);

        Debug.Log($"Picked up: {entry.DisplayName} ({entry.Id})");
    }

    public bool Has(string id)
    {
        return items.Exists(item => item.Id == id);
    }

    // -----------------------
    // Helper methods

    // Attempts combining selected entries
    public bool TryCombine(
        IReadOnlyList<InventoryEntry> selectedEntries,
        CombinationRecipe recipe)
    {
        if (recipe == null || !recipe.Matches(selectedEntries))
            return false;

        if (Has(recipe.ResultId))
        {
            Debug.LogWarning(
                $"The result '{recipe.ResultId}' is already in the inventory.");
            return false;
        }

        foreach (InventoryEntry entry in selectedEntries)
        {
            Remove(entry.Id);
        }

        InventoryEntry result = new InventoryEntry(
            recipe.ResultId,
            recipe.ResultName,
            recipe.ResultDescription,
            recipe.ResultIcon);

        items.Add(result);

        Debug.Log(
            $"Combined {selectedEntries.Count} items into {result.DisplayName}.");

        return true;
    }

    public bool Remove(string id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == id)
            {
                items.RemoveAt(i);

                if (IsEquipped(id))
                    Unequip();

                return true;
            }
        }

        return false;
    }    
}