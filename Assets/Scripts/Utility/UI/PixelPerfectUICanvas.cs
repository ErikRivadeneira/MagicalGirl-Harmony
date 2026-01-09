using UnityEngine;

public class PixelPerfectUICanvas : MonoBehaviour
{
    [Header("Assign your Pixel Perfect Camera")]
    public Camera pixelCam;

    [Header("Assign a root RectTransform child for UI scaling")]
    public RectTransform uiRoot;

    private int lastWidth;
    private int lastHeight;

    void Awake()
    {
        if (!pixelCam)
            pixelCam = Camera.main;

        if (!uiRoot)
            Debug.LogError("UI Root not assigned! Assign a child RectTransform containing all UI elements.");
    }

    void Update()
    {
        // Only update when screen size changes
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            AdjustUIScale();
            lastWidth = Screen.width;
            lastHeight = Screen.height;
        }
    }

    private void AdjustUIScale()
    {
        if (!uiRoot) return;

        float screenRatio = (float)Screen.width / Screen.height;
        float targetRatio = pixelCam.aspect;

        float scaleX = 1f;
        float scaleY = 1f;

        if (screenRatio >= targetRatio)
        {
            scaleX = targetRatio / screenRatio;
        }
        else
        {
            scaleY = screenRatio / targetRatio;
        }

        uiRoot.localScale = new Vector3(scaleX, scaleY, uiRoot.localScale.z);
    }
}
