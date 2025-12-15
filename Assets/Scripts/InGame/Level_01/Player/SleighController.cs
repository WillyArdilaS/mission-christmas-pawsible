using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D), typeof(Rigidbody2D))]
public class SleighController : MonoBehaviour
{
    // === Sprites ===
    [SerializeField] private Sprite[] sleighSprites;
    private SpriteRenderer spriteRend;
    private Collider2D col2D;
    private Vector2 originalColliderOffset;

    // === Movement ===
    [SerializeField] private Transform[] tracks;
    private int originalTrackIndex;
    private int currentTrackIndex;
    private bool canMove = true;

    // === Jump ===
    [SerializeField] private float jumpForce;
    private Rigidbody2D rb2D;
    private bool isJumping = false;

    // === Properties ===
    public bool CanMove { get => canMove; set => canMove = value; }
    public bool IsJumping => isJumping;

    // === Initialization Methods ===
    void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        col2D = GetComponent<Collider2D>();
        rb2D = GetComponent<Rigidbody2D>();

        LevelManager1.instance.LapRestarted += ResetPosition;

        // Tracks initialization
        tracks = tracks.OrderBy(track => track.transform.position.x).ToArray();

        currentTrackIndex = Array.FindIndex(tracks, track => Mathf.Approximately(track.position.x, transform.position.x));
        if (currentTrackIndex == -1)
        {
            Debug.LogWarning($"No se encontró un track con la misma posición X que {gameObject.name}");
        }

        originalTrackIndex = currentTrackIndex;
        originalColliderOffset = col2D.offset;
    }

    void OnEnable()
    {
        GameManager.instance.InputManager.MoveLeftPressed += OnMoveLeft;
        GameManager.instance.InputManager.MoveRightPressed += OnMoveRight;
        GameManager.instance.InputManager.JumpPressed += Jump;
    }

    void OnDisable()
    {
        GameManager.instance.InputManager.MoveLeftPressed -= OnMoveLeft;
        GameManager.instance.InputManager.MoveRightPressed -= OnMoveRight;
        GameManager.instance.InputManager.JumpPressed -= Jump;
    }

    private void OnMoveLeft()
    {
        ChangeTrack(-1);
    }

    private void OnMoveRight()
    {
        ChangeTrack(1);
    }

    // === Movement Methods ===
    void Update()
    {
        if (Mathf.Abs(rb2D.linearVelocityY) < 0.01f) isJumping = false;
    }

    private void ChangeTrack(int direction)
    {
        if (!canMove || isJumping) return;

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
        if (!canMove) return;
        isJumping = true;
        rb2D.AddForceY(jumpForce, ForceMode2D.Impulse);
    }

    private void ResetPosition()
    {
        currentTrackIndex = originalTrackIndex;

        transform.position = new Vector2(tracks[originalTrackIndex].position.x, transform.position.y);
        spriteRend.sprite = sleighSprites[originalTrackIndex];
        col2D.offset = originalColliderOffset;
    }
}