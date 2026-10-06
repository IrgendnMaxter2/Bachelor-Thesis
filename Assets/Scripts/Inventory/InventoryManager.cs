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
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private readonly List<InventoryEntry> items = new List<InventoryEntry>();

    public IReadOnlyList<InventoryEntry> Items => items.AsReadOnly();

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
}