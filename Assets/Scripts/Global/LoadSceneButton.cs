using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadSceneButton : MonoBehaviour
{
    // === Managers ===
    private AudioManager audioManager;

    // === Scene Settings ===
    [SerializeField] private string sceneName;

    // === UI ===
    private Button buttonUI;

    void OnEnable()
    {
        audioManager = GlobalGameManager.instance.AudioManager;
        buttonUI = GetComponent<Button>();

        buttonUI.onClick.RemoveAllListeners();
        buttonUI.onClick.AddListener(ChangeScene);
    }

    private void ChangeScene()
    {
        if (GlobalGameManager.instance.SceneSwitchManager.GetCurrentScene() == "MainMenu" && sceneName == "LevelSelector")
        {
            if (LevelSelectorManager.instance != null) LevelSelectorManager.instance.ResetLevelSelector();
        }

        audioManager.PlayUISFX(audioManager.UISfxDictionary["Click"], audioManager.ButtonClickVol);
        GlobalGameManager.instance.SceneSwitchManager.StartLoadScene(sceneName);
    }
}