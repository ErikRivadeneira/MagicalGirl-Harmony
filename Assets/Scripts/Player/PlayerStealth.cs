using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UIElements;

public class PlayerStealth : MonoBehaviour
{
    [SerializeField] private InputManagerSO inputManager;
    public float visibility { get; private set; } = 1f;
    private bool isCrouching = false;
    private bool isRunning = false;
    private bool inStealthZone = false;
    private float stealthZoneModifier;

    public Action<float> onVisibilityChanged;
    float previousVisibility;

    #region ENABLE-DISABLE Input
    private void OnEnable()
    {
        inputManager.OnCrouch += CrouchStealthCheck;
        inputManager.OnSprint += RunningStealthCheck;
    }
    private void OnDisable()
    {
        inputManager.OnCrouch -= CrouchStealthCheck;
        inputManager.OnSprint -= RunningStealthCheck;
    }
    #endregion

    private void CrouchStealthCheck()
    {
        isCrouching = !isCrouching;
        isRunning = false;
        UpdateVisibility(0);
    }

    private void RunningStealthCheck()
    {
        isRunning = !isRunning;
        isCrouching = false;
        UpdateVisibility(0);
    }
    private void Start()
    {
        previousVisibility = visibility;
    }

    private void UpdateVisibility(float stealthZoneModf)
    {
        float totalModifier = 1f;
        if (isCrouching)
        {
            totalModifier -= 0.5f;
        }
        else if (isRunning)
        {
            totalModifier += 0.25f;
        }
        if(inStealthZone)
        {
            totalModifier -= stealthZoneModf;
        }
        visibility = totalModifier < 0 ? 0f : totalModifier;
        onVisibilityChanged?.Invoke(visibility);
    }

    public void EnterStealthZone(float modifier)
    {
        inStealthZone = true;
        UpdateVisibility(modifier);
    }
    public void ExitStealthZone(float modifier)
    {
        inStealthZone = false;
        UpdateVisibility(0);
    }

    public bool GetWalking()
    {
        return isCrouching;
    }
}
