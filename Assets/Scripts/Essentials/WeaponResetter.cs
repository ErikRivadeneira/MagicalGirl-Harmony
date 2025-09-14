using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WeaponResetter : MonoBehaviour
{
    [SerializeField] private List<WeaponSO> weapons = new List<WeaponSO>();

    private void Awake()
    {
        foreach( WeaponSO weapon in weapons)
        {
            weapon.ResetCurrentMag();
        }
    }
}
