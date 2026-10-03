using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private GameObject prompt;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private InspectionUI inspectionUI;

    private readonly List<PickupItem> nearbyItems = new List<PickupItem>();

    private void Start()
    {
        if (prompt != null)
            prompt.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        PickupItem item = other.GetComponentInParent<PickupItem>();

        if (item != null && !nearbyItems.Contains(item))
            nearbyItems.Add(item);
    }

    private void OnTriggerExit(Collider other)
    {
        PickupItem item = other.GetComponentInParent<PickupItem>();

        if (item != null)
            nearbyItems.Remove(item);
    }

    private void Update()
    {
        nearbyItems.RemoveAll(item =>
            item == null || !item.gameObject.activeInHierarchy);

        if (inspectionUI != null && inspectionUI.IsOpen)
        {
            if (prompt != null)
                prompt.SetActive(false);

            return;
        }

        PickupItem closestItem = GetClosestNearbyItem();

        if (prompt != null)
            prompt.SetActive(closestItem != null);

        if (closestItem == null)
            return;

        if (promptText != null)
            promptText.text = $"Press 'E' to inspect {closestItem.displayName}";

        if (Input.GetKeyDown(KeyCode.E) && inspectionUI != null)
            inspectionUI.Show(closestItem);
    }

    private PickupItem GetClosestNearbyItem()
    {
        PickupItem closestItem = null;
        float closestDistance = float.MaxValue;

        foreach (PickupItem item in nearbyItems)
        {
            if (item == null)
                continue;

            float distance = (item.transform.position - transform.position).sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestItem = item;
            }
        }

        return closestItem;
    }
}