using System;
using UnityEngine;

public class NoiseSystem : MonoBehaviour
{
    public static event Action<Vector2, float> OnNoiseHeard;
    

    public static void MakeNoise(Vector2 position, float volume = 1f)
    {
        OnNoiseHeard?.Invoke(position, volume);
    }
}
