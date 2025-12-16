using TMPro;
using UnityEngine;

public class RaceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lapCounter;
    private int currentLap = 0;

    void Update()
    {
        if (currentLap == GameManagerLevel1.instance.CurrentLap) return;
        if (GameManagerLevel1.instance.CurrentLap > GameManagerLevel1.instance.TotalLaps) return;

        currentLap = GameManagerLevel1.instance.CurrentLap;
        
        if (currentLap == 0)
        {
            lapCounter.text = "";
        }
        else
        {
            lapCounter.text = $"{currentLap}/{GameManagerLevel1.instance.TotalLaps}";
        }
    }
}