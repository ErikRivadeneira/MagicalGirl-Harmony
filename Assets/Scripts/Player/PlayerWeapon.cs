using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private GameObject muzzleLight;
    [SerializeField] ParticleSystem muzzleParticles;
    [SerializeField] private PlayerMana playerMana;
    [SerializeField] private AudioSource gunSource;
    [SerializeField] private AudioClip emptyMag;
    private WeaponSO equippedWeapon;
    private float readyForNextShot;
    private float reloadTime = 3f;
    private bool isReloading = false;

    public event Action<Sprite, string, int, int> OnChangeWeapon;
    public event Action<int> OnShotOrReload;
    public event Action<float> OnStartReload;
    public event Action<string> OnAmmoFull;
    public event Action<string> OnAmmoEmpty;

    public static event Action OnShoot;


    public void Shoot(Rigidbody2D rb)
    {
        GameObject bullet = BulletPool.instance.GetPooledBullet();

        if(bullet != null)
        {
            if(Time.time > readyForNextShot && !isReloading && !equippedWeapon.MagIsEmpty())
            {
                readyForNextShot = Time.time + 1/equippedWeapon.GetWeaponRateOfFire();
                gunSource.PlayOneShot(equippedWeapon.GetShotClip());
                BulletInstatiationLogic(bullet);  
                ApplyRecoil(rb);
                ApplyMuzzleEffect(); 
                equippedWeapon.ReduceCurrentMag();
                OnShotOrReload?.Invoke(equippedWeapon.GetCurrentMag());
            }
            else if(equippedWeapon.MagIsEmpty())
            {
                if (Time.time > readyForNextShot)
                {
                    readyForNextShot = Time.time + 1 / equippedWeapon.GetWeaponRateOfFire();
                    gunSource.PlayOneShot(emptyMag);
                }
            }
        }
    }

    public void SetEquippedWeapon(WeaponSO weapon)
    {
        equippedWeapon = weapon;
        Sprite weaponSprite = equippedWeapon.GetWeaponSprite();
        string weaponName = equippedWeapon.GetWeaponName();
        int weaponMag = equippedWeapon.GetCurrentMag();
        reloadTime = weapon.GetReloadSpeed();
        OnChangeWeapon?.Invoke(weaponSprite, weaponName, weaponMag, equippedWeapon.GetWeaponMagazineCapacity());

    }

    public void IsNotReloading()
    {
        isReloading = false;
        equippedWeapon.ResetCurrentMag();
        OnShotOrReload?.Invoke(equippedWeapon.GetCurrentMag());
    }

    public void Reload()
    {
        float reloadValue = equippedWeapon.GetManaReloadCost();
        if (!equippedWeapon.MagIsFull() && !isReloading && (playerMana.GetCurrentMana() >= equippedWeapon.GetManaReloadCost()))
        {
            isReloading = true;
            gunSource.PlayOneShot(equippedWeapon.GetReloadClip());
            if (equippedWeapon.GetCurrentMag() != 0)
            {
                reloadValue += 5f;
            }
            playerMana.UseMana(reloadValue);
            Invoke(nameof(IsNotReloading), reloadTime);
            OnStartReload?.Invoke(reloadTime);
        }
        else
        {
            OnAmmoFull?.Invoke("FULL AMMO");
        }
    }

    Vector2 GetCurrentDirectionVector()
    {
        Vector2 direction = Vector2.zero;
        switch (playerAim.currentDirIndex)
        {
            case 0:
                direction = new Vector2(1,0);
                break;
            case 1:
                direction = new Vector2(1, 1);
                break;
            case 2:
                direction = new Vector2(0, 1);
                break;
            case 3:
                direction = new Vector2(-1, 1);
                break;
            case 4:
                direction = new Vector2(-1, 0);
                break;
            case 5:
                direction = new Vector2(-1, -1);
                break;
            case 6:
                direction = new Vector2(0, -1);
                break;
            case 7:
                direction = new Vector2(1, -1);
                break;
            default:
                direction = new Vector2(0, 1);
                break;
        }
        return direction;
    }

    Vector2 ApplySpread(Vector2 baseDirection, float spreadAngleDegrees)
    {
        float offset = UnityEngine.Random.Range(-spreadAngleDegrees * 0.5f, spreadAngleDegrees * 0.5f);

        // Rotate baseDirection by that offset
        float radians = offset * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new Vector2(
            baseDirection.x * cos - baseDirection.y * sin,
            baseDirection.x * sin + baseDirection.y * cos
        ).normalized;
    }

    public void ApplyRecoil(Rigidbody2D rb)
    {
        float gunKick = equippedWeapon.GetWeaponKick();
        Vector2 backwardsDir = GetCurrentDirectionVector() * -1;
        backwardsDir = new Vector2(backwardsDir.x * gunKick, backwardsDir.y * gunKick);
        rb.AddForce(backwardsDir, ForceMode2D.Impulse);
    }

    void ApplyMuzzleEffect()
    {
        muzzleParticles.Play();
        StartCoroutine(FlashMuzzleLight(muzzleParticles.main.duration));
    }

    IEnumerator FlashMuzzleLight(float duration)
    {
        Light2D lightComp = muzzleLight.GetComponent<Light2D>();
        muzzleLight.SetActive(true);

        float startIntensity = lightComp.intensity;
        float peakIntensity = startIntensity * 2f; // brighter burst
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Fade from peak  original intensity  0
            lightComp.intensity = Mathf.Lerp(peakIntensity, 0f, t);

            yield return null;
        }

        lightComp.intensity = startIntensity; // reset
        muzzleLight.SetActive(false);

    }

    void BulletInstatiationLogic(GameObject bullet)
    {
        float bulletSpeed = equippedWeapon.GetBulletSpeed();
        float bulletDamage = equippedWeapon.GetWeaponDamage();
        float bulletReach = equippedWeapon.GetWeaponRange();
        Vector2 direction = GetCurrentDirectionVector();
        Vector2 directionWithSpread = ApplySpread(direction, equippedWeapon.GetSpreadAngle());
        bullet.transform.position = shootPoint.position;
        bullet.GetComponent<BulletControler>().SetBulletData(bulletSpeed, bulletReach, bulletDamage, directionWithSpread, equippedWeapon.GetBulletSprite());
        bullet.SetActive(true);
    }
}
