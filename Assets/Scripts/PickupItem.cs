using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string id;
    public string displayName;
    [TextArea] public string description;
    public Sprite icon;
    public bool destroyOnPickup = false;

    public void OnPickup()
    {
        gameObject.SetActive(false);
        if (destroyOnPickup) Destroy(gameObject);
    }
}