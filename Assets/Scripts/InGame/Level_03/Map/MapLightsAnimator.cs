using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLightsAnimator : MonoBehaviour
{
    // === Sequence Animation ===
    [SerializeField] private GameObject[] mapLights;
    [SerializeField] private float lightOnDuration;
    private float waitingTime;

    // === Coroutines ===
    private Coroutine lightSequenceRoutine;

    void Awake()
    {
        waitingTime = LevelManager3.instance.WaitingTime;
    }

    public void StartAnimation(List<int> sequence)
    {
        if (lightSequenceRoutine != null) StopCoroutine(lightSequenceRoutine);
        lightSequenceRoutine = StartCoroutine(ShowLightSequence(sequence));
    }

    private IEnumerator ShowLightSequence(List<int> sequence)
    {
        LevelManager3.instance.State = LevelManager3.GameState.ShowingAnimation;
        
        foreach (var number in sequence)
        {
            GameObject currentLight = Array.Find(mapLights, light => light.name == number.ToString());

            if (currentLight == null)
            {
                Debug.LogWarning($"No se encontró la casa número {number} en el mapa");
                continue;
            }

            // Turn on and off animation
            yield return new WaitForSeconds(waitingTime);
            currentLight.SetActive(true);
            
            yield return new WaitForSeconds(lightOnDuration);
            currentLight.SetActive(false);
        }

        LevelManager3.instance.State = LevelManager3.GameState.Playing;
    }
}