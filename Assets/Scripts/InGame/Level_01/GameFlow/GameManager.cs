using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // === Singleton ===
    public static GameManager instance;

    // === Managers ===
    private GameObject raceManager;
    private GameObject trackManager;

    // === States ===
    public enum GameState { Playing, InPause }
    [SerializeField] private GameState gameState = GameState.Playing;

    // === Game Timer ===
    [Header("Game duration")]
    [SerializeField, Min(0)] private int minutes;
    [SerializeField, Range(0, 59)] private int seconds;
    [SerializeField] private int currentGameTime = 0;
    private int gameDuration;

    // === Player ===
    [Header("Player Lives")]
    [SerializeField] private LifeManager playerLifeManager;
    private bool hasExtraLife = true;

    // === Race ===
    [Header("Race")]
    [SerializeField] private int currentLap = 0;
    private const int LAP_DURATION = 60;

    // === Coroutines ===
    private Coroutine gameTimerRoutine;

    // === Events ===
    public event Action LapRestarted;

    // === Properties ===
    public GameObject TrackManager => trackManager;
    public GameState State { get => gameState; set => gameState = value; }
    public int CurrentGameTimer => currentGameTime;
    public int TotalGameDuration => gameDuration;
    public int CurrentLap { get => currentLap; set => currentLap = value; }

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
        if (playerLifeManager.LifeCounter == 0 && hasExtraLife)
        {
            RestartLap();
        }
        else if (playerLifeManager.LifeCounter == 0 && !hasExtraLife)
        {
            RestartRace();
        }
    }

    private void InitializeManagers()
    {
        if (raceManager == null) raceManager = transform.Find("RaceManager").gameObject;
        if (trackManager == null) trackManager = transform.Find("TrackManager").gameObject;
    }

    private IEnumerator StartGameTimer()
    {
        while (currentGameTime < gameDuration)
        {
            yield return new WaitForSeconds(1);
            currentGameTime++;
        }
    }

    private void RestartLap()
    {
        int lapStartTime = (currentGameTime / LAP_DURATION) * LAP_DURATION;
        currentGameTime = lapStartTime;
        currentLap--;
        playerLifeManager.AddLife();
        hasExtraLife = false;

        RaceGenerator[] raceGenerators = raceManager.GetComponents<RaceGenerator>();
        foreach (var raceGenerator in raceGenerators)
        {
            raceGenerator.InitializeRace();
        }

        StopCoroutine(gameTimerRoutine);
        gameTimerRoutine = StartCoroutine(StartGameTimer());

        LapRestarted?.Invoke();
    }

    private void RestartRace()
    {
        currentGameTime = 0;
        currentLap = 0;
        playerLifeManager.ResetLives();
        hasExtraLife = true;

        RaceGenerator[] raceGenerators = raceManager.GetComponents<RaceGenerator>();
        foreach (var raceGenerator in raceGenerators)
        {
            raceGenerator.InitializeRace();
        }

        StopCoroutine(gameTimerRoutine);
        gameTimerRoutine = StartCoroutine(StartGameTimer());

        LapRestarted?.Invoke();
    }
}