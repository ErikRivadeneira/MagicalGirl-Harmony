using System.Runtime.CompilerServices;
using UnityEngine;

public class StealthZone : MonoBehaviour
{
    [SerializeField] private float visibilityReduction = 0.5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStealth playerStealth = collision.GetComponent<PlayerStealth>();
        if(playerStealth != null)
        {
            playerStealth.EnterStealthZone(visibilityReduction);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerStealth playerStealth = collision.GetComponent<PlayerStealth>();
        if (playerStealth != null)
        {
            playerStealth.ExitStealthZone(0);
        }
    }
}
