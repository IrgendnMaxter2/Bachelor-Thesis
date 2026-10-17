using TMPro;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private Camera interactionCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private float interactionRadius = 0.2f;
    [SerializeField] private LayerMask interactionLayers = ~0;
    [SerializeField] private GameObject prompt;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private InspectionUI inspectionUI;

    private float feedbackUntil;
    private ItemLockedInteractable feedbackTarget;
    private string interactionFeedback;

    private void Start()
    {
        if (prompt != null)
            prompt.SetActive(false);
    }

    private void Update()
    {
        if (inspectionUI != null && inspectionUI.IsOpen)
        {
            HidePrompt();
            return;
        }

        Ray ray = new Ray(
            interactionCamera.transform.position,
            interactionCamera.transform.forward);

        // Temporary debug ray
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.blue);

        if (!Physics.SphereCast(
                ray,
                interactionRadius,
                out RaycastHit hit,
                interactionDistance,
                interactionLayers,
                QueryTriggerInteraction.Collide))
        {
            HidePrompt();
            return;
        }

        ItemLockedInteractable lockedObject =
            hit.collider.GetComponentInParent<ItemLockedInteractable>();

        if (lockedObject != null)
        {
            if (lockedObject.IsUnlocked)
            {
                HidePrompt();
                return;
            }

            ShowPrompt(
                Time.unscaledTime < feedbackUntil &&
                feedbackTarget == lockedObject
                    ? interactionFeedback
                    : $"Press 'E' to interact with {lockedObject.DisplayName}");

            if (Input.GetKeyDown(KeyCode.E) && !lockedObject.TryInteract())
            {
                feedbackTarget = lockedObject;
                interactionFeedback = lockedObject.LockedMessage;
                feedbackUntil = Time.unscaledTime + 1.5f;
            }

            return;
        }

        PickupItem pickup = hit.collider.GetComponentInParent<PickupItem>();

        if (pickup == null)
        {
            HidePrompt();
            return;
        }

        ShowPrompt($"Press 'E' to inspect {pickup.displayName}");

        if (Input.GetKeyDown(KeyCode.E) && inspectionUI != null)
            inspectionUI.ShowItem(pickup);
    }

    private void ShowPrompt(string message)
    {
        if (prompt != null)
            prompt.SetActive(true);

        if (promptText != null)
            promptText.text = message;
    }

    private void HidePrompt()
    {
        if (prompt != null)
            prompt.SetActive(false);
    }
}