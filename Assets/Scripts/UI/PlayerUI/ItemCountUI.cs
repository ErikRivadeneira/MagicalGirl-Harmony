using TMPro;
using UnityEngine;

public class ItemCountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI medkitCounter;
    [SerializeField] private TextMeshProUGUI manaOrbCounter;

    #region ENABLE-DISABLE Events
    private void OnEnable()
    {
        PlayerInventory.OnUseManaOrb += UpdateManaOrbCounter;
        PlayerInventory.OnUseMedkit += UpdateMedkitCounter;
        PlayerInventory.OnPickMedkit += UpdateMedkitCounter;
        PlayerInventory.OnPickOrb += UpdateManaOrbCounter;
    }
    private void OnDisable()
    {
        PlayerInventory.OnUseManaOrb -= UpdateManaOrbCounter;
        PlayerInventory.OnUseMedkit -= UpdateMedkitCounter;
        PlayerInventory.OnPickMedkit -= UpdateMedkitCounter;
        PlayerInventory.OnPickOrb -= UpdateManaOrbCounter;
    }
    #endregion
    
    void UpdateMedkitCounter(int count)
    {
        medkitCounter.text = $"x{count}";
    }
    void UpdateManaOrbCounter(int count)
    {
        manaOrbCounter.text = $"x{count}";
    }
}
