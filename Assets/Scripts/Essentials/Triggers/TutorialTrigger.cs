using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] bool inSceneText = true;
    [SerializeField] TextMeshPro sceneText;
    [SerializeField] float fadeRate = 2;
    [SerializeField] bool showText;
    [TextArea][SerializeField] string tutorialText;
    public static event Action<string> OnTutorialTriggered;

    private int fadeCounter = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            if (!inSceneText && showText)
            {
                OnTutorialTriggered?.Invoke(tutorialText);
            }
            if(showText && inSceneText)
            {
                sceneText.text = tutorialText;
                StartCoroutine(FadeToWhite());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && fadeCounter == 0)
        {
            fadeCounter++;
            StartCoroutine(FadeToTransparent());
        }
    }

    IEnumerator FadeToTransparent()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha = alpha - Time.deltaTime * fadeRate;
            sceneText.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        yield return null;
    }
    IEnumerator FadeToWhite()
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha = alpha + Time.deltaTime * fadeRate;
            sceneText.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        showText = false;
        yield return null;
    }
}
