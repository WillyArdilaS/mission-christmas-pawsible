using System;
using System.Collections;
using UnityEngine;

public class TreeAnimator : MonoBehaviour
{
    // === Managers ===
    private SequenceManager sequenceManager;
    private SequenceGenerator sequenceGenerator;
    private LightManager lightManager;

    // === Tree ===
    [SerializeField] private GameObject treeLight;

    // === Camera ===
    private Camera mainCam;
    private CameraFollow cameraFollow;
    private float cameraSpeed;

    // === Animation ===
    private float transitionTime;

    [Header("Tree Animation Settings")]
    [SerializeField] private float houseLightTransitionTime;

    [Header("Final Animation Settings")]
    [SerializeField] private Vector3 targetCamPosition;
    [SerializeField] private float targetZoom;
    [SerializeField] private float zoomSpeed;

    // === Coroutines ===
    private Coroutine treeAnimationRoutine;
    private Coroutine finalTreeAnimationRoutine;

    // === Events ===
    public event Action LightedTree;
    public event Action<LevelSelectorManager.NextLevel> LightedTreeFinal;

    void Awake()
    {
        sequenceManager = LevelManager3.instance.SequenceManager.GetComponent<SequenceManager>();
        sequenceGenerator = LevelManager3.instance.SequenceManager.GetComponent<SequenceGenerator>();
        lightManager = LevelManager3.instance.LightManager;

        mainCam = Camera.main;
        cameraFollow = LevelManager3.instance.CameraFollow;
        cameraSpeed = LevelManager3.instance.CameraSpeed;

        transitionTime = LevelManager3.instance.TransitionTime;

        sequenceManager.SequenceMatched += StartTreeAnimation;
    }

    public void TurnOffTree()
    {
        treeLight.SetActive(false);
    }

    // === Tree Animation Methods ===
    private void StartTreeAnimation()
    {
        if (treeAnimationRoutine != null) StopCoroutine(treeAnimationRoutine);
        treeAnimationRoutine = StartCoroutine(ShowTreeIlumination());
    }

    private IEnumerator ShowTreeIlumination()
    {
        LevelManager3.instance.State = LevelManager3.GameState.ShowingAnimation;
        LevelManager3.instance.SetMapUI(false);

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

        // Extend the animation only if it is the final round
        yield return new WaitForSeconds(transitionTime);
        if (!sequenceGenerator.IsFinalRound)
        {
            LightedTree?.Invoke();
        }
        else
        {
            if (finalTreeAnimationRoutine != null) StopCoroutine(finalTreeAnimationRoutine);
            finalTreeAnimationRoutine = StartCoroutine(ShowFinalTreeIlumination());
        }
    }

    private IEnumerator ShowFinalTreeIlumination()
    {
        Vector3 startPos = mainCam.transform.position;

        float startZoom = mainCam.orthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * zoomSpeed;

            mainCam.transform.position = Vector3.Lerp(startPos, targetCamPosition, elapsedTime); // Interpolate position
            mainCam.orthographicSize = Mathf.Lerp(startZoom, targetZoom, elapsedTime); // Interpolate zoom

            yield return null;
        }

        // Adjust it to the exact final values.
        mainCam.transform.position = targetCamPosition;
        mainCam.orthographicSize = targetZoom;

        yield return new WaitForSeconds(transitionTime);
        LightedTreeFinal?.Invoke(LevelSelectorManager.NextLevel.Finished);
    }
}