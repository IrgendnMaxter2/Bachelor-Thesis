using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // --- SerializeFields ---
    [SerializeField] private Transform player;

    [SerializeField] private float mouseSensitivity = 350f;
    [SerializeField] private float maxVerticalAngle = 40f;
    [SerializeField] private float cameraDistance = 7.5f;
    [SerializeField] private float cameraHeight = 2.5f;

    // Camera collision settings
    [SerializeField] private LayerMask collisionLayers = -1; // What layers the camera should collide with
    [SerializeField] private float cameraRadius = 0.3f; // Size of camera collision sphere
    [SerializeField] private float minDistance = 1f; // Minimum distance from player
    [SerializeField] private float collisionOffset = 0.2f; // Small offset to prevent clipping

    // --- Variables ---
    private float xRotation = 0f;
    private float yRotation = 0f;

    // ----------------
    // Start and Update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    } 
    void Update()
    {
        HandleMouseLook();
        UpdateCameraPosition();
    }

    // ----------------
    // Help functions
    private void HandleMouseLook()
    {
        // Get mouse movement input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        yRotation += mouseX;
        xRotation -= mouseY;
        
        // Prevent over-rotation
        xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);
        
        // Apply rotation to camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    private void UpdateCameraPosition()
    {
        if (player == null) return;
        
        Vector3 direction = new Vector3(0, 0, -cameraDistance);
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        Vector3 playerCenter = player.position + Vector3.up * cameraHeight;
        Vector3 desiredPos = playerCenter + rotation * direction;

        Vector3 finalPos = CheckCameraCollision(playerCenter, desiredPos);
        
        transform.position = finalPos;
        transform.LookAt(player.position + Vector3.up * cameraHeight);
    }

    private Vector3 CheckCameraCollision(Vector3 playerCenter, Vector3 desiredPos)
    {
        Vector3 directionToCamera = (desiredPos - playerCenter).normalized;
        float desiredDistance = Vector3.Distance(playerCenter, desiredPos);

        RaycastHit hit;
        if (Physics.SphereCast(playerCenter, cameraRadius, directionToCamera, out hit, desiredDistance, collisionLayers))
        {
            // Calculate new distance, ensuring minimum distance
            float newDistance = Mathf.Max(hit.distance - collisionOffset, minDistance);
            return playerCenter + directionToCamera * newDistance;
        }
        
        // No collision, use desired position
        return desiredPos;
    }
}