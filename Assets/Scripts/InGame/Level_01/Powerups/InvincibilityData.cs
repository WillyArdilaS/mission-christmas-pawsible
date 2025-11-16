using UnityEngine;

[CreateAssetMenu(fileName = "New Invincibility", menuName = "ScriptableObjects/Powerups/Invincibility")]
public class InvincibilityData : PowerupData
{
    // === Extra data fields ===
    [SerializeField] private float transparency;

    // === Properties ===
    public float Transparency => transparency;
}