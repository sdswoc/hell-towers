using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;
using TMPro;

using UnityEditor;


public class WaveSpawnner : NetworkBehaviour, INetworkPrefabInstanceHandler
{
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] TMP_Text WaveCounter;

    #region wave_info
    [System.Serializable]
    public class wave
    {
        public List<enemy> enemies;
    }

    [System.Serializable]
    public class enemy
    {
        public int count;
        public float rate;
        public string tag;
    }

    public List<wave> waves;
    #endregion

    #region time_setting
    [SerializeField] float time_between_waves;
    bool inWave;
    float time;
    #endregion

    //keep-track;
    int wave_count = 0;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public Dictionary<string, Queue<NetworkObject>> poolDictionaryNetwork;

    GameObject prefab_instance;
    NetworkObject network_prefab_instance;
    GameObject object_to_destory;

    //temp-prefab

    #region pooling
    [System.Serializable]
    public class pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public int expansion_size;
    }

    public List<pool> pools;
    #endregion

    #region Singleton
    public static WaveSpawnner Instance;
    private void Awake()
    {
        Instance = this;
        gameOverScreen.SetActive(false);
    }
    #endregion

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        poolDictionaryNetwork = new Dictionary<string, Queue<NetworkObject>>();
        
        foreach(pool pool in pools)
        {
            NetworkManager.PrefabHandler.AddHandler(pool.prefab, this);

            Queue<GameObject> objectPool = new Queue<GameObject>();
            Queue<NetworkObject> networkObjects = new Queue<NetworkObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                NetworkObject netObj = obj.GetComponent<NetworkObject>();
                networkObjects.Enqueue(netObj);
                objectPool.Enqueue(obj);
                if (IsServer) netObj.Spawn();
                obj.SetActive(false);
            }

            poolDictionary.Add(pool.tag, objectPool);
            poolDictionaryNetwork.Add(pool.tag, networkObjects);
        }


        time = time_between_waves;
    }

    #region Interface-functions
    public NetworkObject Instantiate(ulong ownerClientID, Vector3 position, Quaternion rotation)
    {
        prefab_instance.SetActive(true);
        network_prefab_instance = prefab_instance.GetComponent<NetworkObject>();
        prefab_instance.transform.position = position;
        prefab_instance.transform.rotation = rotation;
        return network_prefab_instance;
    }

    public void Destroy(NetworkObject networkObject)
    {
        object_to_destory = networkObject.gameObject;
        DestroyObjectClientRPC();
    }

#endregion

   async void spawnWave(int i)
    {
        if(i == waves.Count)
        {
            expandWaveClientRPC(i);
        }

        foreach(enemy enemy in waves[i].enemies)
        {
            for(int j = 0; j < enemy.count; j++)
            {
                spawnObjectFromPool(enemy.tag, transform.position, transform.rotation);
                int wait = (int)(enemy.rate * 1000);
                await Task.Delay(wait);
            }
        }
        switchInWaveClientRPC();
    }

    [ClientRpc]
    private void expandWaveClientRPC(int i)
    {
        List<enemy> new_enemies = waves[i - 1].enemies;
        for (int h = 0; h < new_enemies.Count; h++)
        {
            new_enemies[h].count *= 2;
        }
        wave new_wave = new wave();
        new_wave.enemies = new_enemies;
        waves.Add(new_wave);
    }

    public void spawnObjectFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        SpawnObjectClientRPC(tag, position, rotation);
    }


    void expandDictionary(string nameing)
    {
        foreach(pool pool in pools)
        {
            if (pool.tag == nameing)
            {
                for (int i = 0; i < pool.expansion_size; i++)
                {
                    GameObject obj = obj = Instantiate(pool.prefab);
                    NetworkObject netObj = obj.GetComponent<NetworkObject>();
                    if (IsServer) netObj.Spawn();
                    poolDictionaryNetwork[pool.tag].Enqueue(netObj);
                    poolDictionary[pool.tag].Enqueue(obj);
                    obj.SetActive(false);
                }

            }
        }
    }

    private void Update()
    {
        if (!inWave)
        {
            WaveCounter.text = "UPCOMING WAVE : " + (wave_count + 1);
        }
        else WaveCounter.text = "";

        if (!IsServer) return;


        if (!inWave)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                incrementWaveCountClientRPC();
                switchInWaveClientRPC();
                spawnWave(wave_count);
                

            }
        }
        else
        {
            time = time_between_waves; 
        }
    }



    [ClientRpc]
    void SpawnObjectClientRPC(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("given tag: " + tag + " doesn't exist");
            return;
        }

        //expanding-the-dictionary
        if (poolDictionary[tag].Count == 1)
        {
            foreach (pool pool in pools)
            {
                if (pool.prefab.GetComponent<IgetObjectType>().isEquals() == tag)
                {
                    expandDictionary(pool.tag);
                    break;
                    //spawnObjectFromPool(tag, position, rotation);
                    //return;
                }
            }
        }

        prefab_instance = poolDictionary[tag].Dequeue();
        prefab_instance.SetActive(true);
        prefab_instance.transform.position = transform.position;
        prefab_instance.transform.rotation = rotation;
        network_prefab_instance = poolDictionaryNetwork[tag].Dequeue();

    }
    [ClientRpc]
    void DestroyObjectClientRPC()
    {
        foreach (pool pool in pools)
        {
            if (pool.prefab.GetComponent<IgetObjectType>().isEquals() == pool.tag)
            {
                poolDictionary[pool.tag].Enqueue(object_to_destory);
                Debug.Log("Length after enqueue: " + poolDictionary[pool.tag].Count);
                poolDictionaryNetwork[pool.tag].Enqueue(object_to_destory.GetComponent<NetworkObject>());
                object_to_destory.SetActive(false);
                return;
            }
        }
    }

    [ClientRpc]
    void incrementWaveCountClientRPC()
    {
        wave_count++;
    }
    [ClientRpc]
    void switchInWaveClientRPC()
    {
        inWave = !inWave;
    }


}