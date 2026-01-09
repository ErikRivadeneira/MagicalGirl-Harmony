using System;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] private float maxMana = 100f;

    public Action<float> onManaChanged;

    private float currentMana;

    #region ENABLE-DISABLE Events
    private void OnEnable()
    {
        PlayerInventory.OnUseManaOrb += RecoverWithOrb;
    }
    private void OnDisable()
    {
        PlayerInventory.OnUseManaOrb -= RecoverWithOrb;
    }
    #endregion

    private void Awake()
    {
        currentMana = maxMana;
    }

    public void RecoverMana(float healAmount)
    {
        if (currentMana + healAmount > maxMana)
        {
            currentMana = maxMana;
        }
        else
        {
            currentMana += healAmount;
        }

        onManaChanged?.Invoke(currentMana);
    }

    void RecoverWithOrb(int value)
    {
        RecoverMana(10);
    }

    public void UseMana(float usedAmount)
    {
        if (currentMana - usedAmount < 0)
        {
            currentMana = 0;
        }
        else
        {
            currentMana -= usedAmount;
        }

        onManaChanged?.Invoke(currentMana);
    }

    public float GetCurrentMana()
    {
        return currentMana;
    }
}
