using UnityEngine;

public class OrthographicCameraScaler : MonoBehaviour
{
    [SerializeField] private float targetWidth = 10f;
    [SerializeField] private float targetHeight = 5f;

    private Camera orthoCamera;

    private void Awake()
    {
        orthoCamera = Camera.main;
        UpdateOrthoSize();
    }

    private void UpdateOrthoSize()
    {
        if (orthoCamera == null)
            return;
        
        float targetAspect = targetWidth / targetHeight;
        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect > targetAspect)
        {
            orthoCamera.orthographicSize = targetHeight / 2f;
        }
        else
        {
            orthoCamera.orthographicSize = targetWidth / (2f * currentAspect);
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        UpdateOrthoSize();
    }
}
