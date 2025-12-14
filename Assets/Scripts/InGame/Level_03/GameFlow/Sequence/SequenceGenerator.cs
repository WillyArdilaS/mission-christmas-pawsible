using UnityEngine;

[RequireComponent(typeof(SequenceManager))]
public class SequenceGenerator : MonoBehaviour
{
    // === Managers ===
    private SequenceManager sequenceManager;

    // === Sequence Events Management ===
    [SerializeField] private SequenceEventData[] sequenceEvents;
    private SequenceEventData currentSequenceEvent;
    private int currentSequenceIndex = 0;
    private bool isFinalRound = false;
    
    // === Animation ===
    private MapLightsAnimator mapAnimator;

    // === Properties ===
    public bool IsFinalRound => isFinalRound;

    void Awake()
    {
        sequenceManager = GetComponent<SequenceManager>();
        mapAnimator = GameManagerLevel3.instance.MapManager.GetComponent<MapLightsAnimator>();

        GameManagerLevel3.instance.RoundStarted += NextRound;
    }

    private void NextRound()
    {
        if(currentSequenceIndex == sequenceEvents.Length - 1) isFinalRound = true;

        CreateSequence();
    }

    private void CreateSequence()
    {
        currentSequenceEvent = sequenceEvents[currentSequenceIndex];

        sequenceManager.Size = currentSequenceEvent.NewSize;
        sequenceManager.Min = currentSequenceEvent.NewMin;
        sequenceManager.Max = currentSequenceEvent.NewMax;
        
        sequenceManager.ResetSequence();
        mapAnimator.StartAnimation(sequenceManager.SequenceList);

        currentSequenceIndex++;
    }
}