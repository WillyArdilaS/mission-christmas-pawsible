using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManagerLevel2 : MonoBehaviour
{
    // === Singleton ===
    public static GameManagerLevel2 instance;

    // === Managers ===
    private GameObject spawnManager;
    private GameObject scoreManager;

    // === States ===
    public enum GameState { Playing, InPause, Finishing }
    [SerializeField] private GameState gameState = GameState.Playing;

    // === Game Timer ===
    [Header("Game Timer")]
    [SerializeField] private int currentGameTime = 0;

    // === Fruits ===
    [Header("Fruits")]
    [SerializeField] private int requiredBerries;
    [SerializeField] private int requiredOranges;
    [SerializeField] private int requiredPinecones;
    private Dictionary<string, int> requiredFruitsDictionary;

    // === Coroutines ===
    private Coroutine gameTimerRoutine;

    // === Properties ===
    public GameObject SpawnManager => spawnManager;
    public GameObject ScoreManager => scoreManager;
    public GameState State { get => gameState; set => gameState = value; }
    public int CurrentGameTime => currentGameTime;
    public Dictionary<string, int> RequiredFruitsDictionary => requiredFruitsDictionary;

    void Awake()
    {
        Time.timeScale = 1;

        // Singleton
        if (instance == null)
        {
            instance = this;
            InitializeManagers();
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeDictionary();

        if (gameTimerRoutine != null) StopCoroutine(gameTimerRoutine);
        gameTimerRoutine = StartCoroutine(StartGameTimer());
    }

    void Update()
    {
        if(scoreManager.GetComponent<ScoreManager>().FruitsDictionary.All(fruit => fruit.Value >= requiredFruitsDictionary[fruit.Key]))
        {
            FinishGame();
        }
    }

    private void InitializeManagers()
    {
        if (spawnManager == null) spawnManager = transform.Find("SpawnManager").gameObject;
        if (scoreManager == null) scoreManager = transform.Find("ScoreManager").gameObject;
    }

    private void InitializeDictionary()
    {
        requiredFruitsDictionary = new()
        {
            {"Berry", requiredBerries},
            {"Orange", requiredOranges},
            {"Pinecone", requiredPinecones},
        };
    }

    private IEnumerator StartGameTimer()
    {
        while (gameState == GameState.Playing)
        {
            yield return new WaitForSeconds(1);
            currentGameTime++;
        }
    }

    private void FinishGame()
    {
        Debug.Log("Juego finalizado...");
        gameState = GameState.Finishing;
        Time.timeScale = 0;
    }
}