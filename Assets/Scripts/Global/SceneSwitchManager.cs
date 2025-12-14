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

    public void StartLoadScene(string sceneName)
    {
        if (loadSceneRoutine != null) StopCoroutine(loadSceneRoutine);
        loadSceneRoutine = StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        transitionAnim.SetTrigger("t_closeScene");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}