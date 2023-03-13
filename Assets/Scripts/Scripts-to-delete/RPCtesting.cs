using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RPCTesting : NetworkBehaviour
//INetworkPrefabInstanceHandler
{


    [SerializeField] private List<GameObject> enemy;
    private NetworkObject SpawnedNetworkEnemy;
    private GameObject enemyInstance;


    //basically-make-the-class-for-the-enemies-to-spawn
    [System.Serializable]
    public class wave
    {
        public List<enemy_spawn> enemies;
    }

    [System.Serializable]
    public class enemy_spawn
    {
        public GameObject prefab;
        public float rate;
        public int count;
    }
    public List<wave> waves;



    public override void OnNetworkSpawn()
    {
        if (!IsServer || enemy == null)
        {
            return;
        }
        InvokeRepeating("Generate", 2f, 2f);

        //NetworkManager.PrefabHandler.AddHandler()
    }

    void Generate()
    {
        enemyInstance = Instantiate(enemy[0]);
        enemyInstance.transform.position = transform.position;
        enemyInstance.transform.rotation = transform.rotation;

        SpawnedNetworkEnemy = enemyInstance.GetComponent<NetworkObject>();
        SpawnedNetworkEnemy.Spawn();
        enemyInstance.SetActive(false); //this code is running only on server side isliye on client side enemies abhi bhi show ho rhe hain;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q)) enemyInstance.SetActive(true);
    }

    #region Singleton
    public static RPCTesting Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Dictionary-Work
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public bool c;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionaryObject;
    public Dictionary<string, Queue<NetworkObject>> poolDictionaryNetwork;

    private void Start()
    {
        if (!IsServer)
        {
            return;
        }


        poolDictionaryObject = new Dictionary<string, Queue<GameObject>>();
        poolDictionaryNetwork = new Dictionary<string, Queue<NetworkObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            Queue<NetworkObject> networkPool = new Queue<NetworkObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                NetworkObject net = obj.GetComponent<NetworkObject>();
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                networkPool.Enqueue(net);
            }
            poolDictionaryObject.Add(pool.tag, objectPool);
            poolDictionaryNetwork.Add(pool.tag, networkPool);

        }
    }
    #endregion


    private void expandDictionary(string tag)
    {



    }




}

