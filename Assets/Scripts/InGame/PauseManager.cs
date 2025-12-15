using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // === Managers ===
    private AudioManager audioManager;

    // === UI ===
    [SerializeField] private GameObject pauseUI;

    // === Game state ===
    private AbstractGameManager.GameState previousState;

    void Awake()
    {
        audioManager = GlobalGameManager.instance.AudioManager;
        pauseUI.SetActive(false);
    }

    public void Pause(AbstractGameManager gameManager)
    {
        previousState = gameManager.State;
        gameManager.State = AbstractGameManager.GameState.InPause;

        pauseUI.SetActive(true);
        Time.timeScale = 0;

        audioManager.PauseMusic();
    }

    public void Continue(AbstractGameManager gameManager)
    {
        gameManager.State  = previousState;

        pauseUI.SetActive(false);
        Time.timeScale = 1;

        audioManager.PlayUISFX(audioManager.UISfxDictionary["Click"], audioManager.ButtonClickVol);
        audioManager.UnPauseMusic();
    }
}