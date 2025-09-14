using UnityEngine;
using static Unity.VisualScripting.Member;

public class SwitchActivatedDoors : MonoBehaviour, IInteractuable
{
    [SerializeField] private GameObject DoorA;
    [SerializeField] private GameObject DoorB;
    [SerializeField] private AudioClip sfxClip;
    [SerializeField] private AudioSource source;
    [SerializeField] private bool horizontalShift = true;
    [SerializeField] private float shiftValue = 1;

    private bool canInteract = true;
    PlayerInteraction interaction;

    public void Interact()
    {
        float xA = 0;
        float xB = 0;
        float yA = 0;
        float yB = 0;
        if (horizontalShift)
        {
            xA = DoorA.transform.position.x - shiftValue;
            xB = DoorB.transform.position.x + shiftValue;
            yA = DoorA.transform.position.y;
            yB = DoorB.transform.position.y;
        }
        else
        {
            xA = DoorA.transform.position.x;
            xB = DoorB.transform.position.x;
            yA = DoorA.transform.position.y + shiftValue;
            yB = DoorB.transform.position.y - shiftValue;
        }
        DoorA.transform.position = new Vector3(xA, yA, 0);
        DoorB.transform.position = new Vector3(xB, yB, 0);
        canInteract = false;
        source.PlayOneShot(sfxClip);
        interaction.ClearInteractuable();
        interaction = null;
        GetComponent<BoxCollider2D>().enabled = false;  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        interaction = collision.gameObject.GetComponent<PlayerInteraction>();
        if (interaction != null && canInteract)
        {
            interaction.SetInteractuable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interaction != null)
        {
            interaction.ClearInteractuable();
        }
    }
}
