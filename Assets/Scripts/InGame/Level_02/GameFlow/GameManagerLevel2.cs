using System.Collections;
using UnityEngine;

public class GameManagerLevel2 : MonoBehaviour
{
    // === Singleton ===
    public static GameManagerLevel2 instance;

    // === Managers ===
    private GameObject spawnManager;

    // === States ===
    public enum GameState { Playing, InPause }
    [SerializeField] private GameState gameState = GameState.Playing;

    // === Game Timer ===
    [Header("Game duration")]
    [SerializeField, Min(0)] private int minutes;
    [SerializeField, Range(0, 59)] private int seconds;
    [SerializeField] private int currentGameTime = 0;
    private int gameDuration;

    // === Fruits ===
    [Header("Fruits")]
    [SerializeField] private int requiredBerries;
    [SerializeField] private int requiredOranges;
    [SerializeField] private int requiredPinecones;
    private int berriesCounter;
    private int orangesCounter;
    private int pineconesCounter;

    // === Coroutines ===
    private Coroutine gameTimerRoutine;

    // === Events ===
    
    // === Properties ===
    public GameObject SpawnManager => spawnManager;
    public GameState State { get => gameState; set => gameState = value; }
    public int CurrentGameTime => currentGameTime;
    public int TotalGameDuration => gameDuration;

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

        // Initialize timer
        gameDuration = (minutes * 60) + seconds;

        if (gameTimerRoutine != null) StopCoroutine(gameTimerRoutine);
        gameTimerRoutine = StartCoroutine(StartGameTimer());
    }

    void Update()
    {
        
    }

    private void InitializeManagers()
    {
        if (spawnManager == null) spawnManager = transform.Find("SpawnManager").gameObject;
    }

    private IEnumerator StartGameTimer()
    {
        while (currentGameTime < gameDuration)
        {
            yield return new WaitForSeconds(1);
            currentGameTime++;
        }
    }
}