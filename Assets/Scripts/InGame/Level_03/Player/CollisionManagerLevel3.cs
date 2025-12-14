using System;
using UnityEngine;

[RequireComponent(typeof(FoxController))]
public class CollisionManagerLevel3 : MonoBehaviour
{
    private bool goInsidePressedThisFrame = false;

    // === Events ===
    public event Action<int> HouseEntered;

    void Awake()
    {
        GlobalGameManager.instance.InputManager.GoInsidePressed += HandleGoInside;
    }

    private void HandleGoInside()
    {
        goInsidePressedThisFrame = true;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!goInsidePressedThisFrame) return;

        if (collision.TryGetComponent(out HouseLightController house))
        {
            HouseEntered?.Invoke(house.HouseID);
        }

        goInsidePressedThisFrame = false;
    }
}