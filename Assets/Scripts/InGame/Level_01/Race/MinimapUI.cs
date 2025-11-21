using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class MinimapUI : MonoBehaviour
{
    [SerializeField] private SplineContainer minimapSpline;
    [SerializeField] private RectTransform minimapIcon;
    private float currentRaceProgress = 0f;

    void Update()
    {
        // Calculate progress relative to the current lap
        float currentTime = GameManagerLevel1.instance.CurrentGameTime;
        currentRaceProgress = (currentTime % GameManagerLevel1.instance.LapDuration) / GameManagerLevel1.instance.LapDuration;
        currentRaceProgress = Mathf.Clamp01(currentRaceProgress);

        // Evaluate spline and convert it to screen point
        float3 newPosition = minimapSpline.EvaluatePosition(currentRaceProgress);
        Vector3 worldPos = new(newPosition.x, newPosition.y, newPosition.z);
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, worldPos);

        // Convert the screen point to a local point within the minimap icon parent
        RectTransform parentRect = minimapIcon.parent as RectTransform;
        if (parentRect == null)
        {
            Debug.LogWarning("El minimapIcon no tiene un RectTransform parent válido.");
            return;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPoint, null, out Vector2 localPoint))
        {
            minimapIcon.anchoredPosition = localPoint;
        }
    }
}