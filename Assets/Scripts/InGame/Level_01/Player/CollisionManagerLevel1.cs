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
        if (CanCrash)
        {
            if ((collision.CompareTag("SmallObstacle") && !sleighController.IsJumping) || collision.CompareTag("BigObstacle"))
            {
                HitObstacle?.Invoke();
                collision.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("FinishLine")) FinishLineCrossed?.Invoke();
    }
}