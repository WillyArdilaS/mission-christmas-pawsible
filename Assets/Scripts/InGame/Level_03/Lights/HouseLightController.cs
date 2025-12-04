using System;
using UnityEngine;

public class HouseLightController : MonoBehaviour
{
    // === Player ===
    private CollisionManagerLevel3 collisionManager;

    // === House ===
    [SerializeField] private int houseID;

    // === Light ===
    private GameObject houseLight;
    private bool isOn = false;

    // === Events ===
    public event Action<HouseLightController> LightSwitched;

    // === Properties ===
    public int HouseID => houseID;
    public bool IsOn => isOn;

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        collisionManager = player.GetComponent<CollisionManagerLevel3>();

        houseLight = transform.GetChild(0).gameObject;
        houseLight.SetActive(false);

        collisionManager.HouseEntered += SwitchLight;
    }

    private void SwitchLight(int triggerID)
    {
        if (triggerID != houseID) return;

        isOn = !isOn;
        houseLight.SetActive(isOn);
        LightSwitched?.Invoke(this);
    }
}