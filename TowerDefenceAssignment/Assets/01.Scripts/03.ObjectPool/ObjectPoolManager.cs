using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int size;
        [HideInInspector] public Queue<GameObject> queue;
    }

    public List<Pool> pools;
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePools()
    {
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            pool.queue = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                pool.queue.Enqueue(obj);
            }
            poolDictionary.Add(pool.prefab, pool.queue);
        }
    }

    public GameObject SpawnFromPool(GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            GameObject newObject = Instantiate(prefab, transform);
            newObject.SetActive(true);
            return newObject;
        }

        if (poolDictionary[prefab].Count == 0)
        {
            GameObject newObject = Instantiate(prefab, transform);
            newObject.SetActive(true);
            return newObject;
        }

        GameObject objectToSpawn = poolDictionary[prefab].Dequeue();
        objectToSpawn.SetActive(true);
        return objectToSpawn;
    }

    public void ReturnToPool(GameObject obj, GameObject originalPrefab)
    {
        if (!poolDictionary.ContainsKey(originalPrefab))
        {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(this.transform);
        poolDictionary[originalPrefab].Enqueue(obj);
    }
}
