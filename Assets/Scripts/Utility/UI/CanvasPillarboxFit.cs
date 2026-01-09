using UnityEngine;

public class CanvasPillarboxFit : MonoBehaviour
{
    [SerializeField] private Camera pixelCam;

    private int lastWidth;
    private int lastHeight;

    private void Update()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            UpdateCanvasScale();
            lastHeight = Screen.height;
            lastWidth = Screen.width;
        }
    }
    
    void UpdateCanvasScale()
    {
        RectTransform canvasRect = GetComponent<RectTransform>();

        float screenRatio = (float)Screen.width / Screen.height;
        float targetRatio = pixelCam.aspect;

        if (screenRatio >= targetRatio)
        {
            float scale = targetRatio / screenRatio;
            canvasRect.localScale = new Vector3(scale, 1f, 1f);
        }
        else
        {
            float scale = screenRatio / targetRatio;
            canvasRect.localScale = new Vector3(1f, scale, 1f);
        }
    }
}
