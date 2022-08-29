using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public ObjectToSpawnType Type;
        public GameObject Prefab;
        public int Amount;
    }

    [SerializeField] private List<Pool> pools = new List<Pool>();

    private readonly Dictionary<Pool, Queue<GameObject>> poolDictionary = new Dictionary<Pool, Queue<GameObject>>();

    #region Singleton
    public static ObjectPool Instance { get { return instance; } }
    private static ObjectPool instance;

    private void Awake()

    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion


    private void Start()
    {
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.Amount; i++)
            {
                GameObject obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                obj.transform.SetParent(transform);
            }

            poolDictionary.Add(pool, objectPool);
        }
    }

    public GameObject GetFromPool(ObjectToSpawnType type, Vector3 position, Quaternion rotation, Vector3 size)
    {
        if (poolDictionary[pools.Find(x => x.Type == type)] == null)
        {
            Debug.LogError($"Can't find pool of type: <b>{nameof(type)}</b>");
            return null;
        }

        if (poolDictionary[pools.Find(x => x.Type == type)].Count == 0)
        {   
            Debug.LogError($"Pool object of type: <b>{nameof(type)}</b> is too small");
            return null;
        }

        GameObject obj = poolDictionary[pools.Find(x => x.Type == type)].Dequeue();

        obj.SetActive(true);
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.transform.localScale = size;

        return obj;
    }

    public void PutBackToPool(GameObject obj, ObjectToSpawnType type)
    {
        poolDictionary[pools.Find(x => x.Type == type)].Enqueue(obj);
        obj.SetActive(false);
    }
}
