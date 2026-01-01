using System.Linq;
using UnityEngine;

public class TrackAnimator : MonoBehaviour
{
    // === Managers ===
    [SerializeField] private RaceGenerator raceGenerator;

    // === Animator ===
    [SerializeField] private Animator lightsAnimator;

    // === Animation Datas Management ===
    [SerializeField] private AnimationEventData[] animationEvents;
    private AnimationEventData currentAnimationEvent;

    void Start()
    {
        raceGenerator.spawnEventChanged += ChangeAnimationSpeed;
    }

    private void ChangeAnimationSpeed(SpawnEventData spawnEvent)
    {
        if (spawnEvent == null)
        {
            currentAnimationEvent = animationEvents[0];
        }
        else
        {
            currentAnimationEvent = animationEvents.FirstOrDefault(animEvent => animEvent.Minutes == spawnEvent.Minutes && animEvent.Seconds == spawnEvent.Seconds);
        }

        if(currentAnimationEvent != null) lightsAnimator.SetFloat("f_speedMultiplier", currentAnimationEvent.SpeedMultiplier);
    }
}