using System;
using UnityEngine;

public class GameManagerLevel3 : AbstractGameManager
{
    // === Singleton ===
    public static GameManagerLevel3 instance;

    // === Managers ===
    private GameObject sequenceManager;
    private GameObject mapManager;
    private LightManager lightManager;
    private GameObject treeManager;

    // === Camera ===
    private CameraFollow cameraFollow;

    // === Player ===
    [SerializeField] private FoxController foxController;

    // === Animation ===
    private TreeAnimator treeAnimator;

    // === Events ===
    public event Action RoundStarted;

    // === Properties ===
    public GameObject SequenceManager => sequenceManager;
    public GameObject MapManager => mapManager;
    public LightManager LightManager => lightManager;
    public GameObject TreeManager => treeManager;

    // === Overridden Abstract Methods ===
    protected override void InitializeManagers()
    {
        if (pauseManager == null) pauseManager = GetComponentInChildren<PauseManager>();
        if (transitionManager == null) transitionManager = GetComponentInChildren<TransitionManager>();
        if (sequenceManager == null) sequenceManager = transform.Find("SequenceManager").gameObject;
        if (mapManager == null) mapManager = transform.Find("MapManager").gameObject;
        if (lightManager == null) lightManager = GetComponentInChildren<LightManager>();
        if (treeManager == null) treeManager = transform.Find("TreeManager").gameObject;
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
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        treeAnimator = treeManager.GetComponent<TreeAnimator>();

        transitionManager.GetComponent<TransitionManager>().TransitionFinished += StartNewRound;
        treeAnimator.LightedTreeFinal += StartLevelFinishing;
    }

    // === Round Management Methods ===
    void Start()
    {
        StartNewRound();
    }

    void Update()
    {
        foxController.CanMove = (gameState == GameState.Playing);
    }

    private void StartNewRound()
    {
        RoundStarted?.Invoke();

        cameraFollow.ResetPosition();
        foxController.ResetPosition();
        lightManager.GetComponent<LightManager>().ResetActiveLights();
        treeAnimator.TurnOffTree();
    }
}