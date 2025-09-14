using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeModule : MonoBehaviour
{
    [SerializeField] private float maxLife = 50f;
    [SerializeField] private float armor;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip hurtClip;

    [Header("Hit Stop")]
    [SerializeField] private float hitStopDuration = 0.07f; // ~4 frames at 60fps

    [Header("HitFlash")]
    [SerializeField] private List<SpriteRenderer> spriteRenderer = new List<SpriteRenderer>();
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    public Action<float> onPlayerLifeChanged;
    public static event Action OnPlayerDead;
    public static event Action OnEnemyDeath;

    private Color originalColor;
    private float currentLife;

    #region ENABLE-DISABLE Events
    private void OnEnable()
    {
        PlayerInventory.OnUseMedkit += HealWithMedkit;
    }
    private void OnDisable()
    {
        PlayerInventory.OnUseMedkit -= HealWithMedkit;
    }
    #endregion

    private void Awake()
    {
        currentLife = maxLife;
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer[0].color;
        }
    }


    public void Heal(float healAmount)
    {
        if(currentLife +  healAmount > maxLife)
        {
            currentLife = maxLife;
        }
        else
        {
            currentLife += healAmount;
        }

        if (this.tag.Equals("Player"))
        {
            onPlayerLifeChanged?.Invoke(currentLife);
        }
    }

    public void HealWithMedkit(int value)
    {
        Heal(20);
    }

    public void TakeDamage(float damage)
    {
        // Apply armor
        float effectiveDamage = Mathf.Max(damage - armor, 0);

        // Only apply if damage > 0
        if (effectiveDamage <= 0) return;

        currentLife -= effectiveDamage;
        if (currentLife >= 0)
        {
            StartCoroutine(DamageFlash());
            StartCoroutine(HitStop(hitStopDuration));
        }

        if (this.tag.Equals("Player"))
        {
            onPlayerLifeChanged?.Invoke(currentLife);
        }

        source.PlayOneShot(hurtClip);
        if (currentLife <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitStop(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        if (Time.timeScale == 0f)
            Time.timeScale = 1f;
    }

    private IEnumerator DamageFlash()
    {
        ChangeAllSpriteRenderers(damageColor);
        yield return new WaitForSecondsRealtime(flashDuration);
        ChangeAllSpriteRenderers(originalColor);
        yield return new WaitForSecondsRealtime(flashDuration);
        ChangeAllSpriteRenderers(damageColor);
        yield return new WaitForSecondsRealtime(flashDuration);
        ChangeAllSpriteRenderers(originalColor);
    }

    public void Die()
    {
        if (this.tag.Equals("Player"))
        {
            OnPlayerDead?.Invoke();
        }
        else
        {
            OnEnemyDeath?.Invoke();
            Destroy(this.gameObject);
        }
    }

    public void ChangeAllSpriteRenderers(Color color)
    {
        foreach(SpriteRenderer renderer in spriteRenderer)
        {
            renderer.color = color;
        }
    }
}
