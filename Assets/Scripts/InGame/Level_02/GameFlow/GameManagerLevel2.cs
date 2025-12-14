using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerLevel2 : AbstractGameManager
{
    // === Singleton ===
    public static GameManagerLevel2 instance;

    // === Managers ===
    private GameObject spawnManager;
    private GameObject scoreManager;

    // === Game Timer ===
    [Header("Game Timer")]
    [SerializeField] private int currentGameTime = 0;

    // === Player ===
    [Header("Player")]
    [SerializeField] private SkaterFoxController skaterFoxController;

    // === Fruits ===
    [Header("Fruits")]
    [SerializeField] private int requiredBerries;
    [SerializeField] private int requiredOranges;
    [SerializeField] private int requiredPinecones;
    private Dictionary<string, int> requiredFruitsDictionary;

    // === Properties ===
    public GameObject SpawnManager => spawnManager;
    public GameObject ScoreManager => scoreManager;
    public int CurrentGameTime => currentGameTime;
    public Dictionary<string, int> RequiredFruitsDictionary => requiredFruitsDictionary;

    // === Overridden Abstract Methods ===
    protected override void InitializeManagers()
    {
        if (spawnManager == null) spawnManager = transform.Find("SpawnManager").gameObject;
        if (scoreManager == null) scoreManager = transform.Find("ScoreManager").gameObject;
    }

    protected override IEnumerator StartGameTimer()
    {
        while (gameState == GameState.Playing)
        {
            yield return new WaitForSeconds(1);
            currentGameTime++;
        }
    }

    // === Initialization Methods ===
    void Awake()
    {
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

        // Initialization
        InitializeDictionary();

        scoreManager.GetComponent<ScoreManager>().ScoreAchieved += StartLevelFinishing;

        if (gameTimerRoutine != null) StopCoroutine(gameTimerRoutine);
        gameTimerRoutine = StartCoroutine(StartGameTimer());
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

    void Update()
    {
        // skaterFoxController.CanMove = (gameState == GameState.Playing);
    }
}