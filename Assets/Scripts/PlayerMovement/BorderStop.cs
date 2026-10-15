using UnityEngine;

public class BorderStop : MonoBehaviour
{
    private void Start()
    {
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning("BorderStop needs Collider component");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Another player hit the border");
        }
    }
}