using System;
using UnityEngine;

[RequireComponent(typeof(SleighController))]
public class CollisionManager : MonoBehaviour
{
    // === Sleigh ===
    private SleighController sleighController;
    private bool canCrash = true;

    // === Events ===
    public event Action ObstacleHit;

    // === Properties ===
    public bool CanCrash { get => canCrash; set => canCrash = value; }

    void Awake()
    {
        sleighController = GetComponent<SleighController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (CanCrash)
        {
            if ((collision.CompareTag("SmallObstacle") && !sleighController.IsJumping) || collision.CompareTag("BigObstacle"))
            {
                ObstacleHit?.Invoke();
                collision.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Goal")) GameManagerLevel1.instance.CurrentLap++;
    }
}