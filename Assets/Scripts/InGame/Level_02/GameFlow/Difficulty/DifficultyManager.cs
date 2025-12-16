using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DifficultyManager : MonoBehaviour
{
    // === Spawn Settings ===
    private SpawnManager spawnManager;
    private float currentSpawnRate;
    private float spawnTimer = 0f;

    // === Difficulty Events Management ===
    [SerializeField] private DifficultyEventLevel2Data[] difficultyEvents;
    private DifficultyEventLevel2Data currentDifficultyEvent;
    private Queue<int> pendingTimes = new();
    private int currentGlobalTime = 0;

    void Start()
    {
        spawnManager = GameManagerLevel2.instance.SpawnManager.GetComponent<SpawnManager>();

        // Convert all timestamps into total seconds and sort them in ascending order
        IOrderedEnumerable<int> times = difficultyEvents.Select(difEvent => (difEvent.Minutes * 60) + difEvent.Seconds).OrderBy(difEvent => difEvent);
        foreach (var time in times)
        {
            pendingTimes.Enqueue(time); // Add timestamp to queue for sequential processing
        }
    }

    void Update()
    {
        currentGlobalTime = GameManagerLevel2.instance.CurrentGameTime;

        // Check if there are pending timestamps AND if the current time matches the next scheduled timestamp
        if (pendingTimes.Count > 0 && currentGlobalTime == pendingTimes.Peek())
        {
            int matchTime = pendingTimes.Dequeue(); // Remove the matched timestamp from the queue
            currentDifficultyEvent = difficultyEvents.First(difEvent => (difEvent.Minutes * 60) + difEvent.Seconds == matchTime);

            ChangeDifficulty(currentDifficultyEvent);
        }

        StartSpawnLoop();
    }

    private void ChangeDifficulty(DifficultyEventLevel2Data difficultyEvent)
    {
        spawnManager.SpawnRate = difficultyEvent.NewSpawnRate;
        currentSpawnRate = difficultyEvent.NewSpawnRate;
    }

    private void StartSpawnLoop()
    {
        if (currentDifficultyEvent == null) return;

        // Spawn object acording to the spawn rate
        spawnTimer += Time.deltaTime;

        if (currentSpawnRate != 0 && spawnTimer >= currentSpawnRate)
        {
            if (currentDifficultyEvent.CoalSpawnChance == 1 || Random.value < currentDifficultyEvent.CoalSpawnChance)
            {
                spawnManager.SpawnCoal();
            } else
            {
                spawnManager.SpawnFruit();
            }
            
            spawnTimer = 0f;
        }
    }
}