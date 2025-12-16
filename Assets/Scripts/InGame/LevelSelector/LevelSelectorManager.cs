using UnityEngine;

[DefaultExecutionOrder(-1)]
public class LevelSelectorManager : MonoBehaviour
{
    // === Singleton ===
    public static LevelSelectorManager instance;

    // === State ===
    public enum NextLevel { Level_01, Level_02, Level_03, Finished };
    public NextLevel nextLevel = NextLevel.Level_01;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        GlobalGameManager.instance.InputManager.SelectLevelPressed += SelectLevel;
    }

    void OnDisable()
    {
        if (GlobalGameManager.instance != null) GlobalGameManager.instance.InputManager.SelectLevelPressed -= SelectLevel;
    }

    public void ResetLevelSelector()
    {
        gameObject.SetActive(true);
        nextLevel = NextLevel.Level_01;
    }

    private void SelectLevel()
    {
        switch (nextLevel)
        {
            case NextLevel.Level_01:
                ChangeScene("Level_01");
                break;
            case NextLevel.Level_02:
                ChangeScene("Level_02");
                break;
            case NextLevel.Level_03:
                ChangeScene("Level_03");
                break;
            case NextLevel.Finished:
                ChangeScene("MainMenu");
                break;
        }
    }

    private void ChangeScene(string sceneName)
    {
        GlobalGameManager.instance.SceneSwitchManager.StartLoadScene(sceneName);
        gameObject.SetActive(false);
    }
}