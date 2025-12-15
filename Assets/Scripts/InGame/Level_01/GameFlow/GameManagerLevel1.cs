using System;
using System.Collections;
using UnityEngine;

public class GameManagerLevel1 : AbstractGameManager
{
    // === Singleton ===
    public static GameManagerLevel1 instance;

    // === Managers ===
    private GameObject raceManager;
    private TrackManager trackManager;

    // === Game Timer ===
    [Header("Game Duration")]
    [SerializeField, Min(0)] private int minutes;
    [SerializeField, Range(0, 59)] private int seconds;
    [SerializeField, Tooltip("It should start at -1 due to the delay at the beginning")] private int currentGameTime;
    private int gameDuration;

    // === Player ===
    [Header("Player")]
    [SerializeField] private GameObject player;
    private SleighController sleighController;
    private LifeManager lifeManager;
    private CollisionManagerLevel1 collisionManager;
    private bool hasExtraLife = true;

    // === Race ===
    [Header("Race")]
    [SerializeField, Range(0, 3)] private float startRaceDelay;
    [SerializeField] private int totalLaps;
    private const int LAP_DURATION = 60;
    private int currentLap = 0;

    // === Events ===
    public event Action LapRestarted;

    // === Properties ===
    public TrackManager TrackManager => trackManager;
    public int CurrentGameTime => currentGameTime;
    public int TotalGameDuration => gameDuration;
    public int TotalLaps => totalLaps;
    public int CurrentLap => currentLap;
    public int LapDuration => LAP_DURATION;

    // === Overridden Abstract Methods ===
    protected override void InitializeManagers()
    {
        if (pauseManager == null) pauseManager = GetComponentInChildren<PauseManager>();
        if (raceManager == null) raceManager = transform.Find("RaceManager").gameObject;
        if (trackManager == null) trackManager = GetComponentInChildren<TrackManager>();
    }

    protected override IEnumerator StartGameTimer()
    {
        yield return new WaitForSeconds(startRaceDelay);

        while (currentGameTime < gameDuration)
        {
            currentGameTime++;
            yield return new WaitForSeconds(1);
        }
    }

    protected override void Awake()
    {
        base.Awake();

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
        sleighController = player.GetComponent<SleighController>();
        lifeManager = player.GetComponent<LifeManager>();
        collisionManager = player.GetComponent<CollisionManagerLevel1>();

        collisionManager.FinishLineCrossed += UpdateLap;

        gameDuration = (minutes * 60) + seconds;

        if (gameTimerRoutine != null) StopCoroutine(gameTimerRoutine);
        gameTimerRoutine = StartCoroutine(StartGameTimer());
    }

    // === Race Management Methods ===
    void Update()
    {
        sleighController.CanMove = (gameState == GameState.Playing);

        if (lifeManager.LifeCounter == 0 && hasExtraLife)
        {
            ResetLap();
        }
        else if (lifeManager.LifeCounter == 0 && !hasExtraLife)
        {
            ResetRace();
        }
    }

    private void UpdateLap()
    {
        currentLap++;

        if (currentLap > totalLaps)
        {
            StartLevelFinishing(LevelSelectorManager.NextLevel.Level_02);
        }
    }

    private void ResetLap()
    {
        int lapStartTime = (currentGameTime / LAP_DURATION) * LAP_DURATION;
        currentGameTime = lapStartTime;
        currentLap--;
        lifeManager.AddLife();
        hasExtraLife = false;

        ResetTimer();
    }

    private void ResetRace()
    {
        currentGameTime = 0;
        currentLap = 0;
        lifeManager.ResetLives();
        hasExtraLife = true;

        ResetTimer();
    }

    private void ResetTimer()
    {
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