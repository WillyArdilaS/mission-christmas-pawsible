using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D), typeof(Animator))]
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
    private bool canMove = false;
    private Rigidbody2D rb2D;
    private Vector2 movementInput;
    private Animator animator;

    // === Sprite ===
    [Header("Sprite Rotation")]
    [SerializeField] private float rotationSpeed;
    private float targetYRotation = 0f;

    // === Go Inside ===
    [Header("Go Inside Animation")]
    [SerializeField] private float transitionTime;
    private bool isGoingInside = false;

    // === Coroutines ===
    private Coroutine goingInsideRoutine;

    // === Events ===
    public event Action GoInsidePressed;

    // === Properties ===
    public bool CanMove { get => canMove; set => canMove = value; }

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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
            animator.SetBool("b_isWalking", true);
        }
    }

    void Update()
    {
        if (!canMove || isGoingInside)
        {
            movementInput = Vector2.zero;
            animator.SetBool("b_isWalking", false);
            return;
        }

        // Read movement input
        movementInput = moveAction.ReadValue<Vector2>();

        // Determine desired rotation according to direction
        if (movementInput.x > 0)
        {
            targetYRotation = 0f;
        }
        else if (movementInput.x < 0)
        {
            targetYRotation = 180f;
        }
        else
        {
            animator.SetBool("b_isWalking", false);
        }

        // Interpolate to the target rotation
        Quaternion targetRot = Quaternion.Euler(0, targetYRotation, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }

    private void OnActionTriggered(InputAction.CallbackContext ctx)
    {
        if (ctx.action.actionMap.name == "Player" && ctx.performed)
        {
            if (ctx.action.name == "Go Inside" && !isGoingInside)
            {
                if (goingInsideRoutine != null) StopCoroutine(goingInsideRoutine);
                goingInsideRoutine = StartCoroutine(GoInside());
                GoInsidePressed?.Invoke();
            }
        }
    }

    private IEnumerator GoInside()
    {
        isGoingInside = true;
        animator.SetBool("b_isLookingBack", true);

        yield return new WaitForSeconds(transitionTime);

        isGoingInside = false;
        animator.SetBool("b_isLookingBack", false);
    }

    public void ResetPosition()
    {
        rb2D.position = new Vector2(minXPos, rb2D.position.y);
        rb2D.linearVelocityX = 0;

        targetYRotation = 0f;
        transform.rotation = Quaternion.Euler(0, targetYRotation, 0);
    }
}