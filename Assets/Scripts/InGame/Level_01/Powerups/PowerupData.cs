using UnityEngine;

public class PowerupData : ScriptableObject
{
    // === Data fields ===
    public enum PowerupType { Invincibility }
    [SerializeField] private PowerupType powerupType;
    [SerializeField] protected float duration;

    // === Properties ===
    public float Duration => duration;
    public PowerupType Type => powerupType;
}