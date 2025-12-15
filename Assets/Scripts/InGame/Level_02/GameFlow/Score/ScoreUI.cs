using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ScoreManager))]
public class ScoreUI : MonoBehaviour
{
    // === Manager ===
    private ScoreManager scoreManager;

    // === Counters ===
    [SerializeField] private TextMeshProUGUI[] fruitCounters;
    private Dictionary<string, TextMeshProUGUI> fruitCountersDictionary;
    

    void Start()
    {
        scoreManager = GetComponent<ScoreManager>();

        scoreManager.ScoreChanged += UpdateScore;

        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        fruitCountersDictionary = new()
        {
            {"Berry", fruitCounters.FirstOrDefault(counter => counter.name == "BerryCounter")},
            {"Orange", fruitCounters.FirstOrDefault(counter => counter.name == "OrangeCounter")},
            {"Pinecone", fruitCounters.FirstOrDefault(counter => counter.name == "PineconeCounter")},
        };

        fruitCountersDictionary["Berry"].text = $"{scoreManager.FruitsDictionary["Berry"]}/{LevelManager2.instance.RequiredFruitsDictionary["Berry"]}";
        fruitCountersDictionary["Orange"].text = $"{scoreManager.FruitsDictionary["Orange"]}/{LevelManager2.instance.RequiredFruitsDictionary["Orange"]}";
        fruitCountersDictionary["Pinecone"].text = $"{scoreManager.FruitsDictionary["Pinecone"]}/{LevelManager2.instance.RequiredFruitsDictionary["Pinecone"]}";
    }

    private void UpdateScore(string fruit)
    {
        fruitCountersDictionary[fruit].text = $"{scoreManager.FruitsDictionary[fruit]}/{LevelManager2.instance.RequiredFruitsDictionary[fruit]}";
    }
}