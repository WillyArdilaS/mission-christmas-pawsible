using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // === Managers ===
    private AudioManager audioManager;

    // === UI ===
    [SerializeField] private GameObject pauseUI;

    // === Game state ===
    private AbstractLevelManager.GameState previousState;

    void Awake()
    {
        audioManager = GameManager.instance.AudioManager;
        pauseUI.SetActive(false);
    }

    public void Pause(AbstractLevelManager gameManager)
    {
        previousState = gameManager.State;
        gameManager.State = AbstractLevelManager.GameState.InPause;

        pauseUI.SetActive(true);
        Time.timeScale = 0;

        audioManager.PauseMusic();
    }

    public void Continue(AbstractLevelManager gameManager)
    {
        gameManager.State = previousState;

        pauseUI.SetActive(false);
        Time.timeScale = 1;


        audioManager.UnPauseMusic();
    }

    public void PlayButtonSFX()
    {
        audioManager.PlayUISFX(audioManager.UISfxDictionary["Click"], audioManager.ButtonClickVol);
    }
}