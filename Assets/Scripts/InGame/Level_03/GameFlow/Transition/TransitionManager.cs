using System;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private TreeAnimator treeAnimator;

    // === Animation ===
    [SerializeField] private Animator transitionAnim;

    // === Events ===
    public event Action TransitionFinished;

    void Awake()
    {
        treeAnimator = LevelManager3.instance.TreeManager.GetComponent<TreeAnimator>();
        treeAnimator.LightedTree += StartTransitionRoutine;
    }

    private void StartTransitionRoutine()
    {
        transitionAnim.SetTrigger("t_showSnowy");
    }

    public void OnTransitionAnimationFinished()
    {
        TransitionFinished?.Invoke();
    }
}