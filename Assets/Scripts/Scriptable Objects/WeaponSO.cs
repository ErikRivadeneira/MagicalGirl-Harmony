using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private string weaponName;
    [SerializeField] private string WeaponTitle;
    [SerializeField] private string weaponType;
    [SerializeField] private float weaponDamage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float spreadAngle;
    [SerializeField] private EFireType fireType;
    [SerializeField] private float weaponRateOfFire;
    [SerializeField] private float weaponRange;
    [SerializeField] private int weaponMagazineCapacity;
    [SerializeField] private float weaponKick;
    [SerializeField] private float manaReloadCost;
    [SerializeField] private Sprite weaponSprite;
    [SerializeField] private Sprite bulletSprite;
    [SerializeField] private int currentMagCapacity;
    [SerializeField] private AudioClip shotClip;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private float gunVolume;

    public string GetWeaponName()
    {
        return weaponName;
    }

    public string GetWeaponTitle()
    {
        return WeaponTitle;
    }

    public string GetWeaponType()
    {
        return weaponType;
    }

    public float GetWeaponDamage()
    {
        return weaponDamage;
    }

    public float GetBulletSpeed()
    {
        return bulletSpeed;
    }

    public float GetSpreadAngle()
    {
        return spreadAngle;
    }

    public EFireType GetFireType()
    {
        return fireType;
    }

    public float GetWeaponRateOfFire()
    {
        return weaponRateOfFire;
    }

    public float GetWeaponRange()
    {
        return weaponRange;
    }

    public int GetWeaponMagazineCapacity()
    {
        return weaponMagazineCapacity;
    }

    public float GetWeaponKick()
    {
        return weaponKick;
    }

    public float GetManaReloadCost()
    {
        return manaReloadCost;
    }

    public Sprite GetWeaponSprite()
    {
        return weaponSprite;
    }

    public Sprite GetBulletSprite()
    {
        return bulletSprite;
    }

    public int GetCurrentMag()
    {
        return currentMagCapacity;
    }

    public void ResetCurrentMag()
    {
        currentMagCapacity = weaponMagazineCapacity;
    }

    public void ReduceCurrentMag()
    {
        currentMagCapacity--;
    }

    public bool MagIsEmpty()
    {
        return currentMagCapacity == 0;
    }

    public bool MagIsFull()
    {
        return currentMagCapacity == weaponMagazineCapacity;
    }

    public AudioClip GetShotClip()
    {
        return shotClip;
    }

    public AudioClip GetReloadClip()
    {
        return reloadSound;
    }

    public float GetReloadSpeed()
    {
        return reloadSpeed;
    }

    public float GetGunVolume()
    {
        return gunVolume;
    }
}
