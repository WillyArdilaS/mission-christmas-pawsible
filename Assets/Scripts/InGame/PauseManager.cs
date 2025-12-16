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

    public void Pause(AbstractLevelManager levelManager)
    {
        previousState = levelManager.State;
        levelManager.State = AbstractLevelManager.GameState.InPause;

        pauseUI.SetActive(true);
        Time.timeScale = 0;

        audioManager.PauseMusic();
    }

    public void Continue(AbstractLevelManager levelManager)
    {
        levelManager.State = previousState;

        pauseUI.SetActive(false);
        Time.timeScale = 1;

        audioManager.UnPauseMusic();
    }

    public void PlayButtonSFX()
    {
        audioManager.PlayUISFX(audioManager.UISfxDictionary["Click"], audioManager.ButtonClickVol);
    }
}