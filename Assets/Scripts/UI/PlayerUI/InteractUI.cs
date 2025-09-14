using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private GameObject indicator;

    #region ENABLE-DISABLE Input
    private void OnEnable()
    {
        PlayerInteraction.OnEnterInteract += ShowIndicator;
        PlayerInteraction.OnLeaveInteract += HideIndicator;
    }
    private void OnDisable()
    {
        PlayerInteraction.OnEnterInteract += ShowIndicator;
        PlayerInteraction.OnLeaveInteract += HideIndicator;
    }
    #endregion

    void ShowIndicator()
    {
        indicator.SetActive(true);
    }

    void HideIndicator()
    {
        indicator.SetActive(false);
    }
}
