using UnityEngine;

public class TransitionEventTrigger : MonoBehaviour
{
    // === Game Manager Type ===
    public enum GameManager { Level1, Level3 };
    [SerializeField] private GameManager gameManager;

    // === Manager ===
    private TransitionManager transitionManager;

    void Awake()
    {
        switch (gameManager)
        {
            // case GameManager.Level1: 
            //     transitionManager = GameManagerLevel1.instance.TransitionManager.GetComponent<TransitionManager>();
            //     break;
            case GameManager.Level3: 
                transitionManager = GameManagerLevel3.instance.TransitionManager.GetComponent<TransitionManager>();
                break;
        }
        
    }

    public void TriggerTransitionFinished()
    {
        if (transitionManager != null) transitionManager.OnTransitionAnimationFinished();
    }
}