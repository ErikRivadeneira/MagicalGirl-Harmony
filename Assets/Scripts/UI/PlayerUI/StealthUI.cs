using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StealthUI : MonoBehaviour
{
    [SerializeField] private PlayerStealth playerStealth;
    [SerializeField] private Image stealthIndicator;
    [SerializeField] private float colorChangeRate = 2.0f;

    #region DISABLE-ENABLE Events
    private void OnEnable()
    {
        playerStealth.onVisibilityChanged += ChangeVisibilityIndicator;
    }

    private void OnDisable()
    {
        playerStealth.onVisibilityChanged -= ChangeVisibilityIndicator;
    }
    #endregion

    private void ChangeVisibilityIndicator(float value)
    {
        value = value > 1.0f ? 1.0f : value;
        float colorValue = stealthIndicator.color.r;
        if(colorValue < value)
        {
            StartCoroutine(AnimateIndicatorToWhite(colorValue, value));
        }
        else
        {
            StartCoroutine(AnimateIndicatorToBlack(colorValue, value));
        }
    }

    IEnumerator AnimateIndicatorToBlack(float colorValue, float target)
    {
        while(colorValue > target)
        {
            colorValue -= Time.deltaTime * colorChangeRate;
            stealthIndicator.color = new Color(colorValue,colorValue,colorValue);
            yield return null;
        }
        yield return null;
    }
    IEnumerator AnimateIndicatorToWhite(float colorValue, float target)
    {
        while (colorValue < target)
        {
            colorValue += Time.deltaTime * colorChangeRate;
            stealthIndicator.color = new Color(colorValue, colorValue, colorValue);
            yield return null;
        }
        yield return null;
    }
}
