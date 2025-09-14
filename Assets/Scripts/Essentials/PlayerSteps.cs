using UnityEngine;

public class PlayerSteps : MonoBehaviour
{
    [SerializeField] private AudioClip footstep;
    [SerializeField] private AudioSource source;

    public void SetFootstepClip(AudioClip clip)
    {
       footstep = clip;
    }

    public void PlayFootstep()
    {
        source.PlayOneShot(footstep);
    }
}
