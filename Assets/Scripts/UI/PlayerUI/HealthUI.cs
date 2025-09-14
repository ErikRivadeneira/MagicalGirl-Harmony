using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private LifeModule playerLife;
    [SerializeField] private float slideRate = 1.0f;

    #region ENABLE-DISABLE Events
    private void OnEnable()
    {
        playerLife.onPlayerLifeChanged += ChangeSliderValue;
    }
    private void OnDisable()
    {
        playerLife.onPlayerLifeChanged -= ChangeSliderValue;
    }
    #endregion

    private void ChangeSliderValue(float value)
    {
        value = value / 100;
        if (healthSlider.value < value)
        {
            StartCoroutine(AnimateSliderIncrease(value));
        }
        else
        {
            StartCoroutine(AnimateSliderDecrease(value));
        }
    }

    IEnumerator AnimateSliderDecrease(float target)
    {
        while (healthSlider.value > target)
        {
            healthSlider.value -= Time.deltaTime * slideRate;
            yield return null;
        }
        yield return null;
    }

    IEnumerator AnimateSliderIncrease(float target)
    {
        while (healthSlider.value < target)
        {
            healthSlider.value += Time.deltaTime * slideRate;
            yield return null;
        }
        yield return null;
    }
}
