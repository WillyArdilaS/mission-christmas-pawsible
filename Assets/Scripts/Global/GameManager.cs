using UnityEngine;

[DefaultExecutionOrder(-2)]
public class GameManager : MonoBehaviour
{
    // === Singleton ===
    public static GameManager instance;

    // === Managers ===
    private InputManager inputManager;
    private SceneSwitchManager sceneSwitchManager;
    private AudioManager audioManager;

    // === Properties ===
    public InputManager InputManager => inputManager;
    public SceneSwitchManager SceneSwitchManager => sceneSwitchManager;
    public AudioManager AudioManager => audioManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioManager.SelectSceneMusic(sceneSwitchManager.GetCurrentScene());
    }

    private void InitializeManagers()
    {
        if (inputManager == null) inputManager = GetComponentInChildren<InputManager>();
        if (sceneSwitchManager == null) sceneSwitchManager = GetComponentInChildren<SceneSwitchManager>();
        if (audioManager == null) audioManager = GetComponentInChildren<AudioManager>();
    }
}