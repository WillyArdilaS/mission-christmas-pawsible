using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadSceneButton : MonoBehaviour
{
    // === Scene Settings ===
    [SerializeField] private string sceneName;

    // === UI ===
    private Button buttonUI;

    void OnEnable()
    {
        buttonUI = GetComponent<Button>();

        buttonUI.onClick.RemoveAllListeners();
        buttonUI.onClick.AddListener(ChangeScene);
    }

    private void ChangeScene()
    {
        if(GlobalGameManager.instance.SceneSwitchManager.GetCurrentScene() == "MainMenu" && sceneName == "LevelSelector")
        {
            if (LevelSelectorManager.instance != null) LevelSelectorManager.instance.ResetLevelSelector();
        }

        GlobalGameManager.instance.SceneSwitchManager.StartLoadScene(sceneName);
    }
}