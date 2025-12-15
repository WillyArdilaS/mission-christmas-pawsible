using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // === UI ===
    [SerializeField] private GameObject pauseUI;

    // === Game state ===
    private AbstractGameManager.GameState previousState;

    void Awake()
    {
        pauseUI.SetActive(false);
    }


    public void Pause(AbstractGameManager gameManager)
    {
        previousState = gameManager.State;
        gameManager.State = AbstractGameManager.GameState.InPause;

        pauseUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue(AbstractGameManager gameManager)
    {
        gameManager.State  = previousState;

        pauseUI.SetActive(false);
        Time.timeScale = 1;
    }
}