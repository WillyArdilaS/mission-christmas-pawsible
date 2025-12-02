using System;
using UnityEngine;

[Serializable]
public class DifficultyEventLevel2Data
{
    // === Settings ===
    [Header("Time Settings")]
    [SerializeField, Min(0)] private int minutes;
    [SerializeField, Range(0, 59)] private int seconds;

    [Header("Spawn Settings")]
    [SerializeField, Range(0f, 1f), Tooltip("Only for obstacles")] private float coalSpawnChance;
    [SerializeField, Tooltip("Only for obstacles and powerups")] private float newSpawnRate;

    // === Properties ===
    public int Minutes => minutes;
    public int Seconds => seconds;
    public float CoalSpawnChance => coalSpawnChance;
    public float NewSpawnRate => newSpawnRate;
}