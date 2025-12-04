using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private HouseLightController[] houseLights;
    [SerializeField] private List<int> activeLights = new();

    void Awake()
    {
        houseLights = houseLights.OrderBy(light => light.HouseID).ToArray();

        foreach (var houseLight in houseLights)
        {
            houseLight.LightSwitched += UpdateActiveLights;
        }
    }

    private void UpdateActiveLights(HouseLightController currentHouseLight)
    {
        if (currentHouseLight.IsOn)
        {
            if(!activeLights.Contains(currentHouseLight.HouseID)) activeLights.Add(currentHouseLight.HouseID);
        }
        else
        {
            activeLights.Remove(currentHouseLight.HouseID);
        }
    }
}