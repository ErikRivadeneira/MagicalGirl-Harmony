using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletControler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float pixelsPerUnit = 32;
    [SerializeField] private SpriteRenderer sr;
    private float bulletSpeed = 10;
    private float bulletDamage = 10;
    private float weaponRange = 10;
    private float ranDistance = 0;
    private Vector2 direction;
    private Vector3 lastPosition;

    public static event Action OnDamagePlayer;
    public static event Action OnDamageEnemy;

    private void OnEnable()
    {
        sr.enabled = true;
    }
    private void FixedUpdate()
    {
        Vector2 targetPos = rb.position + direction * bulletSpeed * Time.deltaTime;

        targetPos.x = Mathf.Round(targetPos.x * pixelsPerUnit) / pixelsPerUnit;
        targetPos.y = Mathf.Round(targetPos.y * pixelsPerUnit) / pixelsPerUnit;

        rb.MovePosition(targetPos);

        float distanceThisFrame = Vector3.Distance(lastPosition, this.transform.position);
        ranDistance += distanceThisFrame;
        lastPosition = transform.position;

        if (ranDistance >= weaponRange)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        LifeModule objectLife = collision.gameObject.GetComponent<LifeModule>();
        if (objectLife != null)
        {
            objectLife.TakeDamage(bulletDamage);
            if (collision.gameObject.CompareTag("Player"))
            {
                OnDamagePlayer?.Invoke();
            }
            else if (collision.gameObject.CompareTag("Enemy"))
            {
                OnDamageEnemy?.Invoke();
            }
        }
        this.gameObject.SetActive(false);
    }

    public void SetBulletData(float projectileSpeed, float projectileRange, float projectileDamage, Vector2 direction,Sprite sprite)
    {
        ranDistance = 0;
        bulletSpeed = projectileSpeed;
        weaponRange = projectileRange;
        bulletDamage = projectileDamage;
        lastPosition = transform.position;
        this.direction = direction; 
        sr.sprite = sprite;
        
        float angle = Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle);  
    }
}
