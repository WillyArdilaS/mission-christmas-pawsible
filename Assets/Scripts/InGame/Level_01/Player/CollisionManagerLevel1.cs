using System;
using UnityEngine;

[RequireComponent(typeof(SleighController))]
public class CollisionManagerLevel1 : MonoBehaviour
{
    // === Sleigh ===
    private SleighController sleighController;
    private bool canCrash = true;

    // === Events ===
    public event Action HitObstacle;
    public event Action FinishLineCrossed;

    // === Properties ===
    public bool CanCrash { get => canCrash; set => canCrash = value; }

    void Awake()
    {
        sleighController = GetComponent<SleighController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (canCrash)
        {
            if (collision.CompareTag("BigObstacle"))
            {
                HitObstacle?.Invoke();
                collision.gameObject.SetActive(false);
            }
        }

        if (collision.CompareTag("FinishLine")) FinishLineCrossed?.Invoke();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // Collisions with small obstacles should be detected here to avoid being ignored when jumping
        if (canCrash)
        {
            if (collision.CompareTag("SmallObstacle") && !sleighController.IsJumping)
            {
                HitObstacle?.Invoke();
                collision.gameObject.SetActive(false);
                canCrash = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SmallObstacle"))
        {
            canCrash = true;
        }
    }
}