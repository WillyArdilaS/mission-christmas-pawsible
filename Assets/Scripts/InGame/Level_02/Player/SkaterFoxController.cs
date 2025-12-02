using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D), typeof(SpriteRenderer))]
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
        Vector2 xPosition = rb2D.position;

        if (xPosition.x < minXPos)
        {
            xPosition.x = minXPos;
            rb2D.position = xPosition;
            rb2D.linearVelocityX = 0;
            rb2D.AddForceX(reboundForce, ForceMode2D.Impulse); // Rebote hacia la derecha
        }
        else if (xPosition.x > maxXPos)
        {
            xPosition.x = maxXPos;
            rb2D.position = xPosition;
            rb2D.linearVelocityX = 0;
            rb2D.AddForceX(-reboundForce, ForceMode2D.Impulse); // Rebote hacia la izquierda
        }

    }

    private void MovePlayer(float xInput)
    {
        rb2D.AddForceX(xInput * speed * Time.deltaTime, ForceMode2D.Force);
        spriteRend.flipX = xInput > 0;

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