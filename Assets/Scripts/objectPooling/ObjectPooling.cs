using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPooling : Singleton<ObjectPooling>
{
    public Dictionary<int, Queue<object>> poolingObjects = new Dictionary<int, Queue<object>>();
    public GameObject bulletPrefab;
    public Transform startPosition;
    [HideInInspector]public UnityEvent onObjectReachDestination;

    private void Awake()
    { 
        CreatePoolingObjectsForBullets(bulletPrefab, 30);
    }
    private void CreatePoolingObjectsForBullets(GameObject prefab, int countOfObjects)
    {
        int indexOfPrefab = prefab.GetInstanceID();
        if (!poolingObjects.ContainsKey(indexOfPrefab))
        {
            poolingObjects.Add(indexOfPrefab, new Queue<object>());
        }

        for (int i = 0; i < countOfObjects; i++)
        {
            GameObject g = Instantiate(prefab);
            g.transform.SetParent(startPosition);
            g.SetActive(false);
            Bullet bullet = g.GetComponent<Bullet>();
            bullet.indexOfPrefab = indexOfPrefab;
            poolingObjects[indexOfPrefab].Enqueue(g.GetComponent<Bullet>());
        }
    }
    public void CreatePoolingObjectsForEnemys(GameObject prefab, Transform enemysParent, int countOfObjects)
    {
        int indexOfPrefab = prefab.GetInstanceID();
        if (!poolingObjects.ContainsKey(indexOfPrefab))
        {
            poolingObjects.Add(indexOfPrefab, new Queue<object>());
        }

        for (int i = 0; i < countOfObjects; i++)
        {
            GameObject g = Instantiate(prefab);
            g.transform.SetParent(enemysParent);
         
            Enemy enemy = g.GetComponent<Enemy>();
            enemy.indexOfPrefab = indexOfPrefab;
            poolingObjects[indexOfPrefab].Enqueue(g.GetComponent<Enemy>());
        }
    }
    public Enemy GetEnemyFromStorage(GameObject prefab)
    {
        int indexOfPrefab = prefab.GetInstanceID();
        if (!poolingObjects.ContainsKey(indexOfPrefab) || poolingObjects[indexOfPrefab].Count == 0)
        {
            CreatePoolingObjectsForBullets(prefab, 10);
        }
        return (Enemy)poolingObjects[indexOfPrefab].Dequeue();
    }
    public Bullet GetObjectFromStorage(GameObject prefab)
    {
        int indexOfPrefab = prefab.GetInstanceID();
        if (!poolingObjects.ContainsKey(indexOfPrefab) || poolingObjects[indexOfPrefab].Count==0)
        {
            CreatePoolingObjectsForBullets(prefab, 10);
        }
        return (Bullet)poolingObjects[indexOfPrefab].Dequeue();
    }
}
