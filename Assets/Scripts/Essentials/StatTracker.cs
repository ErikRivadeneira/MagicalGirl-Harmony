using System;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
    [Header("Level Optimal Time")]
    [SerializeField] private float targetTime;
    [SerializeField] private float maxTime;
    [SerializeField] private int timeBonus = 5000;

    public static StatTracker Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private bool trackTime = true;
    private float playTime;
    private float totalPlayTime;
    private int timesDiscovered;
    private int hitsTaken;
    private int shots;
    private int hits;
    private int kills;

    #region ENABLE-DISABLE events
    private void OnEnable()
    {
        BulletControler.OnDamagePlayer += TakeAHit;
        BulletControler.OnDamageEnemy += AddHits;
        PlayerWeapon.OnShoot += AddShots;
        LifeModule.OnEnemyDeath += IncreaseKills;
        EnemyAI.OnPlayerDiscovered += AddDiscovery;
    }
    private void OnDisable()
    {
        BulletControler.OnDamagePlayer -= TakeAHit;
        BulletControler.OnDamageEnemy -= AddHits;
        PlayerWeapon.OnShoot -= AddShots;
        LifeModule.OnEnemyDeath += IncreaseKills;
        EnemyAI.OnPlayerDiscovered -= AddDiscovery;
    }
    #endregion


    private void Update()
    {
        if(trackTime)
        {
            playTime += Time.unscaledDeltaTime;
        }
    }

    private void AddShots()
    {
        shots++;
    }

    private void AddHits()
    {
        hits++;
    }

    private void AddDiscovery()
    {
        timesDiscovered++;
    }

    private void TakeAHit()
    {
        hitsTaken++;
    }

    private void IncreaseKills()
    {
        kills++;
    }

    private void ResetZoneTimer()
    {
        playTime = 0;
    }

    public string GetAccuracyPercentage()
    {
        double percentage = (hits / shots) * 100;
        return $"{percentage}% ({hits}/{shots})";
    }

    public int GetAccuracyScore()
    {
        return (hitsTaken / shots) * 1000;
    }
    
    public int GetHitsTakenScore()
    {
        return -hitsTaken * 100;
    }

    public int GetEnemyAlertsScore()
    {
        return -timesDiscovered * 100;
    }

    public int GetGhostModifier()
    {
        return timesDiscovered > 0 ? 5000 : 0;
    }

    public int GetMercyModifier()
    {
        return kills > 0 ? 2500 : 0;
    }

    public float GetPlaytime()
    {
        return playTime;
    }
    
    public int GetTotalScore()
    {
        int score = 0;
        int accuracyScore = GetAccuracyScore();
        int timeScore = GetTimeScore();
        int hitsTakenScore = GetHitsTakenScore();
        int alertsScore = GetEnemyAlertsScore();
        int ghostModifier = GetGhostModifier();
        int mercyModifier = GetMercyModifier();
        score = accuracyScore + timeScore + hitsTakenScore + alertsScore + ghostModifier + mercyModifier;
        return score;
    }

    public int GetTimeScore()
    {
        int timeScore = 0;
        if(playTime <= targetTime)
        {
           timeScore = timeBonus;
        }else if (playTime >= maxTime)
        {
            timeScore = 0;
        }
        else
        {
            float t = (playTime - targetTime) / (maxTime - targetTime);
            timeScore = (int)Mathf.Lerp(timeBonus,0,t);
        }
        return timeScore;
    }

    public void AddTotalPlaytime()
    {
        totalPlayTime += playTime;
    }

    public void StopOrStartTracking()
    {
        trackTime = !trackTime;
    }

    public int GetAlerts()
    {
        return timesDiscovered;
    }

    public int GetHitsTaken()
    {
        return hitsTaken;
    }
}
