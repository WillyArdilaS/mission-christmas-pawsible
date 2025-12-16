using System;
using System.Collections;
using UnityEngine;

public class TreeAnimator : MonoBehaviour
{
    // === Managers ===
    private SequenceManager sequenceManager;
    private LightManager lightManager;

    // === Tree ===
    [SerializeField] private GameObject treeLight;

    // === Camera ===
    [SerializeField] private float cameraSpeed;
    private Camera mainCam;
    private CameraFollow cameraFollow;

    // === Animation ===
    [Header("Animation Time Settings")]
    [SerializeField] private float transitionTime;
    [SerializeField] private float houseLightTransitionTime;

    // === Coroutines ===
    private Coroutine treeAnimationRoutine;

    // === Events ===
    public event Action LightedTree;

    void Awake()
    {
        sequenceManager = GameManagerLevel3.instance.SequenceManager.GetComponent<SequenceManager>();
        lightManager = GameManagerLevel3.instance.LightManager.GetComponent<LightManager>();

        mainCam = Camera.main;
        cameraFollow = Camera.main.GetComponent<CameraFollow>();

        sequenceManager.SequenceMatched += StartTreeAnimation;
    }

    private void StartTreeAnimation()
    {
        if (treeAnimationRoutine != null) StopCoroutine(treeAnimationRoutine);
        treeAnimationRoutine = StartCoroutine(ShowTreeIlumination());
    }

    private IEnumerator ShowTreeIlumination()
    {
        GameManagerLevel3.instance.State = GameManagerLevel3.GameState.ShowingTree;

        // Turn on all the house lights
        WaitForSeconds lightDelay = new WaitForSeconds(houseLightTransitionTime);
        for (int i = 0; i < lightManager.HouseLights.Length; i++)
        {
            yield return lightDelay;
            lightManager.HouseLights[i].SetLight(true);
        }

        // Horizontal camera movement
        yield return new WaitForSeconds(transitionTime);
        float targetX = cameraFollow.MaxXPos;

        while (mainCam.transform.position.x < targetX)
        {
            Vector3 currentPos = mainCam.transform.position;
            currentPos.x = Mathf.MoveTowards(currentPos.x, targetX, cameraSpeed * Time.deltaTime);
            mainCam.transform.position = currentPos;

            yield return null;
        }

        // Turn on the tree light
        yield return new WaitForSeconds(transitionTime);
        treeLight.SetActive(true);

        yield return new WaitForSeconds(transitionTime);
        LightedTree?.Invoke();
    }

    public void TurnOffTree()
    {
        treeLight.SetActive(false);
    }
}