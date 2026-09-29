using UnityEngine;

public class PlayerInspect : MonoBehaviour
{
    [Header("Prompt (optional)")]
    public GameObject inspectPrompt; // small UI element to enable/disable
    public string promptFormat = "Press E to inspect";

    InspectableItem currentInspectable;

    void Update()
    {
        if (currentInspectable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInspectable.OnInspect();
            HidePrompt();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Inspectable")) return;
        var item = other.GetComponent<InspectableItem>();
        if (item != null)
        {
            currentInspectable = item;
            ShowPrompt(item);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Inspectable")) return;
        var item = other.GetComponent<InspectableItem>();
        if (item != null && item == currentInspectable)
        {
            currentInspectable = null;
            HidePrompt();
        }
    }

    void ShowPrompt(InspectableItem item)
    {
        if (inspectPrompt != null)
        {
            inspectPrompt.SetActive(true);
            var txt = inspectPrompt.GetComponentInChildren<UnityEngine.UI.Text>();
            if (txt != null) txt.text = promptFormat + (string.IsNullOrEmpty(item.title) ? "" : $" ({item.title})");
        }
    }

    void HidePrompt()
    {
        if (inspectPrompt != null) inspectPrompt.SetActive(false);
    }
}