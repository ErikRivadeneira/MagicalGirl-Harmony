using System.Collections;
using UnityEngine;

public class SceneChangeAnimation : MonoBehaviour
{
    [SerializeField] CanvasGroup panel;
    public float fadeRate = 1.0f;

    #region ENABLE-DISABLE Events
    private void OnEnable()
    {
        UIFlowManager.OnLevelChange += BeginFadeIn;
    }
    private void OnDisable()
    {
        UIFlowManager.OnLevelChange -= BeginFadeIn;
    }
    #endregion

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeIn()
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeRate;
            panel.alpha = alpha;
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeRate;
            panel.alpha = alpha;
            yield return null;
        }
    }

    public void BeginFadeIn()
    {
        StartCoroutine(FadeIn());
    }
}
