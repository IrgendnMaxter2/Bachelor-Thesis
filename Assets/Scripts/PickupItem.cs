using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string id;
    public string displayName;
    public Sprite icon;
    public bool destroyOnPickup = false;

    public void OnPickup()
    {
        gameObject.SetActive(false);
        if (destroyOnPickup) Destroy(gameObject);
    }
}