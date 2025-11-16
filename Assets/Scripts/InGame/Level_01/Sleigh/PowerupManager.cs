using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(CollisionManager))]
public class PowerupManager : MonoBehaviour
{
    // === Invincibility ===
    private SpriteRenderer spriteRend;
    private CollisionManager collisionManager;

    // === Fade Settings ===
    [SerializeField] protected float fadeBlinkDuration;
    [SerializeField] protected int fadeBlinksQuantity;

    // === Coroutines ===
    private Coroutine invincibilityRoutine;
    private Coroutine finishPowerupRoutine;

    void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        collisionManager = GetComponent<CollisionManager>();
    }

    public void ApplyPowerup(PowerupData data)
    {
        switch (data.Type)
        {
            case PowerupData.PowerupType.Invincibility:
                var invData = (InvincibilityData)data;

                if (invincibilityRoutine != null) StopCoroutine(invincibilityRoutine);
                invincibilityRoutine = StartCoroutine(StartInvincibility(invData.Duration, invData.Transparency));
                
                break;
        }
    }

    public IEnumerator StartInvincibility(float duration, float transparency)
    {
        spriteRend.color = new Color(1, 1, 1, transparency);
        collisionManager.CanCrash = false;

        yield return new WaitForSeconds(duration);

        if (finishPowerupRoutine != null) StopCoroutine(finishPowerupRoutine);
        finishPowerupRoutine = StartCoroutine(FinishPowerup());
    }

    private IEnumerator FinishPowerup()
    {
        Color prevColor = spriteRend.color;
        Color newColor = Color.white;

        for (int i = 0; i < fadeBlinksQuantity; i++)
        {
            spriteRend.color = newColor;
            newColor = prevColor;
            prevColor = spriteRend.color;


            yield return new WaitForSeconds(fadeBlinkDuration);
        }

        spriteRend.color = Color.white;
        collisionManager.CanCrash = true;
    }
}