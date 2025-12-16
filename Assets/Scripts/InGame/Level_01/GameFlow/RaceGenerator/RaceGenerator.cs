using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceGenerator : MonoBehaviour
{
    // === Spawn Settings ===
    [SerializeField] private SpawnManagerBase spawnManager;
    private float currentSpawnRate;
    private float spawnTimer = 0f;

    // === Difficulty Events Management ===
    [SerializeField] private DifficultyEventLevel1Data[] difficultyEvents;
    private DifficultyEventLevel1Data currentDifficultyEvent;
    private readonly List<int> originalDifficultyEventTimes = new();
    private Queue<int> pendingTimes;
    private int currentGlobalTime = 0;

    void Start()
    {
        // Convert all timestamps into total seconds and sort them in ascending order
        IOrderedEnumerable<int> times = difficultyEvents.Select(difEvent => (difEvent.Minutes * 60) + difEvent.Seconds).OrderBy(difEvent => difEvent);
        foreach (var time in times)
        {
            originalDifficultyEventTimes.Add(time); // Add timestamp to list for sequential processing
        }

        InitializeRace();
    }

    void Update()
    {
        currentGlobalTime = GameManagerLevel1.instance.CurrentGameTime;

        // Check if there are pending timestamps AND if the current time matches the next scheduled timestamp
        if (pendingTimes.Count > 0 && currentGlobalTime == pendingTimes.Peek())
        {
            int matchTime = pendingTimes.Dequeue(); // Remove the matched timestamp from the queue
            currentDifficultyEvent = difficultyEvents.First(difEvent => (difEvent.Minutes * 60) + difEvent.Seconds == matchTime);

            ChangeDifficulty(currentDifficultyEvent);
        }

        StartSpawnLoop();
    }

    public void InitializeRace()
    {
        currentDifficultyEvent = null;
        currentGlobalTime = GameManagerLevel1.instance.CurrentGameTime;
        currentSpawnRate = 0f;
        spawnTimer = 0f;
        pendingTimes = new Queue<int>(originalDifficultyEventTimes);

        FilterPendingTimes();
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

    private void ChangeDifficulty(DifficultyEventLevel1Data difficultyEvent)
    {
        spawnManager.SpawnRate = difficultyEvent.NewSpawnRate;
        spawnManager.GlobalSpeed = difficultyEvent.NewGlobalSpeed;

        currentSpawnRate = difficultyEvent.NewSpawnRate;
    }

    private void StartSpawnLoop()
    {
        if (currentDifficultyEvent == null) return;

        if (currentDifficultyEvent.SpawnedType == SpawnManagerBase.SpanwedType.Goal)
        {
            spawnManager.SpawnObject(1);
            currentDifficultyEvent = null;
        }
        else
        {
            // Spawn object acording to the spawn rate
            spawnTimer += Time.deltaTime;

            if (currentSpawnRate != 0 && spawnTimer >= currentSpawnRate)
            {
                int spawnCount = 1;

                if (currentDifficultyEvent.DoubleSpawnChance == 1 || Random.value < currentDifficultyEvent.DoubleSpawnChance) spawnCount = 2;

                spawnManager.SpawnObject(spawnCount);
                spawnTimer = 0f;
            }
        }
    }
}