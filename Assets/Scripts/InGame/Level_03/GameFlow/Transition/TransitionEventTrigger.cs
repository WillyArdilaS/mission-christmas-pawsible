using UnityEngine;

public class TransitionEventTrigger : MonoBehaviour
{
    [SerializeField] private AbstractGameManager gameManager;
    private TransitionManager transitionManager;

    void Awake()
    {
        if (gameManager != null)
        {
            transitionManager = gameManager.TransitionManager.GetComponent<TransitionManager>();
        } else
        {
            Debug.LogWarning($"No se le ha asignado un GameManager a {name}");
        }
    }

    public void TriggerTransitionFinished()
    {
        if (transitionManager != null) transitionManager.OnTransitionAnimationFinished();
    }
}