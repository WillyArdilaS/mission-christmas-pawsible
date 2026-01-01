using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CollisionManagerLevel1 collisionManager;

    // === Shake Settings ===
    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float shakeSpeed;

    // === Camera ===
    private Transform mainCam;
    private Vector3 originalPosition;

    // === Coroutines ===
    private Coroutine shakeRoutine;

    void Awake()
    {
        mainCam = transform;
        originalPosition = mainCam.localPosition;

        collisionManager.HitObstacle += StartShake;
    }

    private void StartShake()
    {
        if (shakeRoutine != null) StopCoroutine(shakeRoutine);
        shakeRoutine = StartCoroutine(ShakeCamera());
    }
    private IEnumerator ShakeCamera()
    {
        //
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime * shakeSpeed;

            // Interpolate position
            float newX = Random.Range(-1, 1) * shakeMagnitude;
            float newY = Random.Range(-1, 1) * shakeMagnitude;

            Vector3 newPosition = new(newX, newY, originalPosition.z);
            mainCam.localPosition = Vector3.Lerp(originalPosition, newPosition, elapsedTime);

            yield return null;
        }

        //
        while (Vector3.Distance(mainCam.localPosition, originalPosition) > 0.01f)
        {
            mainCam.localPosition = Vector3.Lerp(mainCam.localPosition, originalPosition, Time.deltaTime * shakeSpeed);
            yield return null;
        }

        mainCam.localPosition = originalPosition;
    }
}
