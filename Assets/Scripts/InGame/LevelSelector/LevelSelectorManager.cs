using UnityEngine;

[DefaultExecutionOrder(-1)]
public class LevelSelectorManager : MonoBehaviour
{
    // === Singleton ===
    public static LevelSelectorManager instance;

    // === Managers ===
    private InputManager inputManager;

    // === State ===
    public enum NextLevel { Level_01, Level_02, Level_03, Finished };
    public static NextLevel nextLevel = NextLevel.Level_01;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        inputManager = GetComponentInChildren<InputManager>();
        inputManager.LevelSelected += SelectLevel;
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
    }
}