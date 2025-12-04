using UnityEngine;

public class ArrowUI : MonoBehaviour
{
    // === Camera ===
    private Camera mainCamera;
    private CameraFollow cameraFollow;

    // === Arrows ===
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;

    void Awake()
    {
        mainCamera = Camera.main;
        cameraFollow = mainCamera.GetComponent<CameraFollow>();
    }

    void Update()
    {
        leftArrow.SetActive(mainCamera.transform.position.x >= cameraFollow.MaxXPos); 
        rightArrow.SetActive(mainCamera.transform.position.x < cameraFollow.MaxXPos); 
    }
}