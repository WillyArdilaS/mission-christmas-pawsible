using UnityEngine;

public class RestartManager : MonoBehaviour
{
    // === Managers ===
    [SerializeField] private CollisionManagerLevel3 collisionManager;
    private AudioManager audioManager;

    // === Player ===
    [SerializeField] private FoxController foxController;

    // === UI ===
    [Header("UI")]
    [SerializeField] private GameObject restartUI;

    void Start()
    {
        audioManager = GameManager.instance.AudioManager;

        collisionManager.RestartZoneEntered += ShowRestartMenu;
    }

    private void ShowRestartMenu()
    {
        LevelManager3.instance.State = AbstractLevelManager.GameState.InPause;
        restartUI.SetActive(true);
    }

    public void PlayButtonSFX()
    {
        audioManager.PlayUISFX(audioManager.UISfxDictionary["Click"], audioManager.ButtonClickVol);
    }

    public void ContinuePlaying()
    {
        restartUI.SetActive(false);

        LevelManager3.instance.State = AbstractLevelManager.GameState.Playing;
        foxController.ResetPosition();
    }

    public void Restart()
    {
        restartUI.SetActive(false);

        LevelManager3.instance.IsRestarting = true;
        LevelManager3.instance.StartNewRoundRoutine();
    }
}