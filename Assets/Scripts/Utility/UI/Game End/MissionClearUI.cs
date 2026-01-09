using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionClearUI : MonoBehaviour
{
    [Header("Base data")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI endMessage;
    [SerializeField] private float fadeRate = 2.0f;
    [SerializeField] private List<string> levelClearMessages = new List<string>();
    [Header("Table Values")]
    [SerializeField] private TextMeshProUGUI missionTimeValue;
    [SerializeField] private TextMeshProUGUI missionTimeScore;
    [SerializeField] private TextMeshProUGUI enemyAlertsValue;
    [SerializeField] private TextMeshProUGUI enemyAlertsScore;
    [SerializeField] private TextMeshProUGUI hitsTakenValue;
    [SerializeField] private TextMeshProUGUI hitsTakenScore;
    [SerializeField] private TextMeshProUGUI accuracyValue;
    [SerializeField] private TextMeshProUGUI accuracyScore;
    [SerializeField] private TextMeshProUGUI psValue;
    [SerializeField] private TextMeshProUGUI psScore;
    [SerializeField] private TextMeshProUGUI noKillsValue;
    [SerializeField] private TextMeshProUGUI noKillsScore;
    [SerializeField] private TextMeshProUGUI totalPointsScore;


    public static event Action OnMissionClear;

    #region ENABLE-DISABLE Events
    private void OnEnable()
    {
        EndLevel.OnLevelEnd += StartLevelClear;
    }
    private void OnDisable()
    {
        EndLevel.OnLevelEnd -= StartLevelClear;
    }
    #endregion

    private void StartLevelClear()
    {
        canvasGroup.gameObject.SetActive(true);
        OnMissionClear?.Invoke();
        int index = UnityEngine.Random.Range(0, levelClearMessages.Count);
        string message = levelClearMessages[index];
        endMessage.text = message;
        ShowScores();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float alpha = 0f;
        while (alpha< 1.0f)
        {
            alpha += Time.deltaTime * fadeRate;
            canvasGroup.alpha = alpha;
            yield return null;
        }
    }

    private void ShowScores()
    {
        float timeInSeconds = StatTracker.Instance.GetPlaytime();
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        int psScoreValue = StatTracker.Instance.GetGhostModifier();
        int nkScoreValue = StatTracker.Instance.GetMercyModifier();
        int alertsValue = StatTracker.Instance.GetAlerts();
        int takenHitsValue = StatTracker.Instance.GetHitsTaken();
        missionTimeValue.text = string.Format("{0:00}:{1:00}",minutes,seconds);
        missionTimeScore.text = StatTracker.Instance.GetTimeScore().ToString();
        enemyAlertsValue.text = alertsValue == 0 ? "NO ALERTS" : alertsValue.ToString();
        enemyAlertsScore.text = StatTracker.Instance.GetEnemyAlertsScore().ToString();
        hitsTakenValue.text = takenHitsValue == 0 ? "NONE" : takenHitsValue.ToString();
        hitsTakenScore.text = StatTracker.Instance.GetHitsTakenScore().ToString();
        accuracyValue.text = StatTracker.Instance.GetAccuracyPercentage().ToString();
        accuracyScore.text = StatTracker.Instance.GetAccuracyScore().ToString();
        psValue.text = psScoreValue == 0 ? "NO" : "YES";
        psScore.text = psScoreValue.ToString();
        noKillsValue.text = nkScoreValue == 0 ? "NO" : "YES";
        noKillsScore.text = nkScoreValue.ToString();
        totalPointsScore.text = StatTracker.Instance.GetTotalScore().ToString();
    }

}
