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

        buttonUI.onClick.AddListener(ChangeScene);
    }

    private void ChangeScene()
    {
        GlobalGameManager.instance.SceneSwitchManager.StartLoadScene(sceneName);
    }
}