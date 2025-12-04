using UnityEngine;

public class HouseLightController : MonoBehaviour
{
    // === Player ===
    private GameObject player;

    // === House ===
    [SerializeField] private int houseID;

    // === Light ===
    private GameObject houseLight;
    private bool isOn = false;

    // === Properties ===
    public int HouseID => houseID;
    public bool IsOn => isOn;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        houseLight = transform.GetChild(0).gameObject;

        player.GetComponent<CollisionManagerLevel3>().HouseEntered += SwitchLight;
    }

    void Update()
    {
        houseLight.SetActive(isOn);
    }

    private void SwitchLight(int triggerID)
    {
        if (triggerID == houseID)
        {
            isOn = !isOn;
            player.GetComponent<FoxController>().IsEntering = false;
        }
    }
}