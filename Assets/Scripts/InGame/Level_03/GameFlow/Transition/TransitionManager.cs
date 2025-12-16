using System;
using System.Collections;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private TreeAnimator treeAnimator;

    // === Animation ===
    [SerializeField] private Animator snowTransition;

    // === Events ===
    public event Action TransitionFinished;

    void Awake()
    {
        treeAnimator = GameManagerLevel3.instance.TreeManager.GetComponent<TreeAnimator>();
        treeAnimator.LightedTree += StartTransitionRoutine;
    }

    private void StartTransitionRoutine()
    {
        snowTransition.SetTrigger("t_showTransition");
    }

    public void OnTransitionAnimationFinished()
    {
        TransitionFinished?.Invoke();
    }
}