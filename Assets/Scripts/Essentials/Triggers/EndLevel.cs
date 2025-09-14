using System;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public static event Action OnLevelEnd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnLevelEnd?.Invoke();
        }
    }
}
