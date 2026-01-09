using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    [SerializeField] private Image gunMask;
    [SerializeField] private TextMeshProUGUI ammoCount;
    [SerializeField] private TextMeshProUGUI gunName;
    [SerializeField] private TextMeshProUGUI gunNotifierText;
    [SerializeField] private PlayerWeapon playerWeapon;
    [SerializeField] private Slider ammoSlider;
    [SerializeField] private CanvasGroup ammoFullNotifier;
    [SerializeField] private float fadeRate = 1f;

    private int gunMaxCapacity;
    private float currentReloadTime;

    #region ENABLE/DISABLE Events
    private void OnEnable()
    {
        playerWeapon.OnChangeWeapon += SetGunData;
        playerWeapon.OnShotOrReload += SetAmmoCount;
        playerWeapon.OnStartReload += StartReloadAnimation;
        playerWeapon.OnAmmoFull += StartFadingAmmoNotifier;
        playerWeapon.OnAmmoEmpty += StartFadingAmmoNotifier;
    }
    private void OnDisable()
    {
        playerWeapon.OnChangeWeapon -= SetGunData;
        playerWeapon.OnShotOrReload -= SetAmmoCount;
        playerWeapon.OnStartReload -= StartReloadAnimation;
        playerWeapon.OnAmmoFull -= StartFadingAmmoNotifier;
        playerWeapon.OnAmmoEmpty -= StartFadingAmmoNotifier;
    }
    #endregion

    void SetGunData(Sprite gunImage, string gunName, int ammo, int mag)
    {
        gunMask.sprite = gunImage;
        this.gunName.text = gunName;
        ammoSlider.value = ammo / mag;
        gunMaxCapacity = mag;
        SetAmmoCount(ammo);
    }

    void SetAmmoCount(int ammo)
    {
        ammoCount.text = $"x{ammo}";
        float sliderPercentage = (float)ammo/(float)gunMaxCapacity;
        ammoSlider.value = sliderPercentage;
    }

    void StartReloadAnimation(float reloadTime)
    {
        ammoSlider.value = 0f;
        StartCoroutine(ReloadFillAnimation(reloadTime));
    }

    void StartFadingAmmoNotifier(string message)
    {
        gunNotifierText.text = message;
        ammoFullNotifier.alpha = 1f;
        StartCoroutine(AmmoFullNotifierFade());
    }

    IEnumerator ReloadFillAnimation(float reloadTime)
    {
        while(currentReloadTime < reloadTime)
        {
            currentReloadTime  = currentReloadTime + Time.deltaTime;
            
            ammoSlider.value = ammoSlider.value + Time.deltaTime/reloadTime;
            yield return null;
        }
        currentReloadTime = 0f;
        yield return null;
    }

    IEnumerator AmmoFullNotifierFade()
    {
        while(ammoFullNotifier.alpha > 0f)
        {
            
            ammoFullNotifier.alpha = ammoFullNotifier.alpha - Time.deltaTime * fadeRate;
            yield return null ;
        }
    }
}
