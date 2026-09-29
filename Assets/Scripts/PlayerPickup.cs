using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Pickup")) return;
        var pickup = other.GetComponent<PickupItem>();
        if (pickup != null)
        {
            InventoryManager.Instance.Add(pickup);
            pickup.OnPickup();
        }
        else Debug.LogWarning("Object tagged 'Pickup' missing PickupItem component.", other.gameObject);
    }
}