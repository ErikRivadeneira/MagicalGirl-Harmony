using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;

    private List<GameObject> pooledNormalBullets = new List<GameObject>();
    private int amountToPool = 50;

    [SerializeField] private GameObject normalBulletPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(normalBulletPrefab);
            obj.SetActive(false);
            pooledNormalBullets.Add(obj);
        }
    }

    public GameObject GetPooledBullet()
    {
        for(int i = 0; i < pooledNormalBullets.Count; i++)
        {
            if (!pooledNormalBullets[i].activeInHierarchy)
            {
                return pooledNormalBullets[i];
            }
        }
        return null;
    }
}
