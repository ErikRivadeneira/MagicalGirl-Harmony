using System;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
    [Header("Level Optimal Time")]
    [SerializeField] private float targetTime;
    [SerializeField] private float maxTime;
    [SerializeField] private int timeBonus = 5000;
    [SerializeField] private int noShotBonus = 1000;
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
        if(shots > 0)
        {
            double percentage = Math.Round(((double)hits / shots) * 100, 2);
            return $"{percentage}% ({hits}/{shots})";
        }
        else
        {
            return "NO SHOTS";
        }
    }

    public int GetAccuracyScore()
    {
        if (shots != 0)
        {
            int minimumScoreMod = Mathf.FloorToInt(noShotBonus * 0.1f);
            int scoreModMinusMinimum = noShotBonus - minimumScoreMod;
            float score = ((hits / (float)shots) * scoreModMinusMinimum) + minimumScoreMod;
            return Mathf.FloorToInt(score);
        }
        else
        {
            return noShotBonus;
        }
    }
    
    public int GetHitsTakenScore()
    {
        if (hitsTaken > 0)
        {
            return -hitsTaken * 100;
        }
        else
        {
            return 500;
        }
    }

    public int GetEnemyAlertsScore()
    {
        if(timesDiscovered > 0)
        {
            return -timesDiscovered * 100;
        }
        else
        {
            return 500;
        }
        
    }

    public int GetGhostModifier()
    {
        int gScore = timesDiscovered == 0 ? 5000 : 0;
        return gScore;
    }

    public int GetMercyModifier()
    {
        return kills == 0 ? 2500 : 0;
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

    public void ResetVariables()
    {
        timesDiscovered = 0;
        hitsTaken = 0;
        hits = 0;
        shots = 0;
        playTime = 0;
        kills = 0;
    }
}
