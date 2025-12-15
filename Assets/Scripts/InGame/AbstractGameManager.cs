using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public abstract class AbstractGameManager : MonoBehaviour
{
    // === Managers ===
    protected PauseManager pauseManager;
    protected TransitionManager transitionManager;

    // === States ===
    public enum GameState { Playing, InPause, ShowingAnimation }
    [SerializeField] protected GameState gameState = GameState.Playing;

    // === Coroutines ===
    protected Coroutine gameTimerRoutine;
    private Coroutine finishLevelRoutine;

    // === Properties ===
    public TransitionManager TransitionManager => transitionManager;
    public GameState State { get => gameState; set => gameState = value; }

    // === Abstract Methods ===
    protected abstract void InitializeManagers();

    // === Level Start Methods ===
    protected virtual void Awake()
    {
        GlobalGameManager.instance.InputManager.PausePressed += HandlePause;
    }

    void OnDestroy()
    {
        GlobalGameManager.instance.InputManager.PausePressed -= HandlePause;
    }

    protected virtual IEnumerator StartGameTimer()
    {
        yield return null;
    }

    // === Level Finishing Methods ===
    protected void StartLevelFinishing(LevelSelectorManager.NextLevel nextLevel)
    {
        if (finishLevelRoutine != null) StopCoroutine(finishLevelRoutine);
        finishLevelRoutine = StartCoroutine(FinishLevel(nextLevel));
    }

    private IEnumerator FinishLevel(LevelSelectorManager.NextLevel nextLevel)
    {
        LevelSelectorManager.instance.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);

        LevelSelectorManager.instance.nextLevel = nextLevel;
        GlobalGameManager.instance.SceneSwitchManager.StartLoadScene("LevelSelector");
    }

    // === Pause Methods ===
    private void HandlePause()
    {
        if (gameState != GameState.InPause)
        {
            pauseManager.Pause(this);
        }
        else
        {
            pauseManager.Continue(this);
        }
    }
}