using System;
using System.Collections;
using UnityEngine;

public class LevelManager3 : AbstractLevelManager
{
    // === Singleton ===
    public static LevelManager3 instance;

    // === Managers ===
    private GameObject sequenceManager;
    private GameObject mapManager;
    private LightManager lightManager;
    private GameObject treeManager;

    // === Camera ===
    [Header("Camera")]
    [SerializeField] private float cameraSpeed;
    private CameraFollow cameraFollow;

    // === Player ===
    [Header("Player")]
    [SerializeField] private FoxController foxController;

    // === Animations ===
    [Header("Animation")]
    [SerializeField] private float transitionTime;
    private TreeAnimator treeAnimator;

    // === Coroutines ===
    private Coroutine resetRoundRoutine;
    private Coroutine openingAnimationRoutine;

    // === Events ===
    public event Action RoundStarted;

    // === Properties ===
    public GameObject SequenceManager => sequenceManager;
    public GameObject MapManager => mapManager;
    public LightManager LightManager => lightManager;
    public GameObject TreeManager => treeManager;
    public CameraFollow CameraFollow => cameraFollow;
    public float CameraSpeed => cameraSpeed;
    public float TransitionTime => transitionTime;

    // === Overridden Abstract Methods ===
    protected override void InitializeManagers()
    {
        if (pauseManager == null) pauseManager = GetComponentInChildren<PauseManager>();
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

        // transitionManager.GetComponent<TransitionManager>().TransitionFinished += StartNewRound;
        treeAnimator.LightedTree += StartResetRoundRoutine;
        treeAnimator.LightedTreeFinal += StartLevelFinishing;
    }

    void Start()
    {
        if (openingAnimationRoutine != null) StopCoroutine(openingAnimationRoutine);
        openingAnimationRoutine = StartCoroutine(ShowOpeningAnimation());
    }

    // === Round Management Methods ===
    void Update()
    {
        foxController.CanMove = (gameState == GameState.Playing);
    }

    private void StartResetRoundRoutine()
    {
        if (resetRoundRoutine != null) StopCoroutine(resetRoundRoutine);
        resetRoundRoutine = StartCoroutine(ResetRound());
    }

    private IEnumerator ResetRound()
    {
        if (sequenceManager.GetComponent<SequenceGenerator>().CurrentSequenceIndex != 0)
        {
            transitionAnim.SetTrigger("t_showSnowy");
            yield return new WaitForSeconds(transitionDelay);
        }

        RoundStarted?.Invoke();

        cameraFollow.ResetPosition();
        foxController.ResetPosition();
        lightManager.GetComponent<LightManager>().ResetActiveLights();
        treeAnimator.TurnOffTree();
    }

    // === Opening Animation ===
    private IEnumerator ShowOpeningAnimation()
    {
        LevelManager3.instance.State = LevelManager3.GameState.ShowingAnimation;

        // Horizontal camera movement
        float targetX = cameraFollow.MaxXPos;
        bool isMovingToLeft = false;
        bool isAnimationFinished = false;

        yield return new WaitForSeconds(transitionTime);
        while (isAnimationFinished == false)
        {
            Vector3 currentPos = Camera.main.transform.position;
            currentPos.x = Mathf.MoveTowards(currentPos.x, targetX, cameraSpeed * Time.deltaTime);
            Camera.main.transform.position = currentPos;

            yield return null;

            if (!isMovingToLeft && Camera.main.transform.position.x >= targetX)
            {
                targetX = cameraFollow.MinXPos;
                isMovingToLeft = true;
                yield return new WaitForSeconds(transitionTime);
            }
            if (isMovingToLeft && Camera.main.transform.position.x == targetX)
            {
                isAnimationFinished = true;
            }
        }

        yield return new WaitForSeconds(transitionTime);
        StartResetRoundRoutine();
    }
}