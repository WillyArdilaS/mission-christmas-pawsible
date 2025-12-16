using TMPro;
using UnityEngine;

public class RaceUI : MonoBehaviour
{
    // === Laps ===
    [SerializeField] private TextMeshProUGUI lapCounter;
    [SerializeField] private int totalLaps;
    private int currentLap = 0;

    void Update()
    {
        if (currentLap == GameManagerLevel1.instance.CurrentLap) return;

        currentLap = GameManagerLevel1.instance.CurrentLap;
        if (currentLap == 0)
        {
            lapCounter.text = "";
        }
        else
        {
            lapCounter.text = $"{currentLap}/{totalLaps}";
        }
    }
}