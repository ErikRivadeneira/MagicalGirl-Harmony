using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MainMenuLighting : MonoBehaviour
{
    [SerializeField] private Light2D lightInScene;
    [Header("IntensityFlicker")]
    [SerializeField] float minIntensity = 0.8f;
    [SerializeField] float maxIntensity = 1.5f;
    [SerializeField] float flickerSpeed = 3f;

    [Header("ColorFlicker")]
    [SerializeField] Color basecolor = new Color(1f, 0.55f, 0.2f); // warm orange
    [SerializeField] float colorVariation = 0.05f;

    private void Update()
    {
        if (lightInScene == null) return;

        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);

        lightInScene.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);

        float r = basecolor.r + Random.Range(-colorVariation, colorVariation);
        float g = basecolor.g + Random.Range(-colorVariation, colorVariation);
        float b = basecolor.b + Random.Range(-colorVariation, colorVariation);
        lightInScene.color = new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b));
    }
}
