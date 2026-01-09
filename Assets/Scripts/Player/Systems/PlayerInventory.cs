using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private InputManagerSO inputManager;
    [SerializeField] private PlayerWeapon equippedWeapon;
    [SerializeField] private List<WeaponSO> weaponsInInventory = new List<WeaponSO>();
    [SerializeField] private int medkits = 3;
    [SerializeField] private int manaOrbs = 1;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip useItemClip;
    [SerializeField] private AudioClip changeGunClip;
    [SerializeField] private AudioClip notPossibleClip;

    public static event Action<int> OnUseMedkit;
    public static event Action<int> OnUseManaOrb;
    public static event Action<int> OnPickOrb;
    public static event Action<int> OnPickMedkit;

    private int weaponIndex = 0;
    //private int quickItemIndex = 0;

    #region ENABLE-DISABLE Input
    private void OnEnable()
    {
        inputManager.OnNextWeapon += EquipNextWeapon;
        inputManager.OnPreviousWeapon += EquipPreviousWeapon;
        Pickup.OnMedkitPickup += AddMedkit;
        Pickup.OnOrbPickup += AddOrb;
        inputManager.OnHealItem += UseMedkit;
        inputManager.OnManaItem += UseOrb;
    }
    private void OnDisable()
    {
        inputManager.OnNextWeapon -= EquipNextWeapon;
        inputManager.OnPreviousWeapon -= EquipPreviousWeapon;
        Pickup.OnMedkitPickup -= AddMedkit;
        Pickup.OnOrbPickup -= AddOrb;
        inputManager.OnHealItem -= UseMedkit;
        inputManager.OnManaItem -= UseOrb;
        /*inputManager.OnPreviousItem -= EquipPreviousItem;
        inputManager.OnNextItem -= EquipNextItem;*/
    }
    #endregion

    private void Start()
    {
        equippedWeapon.SetEquippedWeapon(weaponsInInventory[0]);
        OnPickMedkit?.Invoke(medkits);
        OnPickOrb?.Invoke(manaOrbs);
        weaponIndex = 0;
    }

    void EquipNextWeapon()
    {
        if (equippedWeapon != null)
        {
            weaponIndex++;
            if(weaponIndex == weaponsInInventory.Count)
            {
                weaponIndex = 0;
            }
            equippedWeapon.SetEquippedWeapon(weaponsInInventory[weaponIndex]);
            source.PlayOneShot(changeGunClip);
        }
    }

    void EquipPreviousWeapon()
    {
        if (equippedWeapon != null)
        {
            weaponIndex--;
            if (weaponIndex < 0)
            {
                weaponIndex = weaponsInInventory.Count - 1;
            }
            equippedWeapon.SetEquippedWeapon(weaponsInInventory[weaponIndex]);
            source.PlayOneShot(changeGunClip);
        }
    }
    void AddOrb()
    {
        manaOrbs++;
        OnPickOrb?.Invoke(manaOrbs);
    }
    void AddMedkit()
    {
        medkits++;
        OnPickMedkit?.Invoke(medkits);

    }

    void UseMedkit()
    {
        if(medkits > 0)
        {
            medkits--;
            OnUseMedkit?.Invoke(medkits);
            source.PlayOneShot(useItemClip);
        }
        else
        {
            source.PlayOneShot(notPossibleClip);
        }
        
        
    }
    void UseOrb()
    {
        if(manaOrbs > 0)
        {
            manaOrbs--;
            OnUseManaOrb?.Invoke(manaOrbs);
            source.PlayOneShot(useItemClip);
        }
        else
        {
            source.PlayOneShot(notPossibleClip);
        }

    }
    // Item System Possibility

   /* void EquipPreviousItem()
    {
        if (equippedWeapon != null)
        {
            weaponIndex--;
            if (weaponIndex < 0)
            {
                weaponIndex = weaponsInInventory.Count - 1;
            }
            equippedWeapon.SetEquippedWeapon(weaponsInInventory[weaponIndex]);
        }
    }

    void EquipNextItem()
    {
        if (equippedWeapon != null)
        {
            weaponIndex++;
            if (weaponIndex == weaponsInInventory.Count)
            {
                weaponIndex = 0;
            }
            equippedWeapon.SetEquippedWeapon(weaponsInInventory[weaponIndex]);
        }
    }*/

}
