using System;
using UnityEngine;

[Serializable]
public class SpawnEventData
{
    // === Settings ===
    [Header("Time Settings")]
    [SerializeField, Min(0)] private int minutes;
    [SerializeField, Range(0, 59)] private int seconds;

    [Header("Spawn Settings")]
    [SerializeField] private SpawnManagerBase.SpanwedType spawnedType;
    [SerializeField, Range(0f, 1f), Tooltip("Only for obstacles")] private float doubleSpawnChance;
    [SerializeField, Tooltip("Only for obstacles and powerups")] private float newSpawnRate;
    [SerializeField] private float newGlobalSpeed;
    
    // === Properties ===
    public int Minutes => minutes;
    public int Seconds => seconds;
    public SpawnManagerBase.SpanwedType SpawnedType => spawnedType;
    public float DoubleSpawnChance => doubleSpawnChance;
    public float NewSpawnRate => newSpawnRate;
    public float NewGlobalSpeed => newGlobalSpeed;
}