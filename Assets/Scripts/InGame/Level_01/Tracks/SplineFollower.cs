using UnityEngine;
using UnityEngine.Splines;

public class SplineFollower : MonoBehaviour
{
    // === Scale ===
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;

    // === Movement ===
    private SplineContainer spline;
    private float speed;
    private float distanceTraveled;
    private float totalLength;
    private bool isActive;

    public void InitializeFollower(SplineContainer spline, float speed)
    {
        this.spline = spline;
        this.speed = Mathf.Max(0.0001f, speed);
        distanceTraveled = 0;
        totalLength = spline.CalculateLength();
        isActive = true;
    }

    void OnEnable()
    {
        if (spline == null) isActive = false;
    }

    void OnDisable()
    {
        spline = null;
        speed = 0;
        distanceTraveled = 0;
        totalLength = 0;
        isActive = false;
    }

    void Update()
    {
        if (!isActive || spline == null) return;

        FollowSpline();
    }

    private void FollowSpline()
    {
        distanceTraveled += Time.deltaTime * speed;
        float progressTime = Mathf.Clamp01(distanceTraveled / totalLength);

        float scaleFactor = Mathf.Lerp(minScale, maxScale, scaleCurve.Evaluate(progressTime));
        transform.localScale = Vector3.one * scaleFactor;

        if (progressTime >= 1f)
        {
            FinishFollowUp();
            return;
        }

        // Update position
        Vector3 newPosition = spline.EvaluatePosition(progressTime);
        transform.position = newPosition;
    }

    private void FinishFollowUp()
    {
        gameObject.SetActive(false);
    }
}