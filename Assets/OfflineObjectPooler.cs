using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public int expansion_size;
    }


    #region Singleton
    public static OfflineObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach(pool pool in pools)
        {
            Queue < GameObject > objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);

        }
    }

    public GameObject spawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("given tag" + tag + "doesn't exist");
            return null;
        }

        if (poolDictionary[tag].Count != 0)
        {
            return returnGameObject(tag, position, rotation);

        }
        else
        {
            foreach(pool pool in pools)
            {
                if(tag == pool.tag)
                {
                    for(int i = 0; i < pool.expansion_size; i++)
                    {
                        GameObject obj1 = Instantiate(pool.prefab);
                        obj1.SetActive(false);
                        poolDictionary[tag].Enqueue(obj1);
                    }
                    break;
                }
            }

            return returnGameObject(tag, position, rotation);

        }
    }

    private GameObject returnGameObject(string tag, Vector3 position, Quaternion rotation)
    {
        GameObject obj = poolDictionary[tag].Dequeue();

        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        //IOfflinePooledObject offlinePooledObject = obj.GetComponent<IOfflinePooledObject>();

        //if (poolDictionary != null) offlinePooledObject.SetDamage(damage);

        return obj;
    }


    public void destroyFromPool(string tag, GameObject prefab)
    {
        poolDictionary[tag].Enqueue(prefab);

        prefab.SetActive(false);
    }
}

public interface IOfflinePooledObject
{
    void SetDamage(float n);
}
