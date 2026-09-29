using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    List<string> items = new List<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void Add(PickupItem item)
    {
        if (item == null) return;
        string key = string.IsNullOrEmpty(item.id) ? item.displayName : item.id;
        items.Add(key);
        Debug.Log($"Picked up: {item.displayName} ({key})");
    }

    public bool Has(string key) => items.Contains(key);
    public IReadOnlyList<string> Items => items.AsReadOnly();
}