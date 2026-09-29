using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // SerializeFields
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private Transform cameraTransform;
    
    // Variables
    private Vector3 moveDirection;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // ----------------
    // Update function
    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    // ----------------
    // Help functions
    private void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();
        
        moveDirection = (forward * vertical + right * horizontal).normalized;
    }
    
    private void Move()
    {
        Vector3 targetVelocity = moveDirection * moveSpeed;
        targetVelocity.y = rb.linearVelocity.y;
        
        rb.linearVelocity = targetVelocity;
    }
}