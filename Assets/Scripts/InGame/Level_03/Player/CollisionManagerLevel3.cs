using System;
using UnityEngine;

[RequireComponent(typeof(FoxController))]
public class CollisionManagerLevel3 : MonoBehaviour
{
    private FoxController foxController;

    // === Events ===
    public event Action<int> HouseEntered;

    void Awake()
    {
        foxController = GetComponent<FoxController>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (foxController.IsEntering)
        {
            int triggerID = collision.GetComponent<HouseLightController>().HouseID;
            HouseEntered?.Invoke(triggerID);
        }
    }
}