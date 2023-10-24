using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Instance;

    public Dictionary<int, Queue<Bullet>> poolingObjects = new Dictionary<int, Queue<Bullet>>();
    public GameObject bulletPrefab;
    public Transform startPosition;
    public UnityEvent onObjectReachDestination;

    private void Awake()
    {
        Instance = this;
        CreatePoolingObjects(bulletPrefab, startPosition, 30);
    }
    private void CreatePoolingObjects(GameObject prefab, Transform transform, int countOfObjects)
    {
        int indexOfPrefab = prefab.GetInstanceID();
        if (!poolingObjects.ContainsKey(indexOfPrefab))
        {
            poolingObjects.Add(indexOfPrefab, new Queue<Bullet>());
        }

        for (int i = 0; i < countOfObjects; i++)
        {
            GameObject g = Instantiate(prefab);
            g.transform.SetParent(startPosition);
            g.SetActive(false);
            Bullet ball = g.GetComponent<Bullet>();
            ball.indexOfPrefab = indexOfPrefab;
            poolingObjects[indexOfPrefab].Enqueue(g.GetComponent<Bullet>());
        }
    }

    public Bullet GetObjectFromStorage(GameObject prefab)
    {
        int indexOfPrefab = prefab.GetInstanceID();
        if (!poolingObjects.ContainsKey(indexOfPrefab) || poolingObjects[indexOfPrefab].Count==0)
        {
            CreatePoolingObjects(prefab, startPosition, 10);
        }
        return poolingObjects[indexOfPrefab].Dequeue();
    }
}
