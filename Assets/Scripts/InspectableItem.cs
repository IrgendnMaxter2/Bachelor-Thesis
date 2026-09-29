using UnityEngine;
using UnityEngine.UI;

public class InspectableItem : MonoBehaviour
{
    [Header("Inspectable Data")]
    public string id;
    public string title;
    [TextArea(3, 10)] public string description;
    public Sprite icon;
    public bool hideOnInspect = false;

    public void OnInspect()
    {
        if (InspectUI.Instance != null)
            InspectUI.Instance.Show(title, description, icon);
        if (hideOnInspect)
            gameObject.SetActive(false);
    }
}