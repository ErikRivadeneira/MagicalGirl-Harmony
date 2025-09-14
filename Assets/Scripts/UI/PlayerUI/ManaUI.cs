using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour
{
    [SerializeField] private PlayerMana playerMana;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private float slideRate = 1.0f;

    #region ENABLE-DISABLE EVENTS
    private void OnEnable()
    {
        playerMana.onManaChanged += ChangeManaSliderValue;
    }
    private void OnDisable()
    {
        playerMana.onManaChanged -= ChangeManaSliderValue;
    }
    #endregion

    private void ChangeManaSliderValue(float value)
    {
        value = value / 100;
        if (manaSlider.value < value)
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
        while (manaSlider.value > target)
        {
            manaSlider.value -= Time.deltaTime * slideRate;
            yield return null;
        }
        yield return null;
    }

    IEnumerator AnimateSliderIncrease(float target)
    {
        while (manaSlider.value < target)
        {
            manaSlider.value += Time.deltaTime * slideRate;
            yield return null;
        }
        yield return null;
    }
}
