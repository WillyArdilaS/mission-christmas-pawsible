using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(SpriteRenderer), typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class SleighController : MonoBehaviour
{
    // === Input ===
    private PlayerInput playerInput;

    // === Sprites ===
    [SerializeField] private Sprite[] sleighSprites;
    private SpriteRenderer spriteRend;
    private Collider2D col2D;
    private Vector2 originalColliderOffset;

    // === Movement ===
    [SerializeField] private Transform[] tracks;
    private int originalTrackIndex;
    private int currentTrackIndex;
    
    // === Jump ===
    [SerializeField] private float jumpForce;
    private Rigidbody2D rb2D;
    private bool isJumping = false;

    // === Properties ===
    public bool IsJumping => isJumping;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        spriteRend = GetComponent<SpriteRenderer>();
        col2D = GetComponent<Collider2D>();
        rb2D = GetComponent<Rigidbody2D>();

        GameManagerLevel1.instance.LapRestarted += RestartPosition;
        playerInput.onActionTriggered += OnActionTriggered;

        tracks = tracks.OrderBy(track => track.transform.position.x).ToArray();

        currentTrackIndex = Array.FindIndex(tracks, track => Mathf.Approximately(track.position.x, transform.position.x));
        if (currentTrackIndex == -1)
        {
            Debug.LogWarning($"No se encontró un track con la misma posición X que {gameObject.name}");
        }

        originalTrackIndex = currentTrackIndex;
        originalColliderOffset = col2D.offset;
    }

    void Update()
    {
        if (rb2D.linearVelocityY == 0) isJumping = false;
    }

    private void OnActionTriggered(InputAction.CallbackContext ctx)
    {
        if (ctx.action.actionMap.name == "Sleigh" && ctx.started)
        {
            switch (ctx.action.name)
            {
                case "Move Left":
                    ChangeTrack(-1);
                    break;
                case "Move Right":
                    ChangeTrack(1);
                    break;
                case "Jump / Go Inside":
                    Jump();
                    break;
                default:
                    Debug.LogWarning($"La acción '{ctx.action.name}' no existe en el action map '{ctx.action.actionMap.name}'");
                    break;
            }
        }
    }

    private void ChangeTrack(int direction)
    {
        if (isJumping) return;

        if ((direction == -1 && currentTrackIndex > 0) || (direction == 1 && currentTrackIndex < tracks.Length - 1))
        {
            currentTrackIndex += direction;
            transform.position = new Vector2(tracks[currentTrackIndex].position.x, transform.position.y);

            spriteRend.sprite = sleighSprites[currentTrackIndex];
            col2D.offset = new Vector2(col2D.offset.x - direction, col2D.offset.y);
        }
    }

    private void Jump()
    {
        isJumping = true;
        rb2D.AddForceY(jumpForce, ForceMode2D.Impulse);
    }

    private void RestartPosition()
    {
        currentTrackIndex = originalTrackIndex;
        
        transform.position = new Vector2(tracks[originalTrackIndex].position.x, transform.position.y);
        spriteRend.sprite = sleighSprites[originalTrackIndex];
        col2D.offset = originalColliderOffset;
    }
}