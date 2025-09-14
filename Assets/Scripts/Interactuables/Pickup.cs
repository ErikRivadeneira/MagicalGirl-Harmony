using System;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractuable
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private AudioClip sfxClip;
    [SerializeField] private AudioSource source;
    [SerializeField] private EItemType pickupType;

    public static event Action OnMedkitPickup;
    public static event Action OnOrbPickup;

    public void Interact()
    {
        switch(pickupType)
        {
            case EItemType.medkitPickup:
                OnMedkitPickup?.Invoke();
                break;
            case EItemType.manaOrbPickup:
                OnOrbPickup?.Invoke();
                break;
        }
        source.PlayOneShot(sfxClip);
        sr.color = new Color(1f,1f,1f,0f);
        Destroy(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInteraction interaction = collision.gameObject.GetComponent<PlayerInteraction>();
        if (interaction != null)
        {
            interaction.SetInteractuable(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerInteraction interaction = collision.gameObject.GetComponent<PlayerInteraction>();
        if (interaction != null)
        {
            interaction.ClearInteractuable();
        }
    }
}
