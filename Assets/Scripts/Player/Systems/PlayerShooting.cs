using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private InputManagerSO inputManager;
    [SerializeField] private PlayerWeapon playerWeapon;
    private bool isShootKeyPressed;
    private Rigidbody2D rb;



    #region ENABLE-DISABLE Input
    private void OnEnable()
    {
        inputManager.OnAttack += SetIsShootKeyPressed;
        inputManager.OnReload += Reload;
        
    }
    private void OnDisable()
    {
        inputManager.OnAttack -= SetIsShootKeyPressed;
        inputManager.OnReload -= Reload;
    }
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void HandleShooting()
    {
        if (isShootKeyPressed)
        {
            playerWeapon.Shoot(rb);
        }
    }

    public void SetIsShootKeyPressed()
    {
        isShootKeyPressed = !isShootKeyPressed;
    }

    public void Reload()
    {
        playerWeapon.Reload();
    }
}
