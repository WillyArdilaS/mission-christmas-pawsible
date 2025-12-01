using System;
using UnityEngine;

public class CollisionManagerLevel2 : MonoBehaviour
{
    // === Events ===
    public event Action<string> FruitRecolected;
    public event Action CoalRecolected;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Berry") || collision.CompareTag("Orange") || collision.CompareTag("Pinecone"))
        {
            FruitRecolected?.Invoke(collision.tag);
        }

        if(collision.CompareTag("Coal")) CoalRecolected?.Invoke();

        collision.gameObject.SetActive(false);
    }
}