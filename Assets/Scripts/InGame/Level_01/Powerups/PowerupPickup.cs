using UnityEngine;

public class PowerupPickup : MonoBehaviour
{
    [SerializeField] private PowerupData powerupData;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PowerupManager>().ApplyPowerup(powerupData);
            gameObject.SetActive(false);
        }
    }
}