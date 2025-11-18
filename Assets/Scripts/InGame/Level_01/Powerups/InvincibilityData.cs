using UnityEngine;

[CreateAssetMenu(fileName = "New Invincibility", menuName = "ScriptableObjects/Powerups/Invincibility")]
public class InvincibilityData : PowerupData
{
    // === Extra Data Fields ===
    [SerializeField] private float transparency;

    // === Properties ===
    public float Transparency => transparency;
}