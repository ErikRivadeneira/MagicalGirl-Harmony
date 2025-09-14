using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManagerSO", menuName = "Scriptable Objects/InputManagerSO")]
public class InputManagerSO : ScriptableObject
{
    InputSystem_Actions inputMapper;
    public event Action<Vector2> OnMove;
    public event Action<InputAction.CallbackContext> OnLook;
    public event Action OnSprint;
    public event Action OnCrouch;
    public event Action OnAttack;
    public event Action OnPreviousWeapon;
    public event Action OnNextWeapon;
    public event Action OnManaItem;
    public event Action OnHealItem;
    public event Action OnReload;
    public event Action OnInteract;

    private void OnEnable()
    {
        inputMapper = new InputSystem_Actions();
        inputMapper.Player.Enable();
        // Movement Input
        inputMapper.Player.Move.started += Move;
        inputMapper.Player.Move.performed += Move;
        inputMapper.Player.Move.canceled += Move;
        inputMapper.Player.Sprint.started += Sprint;
        inputMapper.Player.Sprint.canceled += Sprint;
        inputMapper.Player.Crouch.started += Crouch;
        inputMapper.Player.Crouch.canceled += Crouch;
        // Aiming/Shooting
        inputMapper.Player.Look.performed += Look;
        inputMapper.Player.Attack.started += Attack;
        inputMapper.Player.Attack.canceled += Attack;
        inputMapper.Player.Reload.started += Reload;
        // Inventory and Quick Items
        inputMapper.Player.PreviousWeapon.started += PreviousWeapon;
        inputMapper.Player.NextWeapon.started += NextWeapon;
        inputMapper.Player.UseOrb.started += ManaItem;
        inputMapper.Player.UseMedkit.started += HealItem;
        // Interaction
        inputMapper.Player.Interact.started += Interact;
    }

    private void OnDisable()
    {
        inputMapper.Player.Disable();
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        OnMove?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void Sprint(InputAction.CallbackContext ctx)
    {
        OnSprint?.Invoke();
    }

    private void Crouch(InputAction.CallbackContext ctx)
    {
        OnCrouch?.Invoke();
    }

    private void Look(InputAction.CallbackContext ctx)
    {
        OnLook?.Invoke(ctx);
    }

    private void Attack(InputAction.CallbackContext ctx)
    {
        OnAttack?.Invoke();
    }

    private void Reload(InputAction.CallbackContext ctx)
    {
        OnReload?.Invoke();
    }

    private void PreviousWeapon(InputAction.CallbackContext ctx)
    {
        OnPreviousWeapon?.Invoke();
    }

    private void NextWeapon(InputAction.CallbackContext ctx)
    {
        OnNextWeapon?.Invoke();
    }

    private void HealItem(InputAction.CallbackContext ctx)
    {
        OnHealItem?.Invoke();
    }

    private void ManaItem(InputAction.CallbackContext ctx)
    {
        OnManaItem?.Invoke();
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        OnInteract?.Invoke();
    }
}
