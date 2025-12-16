using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // === Target ===
    [SerializeField] private Transform target;
    private float targetXPos;

    // === Limits ===
    [Header("Limits")]
    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;

    // === Properties ===
    public float MaxXPos => maxXPos;

    void LateUpdate()
    {
        if (GameManagerLevel3.instance.State == GameManagerLevel3.GameState.Playing)
        {
            targetXPos = target.position.x;

            // Clamp horizontal position
            float clampedXPos = Mathf.Clamp(targetXPos, minXPos, maxXPos);

            // Apply new position
            Vector3 currentPos = transform.position;
            transform.position = new Vector3(clampedXPos, currentPos.y, currentPos.z);
        }
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(minXPos, transform.position.y, transform.position.z);
    }
}