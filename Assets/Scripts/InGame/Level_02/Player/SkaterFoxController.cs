using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class SkaterFoxController : MonoBehaviour
{
    private InputAction moveAction;

    // === Limits ===
    [Header("Limits")]
    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;

    // === Movement ===
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float slidingDamping;
    [SerializeField] private float reboundForce;
    private float normalDamping = 0f;
    private bool canMove = false;
    private Rigidbody2D rb2D;
    private Vector2 movementInput;

    // === Sprite ===
    [Header("Sprite Rotation")]
    [SerializeField] private float rotationSpeed;
    private float targetYRotation = 0f;

    // === Properties ===
    public bool CanMove { get => canMove; set => canMove = value; }

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();

        moveAction = GameManager.instance.InputManager.PlayerInput.actions["Move"];
    }

    void FixedUpdate()
    {
        // Horizontal movement
        rb2D.linearDamping = (movementInput.x == 0) ? slidingDamping : normalDamping; // Apply damping according to input

        if (movementInput.x != 0)
        {
            rb2D.AddForceX(movementInput.x * speed * Time.deltaTime, ForceMode2D.Force);
        }

        // Check that the player doesn't exceed the horizontal limits
        Vector2 position = rb2D.position;

        if (position.x < minXPos)
        {
            position.x = minXPos;
            rb2D.position = position;
            rb2D.linearVelocityX = 0;
            rb2D.AddForceX(reboundForce, ForceMode2D.Impulse); // Bounce to the right
        }
        else if (position.x > maxXPos)
        {
            position.x = maxXPos;
            rb2D.position = position;
            rb2D.linearVelocityX = 0;
            rb2D.AddForceX(-reboundForce, ForceMode2D.Impulse); // Bounce to the left
        }
    }

    void Update()
    {
        if (!canMove)
        {
            movementInput = Vector2.zero;
            return;
        }
        
        // Read movement input
        movementInput = moveAction.ReadValue<Vector2>();

        // Determine desired rotation according to direction
        if (movementInput.x > 0)
        {
            targetYRotation = 180f;
        }
        else if (movementInput.x < 0)
        {
            targetYRotation = 0f;
        }

        // Interpolate to the target rotation
        Quaternion targetRot = Quaternion.Euler(0, targetYRotation, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }
}