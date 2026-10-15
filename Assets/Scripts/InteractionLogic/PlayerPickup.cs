using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private GameObject prompt;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private InspectionUI inspectionUI;

    private readonly List<PickupItem> nearbyItems = new List<PickupItem>();

    private readonly List<ItemLockedInteractable> nearbyLockedObjects =
    new List<ItemLockedInteractable>();

    private float feedbackUntil;
    private string interactionFeedback;

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

        ItemLockedInteractable lockedObject =
            other.GetComponentInParent<ItemLockedInteractable>();
        
        if (lockedObject != null && !nearbyLockedObjects.Contains(lockedObject))
            nearbyLockedObjects.Add(lockedObject);
    }

    private void OnTriggerExit(Collider other)
    {
        PickupItem item = other.GetComponentInParent<PickupItem>();

        if (item != null)
            nearbyItems.Remove(item);

        ItemLockedInteractable lockedObject =
            other.GetComponentInParent<ItemLockedInteractable>();

        if (lockedObject != null)
            nearbyLockedObjects.Remove(lockedObject);
    }

    private void Update()
    {
        nearbyItems.RemoveAll(item =>
            item == null || !item.gameObject.activeInHierarchy);

        nearbyLockedObjects.RemoveAll(item =>
            item == null || !item.gameObject.activeInHierarchy || item.IsUnlocked);

        if (inspectionUI != null && inspectionUI.IsOpen)
        {
            if (prompt != null)
                prompt.SetActive(false);

            return;
        }

        PickupItem closestItem = GetClosestNearbyItem();

        ItemLockedInteractable closestLockedObject =
            GetClosestNearbyLockedObject();
        
        bool lockedObjectIsClosest =
            closestLockedObject != null &&
            (closestItem == null ||
             (closestLockedObject.transform.position - transform.position).sqrMagnitude <
             (closestItem.transform.position - transform.position).sqrMagnitude);
        
        if (lockedObjectIsClosest)
        {
            if (prompt != null)
                prompt.SetActive(true);
        
            if (promptText != null)
            {
                promptText.text = Time.unscaledTime < feedbackUntil
                    ? interactionFeedback
                    : $"Press 'E' to interact with {closestLockedObject.DisplayName}";
            }
        
            if (Input.GetKeyDown(KeyCode.E) &&
                !closestLockedObject.TryInteract())
            {
                interactionFeedback = closestLockedObject.LockedMessage;
                feedbackUntil = Time.unscaledTime + 1.5f;
            }
        
            return;
        }

        if (prompt != null)
            prompt.SetActive(closestItem != null);

        if (closestItem == null)
            return;

        if (promptText != null)
            promptText.text = $"Press 'E' to inspect {closestItem.displayName}";

        if (Input.GetKeyDown(KeyCode.E) && inspectionUI != null)
            inspectionUI.ShowItem(closestItem);
    }


    // Helper methods
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

    private ItemLockedInteractable GetClosestNearbyLockedObject()
    {
        ItemLockedInteractable closestObject = null;
        float closestDistance = float.MaxValue;

        foreach (ItemLockedInteractable item in nearbyLockedObjects)
        {
            if (item == null)
                continue;

            float distance =
                (item.transform.position - transform.position).sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = item;
            }
        }

        return closestObject;
    }
}