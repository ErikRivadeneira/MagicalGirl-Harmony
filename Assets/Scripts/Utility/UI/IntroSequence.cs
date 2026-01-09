using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class IntroSequence : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI introText1;
    [SerializeField] private TextMeshProUGUI introText2;
    [SerializeField] private TextMeshProUGUI introText3;
    [SerializeField] private TextMeshProUGUI introText4;
    [SerializeField] private CanvasGroup introPanel;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip introClip;
    [SerializeField] private AudioClip ambienceClip;
    [SerializeField] private float fadeRate = 1f;

    public static event Action OnIntroFinished;

    private void Start()
    {
        StartCoroutine(IntroRoutine());
    }

    IEnumerator FadeIn(TextMeshProUGUI text)
    {
        float alpha = 0f;
        while(alpha< 1f)
        {
            alpha += Time.deltaTime * fadeRate;
            text.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
    }
    IEnumerator FadeAmbienceIn()
    {
        float alpha = 0f;
        while (alpha < 0.35f)
        {
            alpha += Time.deltaTime * fadeRate;
            source.volume = alpha;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * (fadeRate/2);
            source.volume = alpha;
            introPanel.alpha = alpha;
            yield return null;
        }
    }

    IEnumerator IntroRoutine()
    {
        source.PlayOneShot(introClip);
        yield return StartCoroutine(FadeIn(introText1));
        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(FadeIn(introText2));
        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(FadeIn(introText3));
        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(FadeIn(introText4));
        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(FadeOut());
        source.Stop();
        source.loop = true;
        source.clip = ambienceClip;
        source.Play();
        //yield return StartCoroutine(FadeAmbienceIn());
        source.volume = 0.85f;
        introPanel.gameObject.SetActive(false);
        OnIntroFinished?.Invoke();
    }
}
