using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // === Player ===
    private GameObject player;

    // === Counters ===
    private int berriesCounter = 0;
    private int orangesCounter = 0;
    private int pineconesCounter = 0;

    // === Dictionaries ===
    private Dictionary<string, int> fruitsDictionary;

    // === Events ===
    public event Action<string> ScoreChanged;
    public event Action<LevelSelectorManager.NextLevel> ScoreAchieved;

    // === Properties ===
    public Dictionary<string, int> FruitsDictionary => fruitsDictionary;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CollisionManagerLevel2>().FruitRecolected += AddPoint;
        player.GetComponent<CollisionManagerLevel2>().CoalRecolected += SubstractPoint;

        fruitsDictionary = new()
        {
            {"Berry", berriesCounter},
            {"Orange", orangesCounter},
            {"Pinecone", pineconesCounter}
        };
    }

    private void AddPoint(string tag)
    {
        if (fruitsDictionary[tag] < LevelManager2.instance.RequiredFruitsDictionary[tag])
        {
            fruitsDictionary[tag]++;
            ScoreChanged?.Invoke(tag);

            CheckPoints();
        }
    }

    private void SubstractPoint()
    {
        // Get only the keys for fruit counters that are greater than 0
        List<string> validKeys = new();

        foreach (var fruit in fruitsDictionary)
        {
            if (fruit.Value > 0) validKeys.Add(fruit.Key);
        }

        if (validKeys.Count == 0) return;

        // Pick random valid fruit
        string randomKey = validKeys[UnityEngine.Random.Range(0, validKeys.Count)];
        fruitsDictionary[randomKey]--;

        ScoreChanged?.Invoke(randomKey);
    }

    private void CheckPoints()
    {
        if (fruitsDictionary.All(fruit => fruit.Value >= LevelManager2.instance.RequiredFruitsDictionary[fruit.Key]))
        {
            ScoreAchieved?.Invoke(LevelSelectorManager.NextLevel.Level_03);
        }
    }
}