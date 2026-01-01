using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceGenerator : MonoBehaviour
{
    // === Spawn Settings ===
    [SerializeField] private SpawnManagerBase spawnManager;
    private float currentSpawnRate = 0f;
    private float spawnTimer = 0f;

    // === Difficulty Events Management ===
    [SerializeField] private SpawnEventData[] spawnEvents;
    private SpawnEventData previousSpawnEvent;
    private SpawnEventData currentSpawnEvent;
    private readonly List<int> originalSpawnEventTimes = new();
    private Queue<int> pendingTimes;
    private int currentGlobalTime = 0;

    // === Events ===
    public event Action<SpawnEventData> spawnEventChanged;

    // === Properties ===
    public SpawnEventData CurrentSpawnEvent => currentSpawnEvent;
    public Queue<int> PendingTimes => pendingTimes;

    void Start()
    {
        // Convert all timestamps into total seconds and sort them in ascending order
        IOrderedEnumerable<int> times = spawnEvents.Select(spawnEvent => (spawnEvent.Minutes * 60) + spawnEvent.Seconds).OrderBy(spawnEvent => spawnEvent);
        foreach (var time in times)
        {
            originalSpawnEventTimes.Add(time); // Add timestamp to list for sequential processing
        }

        InitializeRace();
    }

    void Update()
    {
        currentGlobalTime = LevelManager1.instance.CurrentGameTime;

        if(currentSpawnEvent != previousSpawnEvent)
        {
            previousSpawnEvent = currentSpawnEvent;
            spawnEventChanged?.Invoke(currentSpawnEvent);
        }

        // Check if there are pending timestamps AND if the current time matches the next scheduled timestamp
        if (pendingTimes.Count > 0 && currentGlobalTime == pendingTimes.Peek())
        {
            int matchTime = pendingTimes.Dequeue(); // Remove the matched timestamp from the queue
            
            currentSpawnEvent = spawnEvents.First(spawnEvent => (spawnEvent.Minutes * 60) + spawnEvent.Seconds == matchTime);

            ChangeDifficulty(currentSpawnEvent);
        }

        StartSpawnLoop();
    }

    // === Initialization Methods ===
    public void InitializeRace()
    {
        currentGlobalTime = LevelManager1.instance.CurrentGameTime;
        pendingTimes = new Queue<int>(originalSpawnEventTimes);
        spawnTimer = 0f;

        if (currentGlobalTime == -1)
        {
            currentSpawnEvent = null;
            currentSpawnRate = 0f;
        }

        FilterPendingTimes();
        SetCurrentSpawnEvent();
    }

    private void FilterPendingTimes()
    {
        Queue<int> filtered = new();

        foreach (int timestamp in pendingTimes)
        {
            if (timestamp >= currentGlobalTime) filtered.Enqueue(timestamp);
        }

        pendingTimes = filtered;
    }

    private void SetCurrentSpawnEvent()
    {
        if (currentSpawnEvent == null || pendingTimes.Count == 0) return;
 
        SpawnEventData previousSpawnEvent = spawnEvents.TakeWhile(spawnEvent => (spawnEvent.Minutes * 60) + spawnEvent.Seconds < pendingTimes.Peek()).LastOrDefault();

        if (previousSpawnEvent != null)
        {
            currentSpawnEvent = previousSpawnEvent;
            currentSpawnRate = currentSpawnEvent.NewSpawnRate;
        }
    }

    // === Race Management Methods ===
    private void ChangeDifficulty(SpawnEventData spawnEvent)
    {
        spawnManager.SpawnRate = spawnEvent.NewSpawnRate;
        spawnManager.GlobalSpeed = spawnEvent.NewGlobalSpeed;

        currentSpawnRate = spawnEvent.NewSpawnRate;
    }

    private void StartSpawnLoop()
    {
        if (currentSpawnEvent == null) return;

        if (currentSpawnEvent.SpawnedType == SpawnManagerBase.SpanwedType.Goal)
        {
            spawnManager.SpawnObject(1);
            currentSpawnEvent = null;
        }
        else
        {
            // Spawn object acording to the spawn rate
            spawnTimer += Time.deltaTime;

            if (currentSpawnRate != 0 && spawnTimer >= currentSpawnRate)
            {
                int spawnCount = 1;

                if (currentSpawnEvent.DoubleSpawnChance == 1 || UnityEngine.Random.value < currentSpawnEvent.DoubleSpawnChance) spawnCount = 2;

                spawnManager.SpawnObject(spawnCount);
                spawnTimer = 0f;
            }
        }
    }
}