using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private InputManagerSO inputManager;
    private bool canInteract = false;

    public static event Action OnEnterInteract;
    public static event Action OnLeaveInteract;

    #region ENABLE-DISABLE EVENTS
    private void OnEnable()
    {
        inputManager.OnInteract += Interact;
    }
    private void OnDisable()
    {
        inputManager.OnInteract -= Interact;
    }
    #endregion

    private IInteractuable currentInteractuable;

    public void SetInteractuable(IInteractuable interactuable)
    {
        currentInteractuable = interactuable;
        canInteract = true;
        OnEnterInteract?.Invoke();
    }

    public void ClearInteractuable()
    {
        currentInteractuable = null;
        canInteract = false;
        OnLeaveInteract?.Invoke();
    }

    void Interact()
    {
        if (currentInteractuable != null && canInteract)
        {
            currentInteractuable.Interact();
        }
        ClearInteractuable();
    }
}
