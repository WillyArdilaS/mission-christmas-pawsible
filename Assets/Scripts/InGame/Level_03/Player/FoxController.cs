using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D))]
public class FoxController : MonoBehaviour
{
    // === Input ===
    private PlayerInput playerInput;
    private InputAction moveAction;

    // === Limits ===
    [Header("Limits")]
    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;

    // === Movement ===
    [Header("Movement")]
    [SerializeField] private float speed;
    private Rigidbody2D rb2D;
    private Vector2 movementInput;

    // === Sprite ===
    [Header("Sprite Rotation")]
    [SerializeField] private float rotationSpeed;
    private float targetYRotation = 180f;

    // === Enter The House ===
    private bool isEntering = false;

    // === Properties ===
    public bool IsEntering { get => isEntering; set => isEntering = value; }

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb2D = GetComponent<Rigidbody2D>();

        playerInput.onActionTriggered += OnActionTriggered;
        moveAction = playerInput.actions["Move"];
    }

    void FixedUpdate()
    {
        // Predict next X position
        float xInput = movementInput.x;

        Vector2 currentPos = rb2D.position;
        float desiredVelocity = xInput * speed;
        float nextXPos = currentPos.x + desiredVelocity * Time.fixedDeltaTime;

        // Check that the player doesn't exceed the horizontal limits
        if (nextXPos < minXPos)
        {
            currentPos.x = minXPos;
            rb2D.position = currentPos;
            rb2D.linearVelocityX = 0;
        }
        else if (nextXPos > maxXPos)
        {
            currentPos.x = maxXPos;
            rb2D.position = currentPos;
            rb2D.linearVelocityX = 0;
        }
        else
        {
            // Horizontal movement
            rb2D.linearVelocityX = desiredVelocity;
        }
    }

    void Update()
    {
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

    private void OnActionTriggered(InputAction.CallbackContext ctx)
    {
        if (ctx.action.actionMap.name == "Player" && ctx.started)
        {
            if (ctx.action.name == "Go Inside") isEntering = true;
        }
    }
}