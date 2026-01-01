using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : MonoBehaviour
{
    // === Transition ===
    [SerializeField] private float transitionTime;
    private Animator transitionAnim;

    // === Coroutines ===
    private Coroutine loadSceneRoutine;

    void Awake()
    {
        transitionAnim = GetComponentInChildren<Animator>();
    }

    public string GetCurrentScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void StartLoadScene(string nextSceneName)
    {
        if (loadSceneRoutine != null) StopCoroutine(loadSceneRoutine);
        loadSceneRoutine = StartCoroutine(LoadScene(nextSceneName));
    }

    private IEnumerator LoadScene(string nextSceneName)
    {
        Time.timeScale = 1;
        transitionAnim.SetTrigger("t_closeScene");

        yield return new WaitForSeconds(transitionTime);

        GameManager.instance.AudioManager.SelectSceneMusic(nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }
}