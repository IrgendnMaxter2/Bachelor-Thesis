using UnityEngine;
using UnityEngine.Events;

public class ItemLockedInteractable : MonoBehaviour
{
    [SerializeField] private string displayName = "object";
    [SerializeField] private string requiredItemId;
    [SerializeField] private string lockedMessage = "It won't open without the right item.";
    [SerializeField] private bool consumeItemOnUnlock;
    [SerializeField] private UnityEvent onUnlocked = new UnityEvent();

    public string DisplayName => displayName;
    public string LockedMessage => lockedMessage;
    public bool IsUnlocked { get; private set; }

    public bool TryInteract()
    {
        if (IsUnlocked)
            return true;

        InventoryManager inventory = InventoryManager.Instance;

        if (inventory == null)
        {
            Debug.LogError("No InventoryManager is active in the scene.");
            return false;
        }

        if (!inventory.IsEquipped(requiredItemId))
            return false;

        IsUnlocked = true;
        onUnlocked.Invoke();

        if (consumeItemOnUnlock)
            inventory.Remove(requiredItemId);

        return true;
    }
}