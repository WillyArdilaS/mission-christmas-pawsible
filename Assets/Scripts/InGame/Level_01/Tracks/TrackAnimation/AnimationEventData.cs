using System;
using UnityEngine;

[Serializable]
public class AnimationEventData
{
    // === Settings ===
    [Header("Time Settings")]
    [SerializeField, Min(0)] private int minutes;
    [SerializeField, Range(0, 59)] private int seconds;

    [Header("Clip Settings")]
    [SerializeField, Range(1, 5)] private float speedMultiplier;

    // === Properties ===
    public int Minutes => minutes;
    public int Seconds => seconds;
    public float SpeedMultiplier => speedMultiplier;
}