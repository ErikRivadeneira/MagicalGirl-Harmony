using UnityEngine;

public class FootstepChanger : MonoBehaviour
{
    [SerializeField] private AudioClip footstepsIn;
    [SerializeField] private AudioClip footstepsOut;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement movement = collision.gameObject.GetComponent<PlayerMovement>();
        if(movement != null)
        {
            movement.SetFootstep(footstepsIn);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement movement = collision.gameObject.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.SetFootstep(footstepsOut);
        }
    }
}
