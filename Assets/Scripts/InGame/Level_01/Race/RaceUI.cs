using TMPro;
using UnityEngine;

public class RaceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lapCounter;
    private int currentLap = 0;

    void Update()
    {
        if (currentLap == LevelManager1.instance.CurrentLap) return;
        if (LevelManager1.instance.CurrentLap > LevelManager1.instance.TotalLaps) return;

        currentLap = LevelManager1.instance.CurrentLap;
        
        if (currentLap == 0)
        {
            lapCounter.text = "";
        }
        else
        {
            lapCounter.text = $"{currentLap}/{LevelManager1.instance.TotalLaps}";
        }
    }
}