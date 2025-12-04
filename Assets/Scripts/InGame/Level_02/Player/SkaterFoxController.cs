using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class SkaterFoxController : MonoBehaviour
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
    [SerializeField] private float slidingDamping;
    [SerializeField] private float reboundForce;
    private float normalDamping = 0f;
    private Rigidbody2D rb2D;
    private Vector2 movementInput;

    // === Sprite ===
    [Header("Sprite")]
    [SerializeField] private Vector2 newColliderOffset;
    private SpriteRenderer spriteRend;
    private Collider2D col2D;
    private Vector2 originalColliderOffset;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb2D = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        col2D = GetComponent<Collider2D>();

        moveAction = playerInput.actions["Move"];
        
        originalColliderOffset = col2D.offset;
    }

    void FixedUpdate()
    {
        // Horizontal movement
        movementInput = moveAction.ReadValue<Vector2>();
        rb2D.linearDamping = (movementInput.x == 0) ? slidingDamping : normalDamping; // Apply damping according to input

        if (movementInput.x != 0) MovePlayer(movementInput.x);

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

    private void MovePlayer(float xInput)
    {
        rb2D.AddForceX(xInput * speed * Time.deltaTime, ForceMode2D.Force);

        if (xInput > 0)
        {
            spriteRend.flipX = true;
            col2D.offset = newColliderOffset;
        }
        else
        {
            spriteRend.flipX = false;
            col2D.offset = originalColliderOffset;
        }
    }
}