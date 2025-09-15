using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrap : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    [SerializeField] List<GameObject> doors = new List<GameObject>();
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip sfxClip;
    public enum Direction { LeftToRight, RightToLeft, TopToBottom, BottomToTop }

    [SerializeField] Direction allowedDirection;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            PlayerStealth stealth = collision.gameObject.GetComponent<PlayerStealth>();
            Vector2 worldDirection = collision.gameObject.transform.position - this.transform.position;
            if (!stealth.GetWalking() && worldDirection.magnitude > 0)
            {
                CheckDirection(collision);
            }
        }
    }

    void ActivateTrap()
    {
        source.PlayOneShot(sfxClip);
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(true);
        }
        foreach (GameObject door in doors)
        {
            door.SetActive(true);
        }
    }

    void CheckDirection(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        Vector2 playerPos = other.transform.position;
        Vector2 triggerPos = transform.position;

        switch (allowedDirection)
        {
            case Direction.LeftToRight:
                if (playerPos.x > triggerPos.x)
                { 
                    this.gameObject.SetActive(false);
                    ActivateTrap();
                }
                break;

            case Direction.RightToLeft:
                if (playerPos.x < triggerPos.x)
                {

                    this.gameObject.SetActive(false);
                    ActivateTrap();
                }
                break;

            case Direction.TopToBottom:
                if (playerPos.y < triggerPos.y)
                {
                    ActivateTrap();
                    this.gameObject.SetActive(false);
                }
                    break;

            case Direction.BottomToTop:
                if (playerPos.y > triggerPos.y)
                {
                    this.gameObject.SetActive(false);
                    ActivateTrap();
                }
                break;
        }
    }
}
