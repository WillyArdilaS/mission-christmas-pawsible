using System;
using UnityEngine;

public class GameManagerLevel3 : AbstractGameManager
{
    // === Singleton ===
    public static GameManagerLevel3 instance;

    // === Managers ===
    private GameObject sequenceManager;
    private GameObject mapManager;
    private GameObject lightManager;
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
    public GameObject LightManager => lightManager;
    public GameObject TreeManager => treeManager;

    // === Overridden Abstract Methods ===
    protected override void InitializeManagers()
    {
        if (transitionManager == null) transitionManager = transform.Find("TransitionManager").gameObject;
        if (sequenceManager == null) sequenceManager = transform.Find("SequenceManager").gameObject;
        if (mapManager == null) mapManager = transform.Find("MapManager").gameObject;
        if (lightManager == null) lightManager = transform.Find("LightManager").gameObject;
        if (treeManager == null) treeManager = transform.Find("TreeManager").gameObject;
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
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        treeAnimator = treeManager.GetComponent<TreeAnimator>();

        transitionManager.GetComponent<TransitionManager>().TransitionFinished += StartNewRound;
        treeAnimator.LightedTreeFinal += StartLevelFinishing;
    }

    void Start()
    {
        StartNewRound();
    }

    void Update()
    {
        foxController.CanMove = (gameState == GameState.Playing);
    }

    // === Round Management Methods ===
    private void StartNewRound()
    {
        RoundStarted?.Invoke();

        cameraFollow.ResetPosition();
        foxController.ResetPosition();
        lightManager.GetComponent<LightManager>().ResetActiveLights();
        treeAnimator.TurnOffTree();
    }
}