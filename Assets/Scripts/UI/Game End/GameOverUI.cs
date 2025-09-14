using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI endMessage;
    [SerializeField] private float fadeRate = 2.0f;
    [SerializeField] private List<string> gameOverMessages = new List<string>();

    public static event Action OnGameOver;

    #region ENABLE-DISABLE Events
    private void OnEnable()
    {
        LifeModule.OnPlayerDead += StartGameOver;
    }
    private void OnDisable()
    {
        LifeModule.OnPlayerDead -= StartGameOver;
    }
    #endregion

    private void StartGameOver()
    {
        canvasGroup.gameObject.SetActive(true);
        OnGameOver?.Invoke();
        int index = UnityEngine.Random.Range(0, gameOverMessages.Count);
        string message = gameOverMessages[index];
        endMessage.text = message;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float alpha = 0f;
        while (alpha < 1.0f)
        {
            alpha += Time.deltaTime * fadeRate;
            canvasGroup.alpha = alpha;
            yield return null;
        }
    }
}
