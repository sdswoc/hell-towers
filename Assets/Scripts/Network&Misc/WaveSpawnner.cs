using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;
using TMPro;

public class WaveSpawnner : NetworkBehaviour
{
    #region Singleton
    public static WaveSpawnner Instance;
    private void Awake()
    {
        Instance = this;
        gameOverScreen.SetActive(false);
    }
    #endregion

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

    [Header("Prefab-here!")]
    [SerializeField] private List<GameObject> prefabs_here;
    private Queue<GameObject> queue_enemies_spawned = new Queue<GameObject>();
    #endregion

    #region time_setting
    [SerializeField] float time_between_waves;
    bool inWave;
    bool previousWaveEnded;
    float time;
    #endregion

    //keep-track;
    int wave_count = 0;

    GameObject prefab_instance;
    NetworkObject network_prefab_instance;
    GameObject object_to_destory;

    private void Start()
    {
        time = time_between_waves;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    private void Update()
    {

        if (queue_enemies_spawned.Count == 0)
        {
            previousWaveEnded = true;
        }
        else previousWaveEnded = false;


        if (!inWave)
        {
            WaveCounter.text = "UPCOMING WAVE : " + (wave_count + 1);
        }
        else WaveCounter.text = "";

        if (!IsServer) return;

        if (!inWave && previousWaveEnded)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                spawnWave(wave_count);
                incrementWaveCountClientRPC();
                switchInWaveClientRPC();
            }
        }
        else
        {
            time = time_between_waves;
        }
    }

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
                spawnEnemy(enemy.tag, transform.position, transform.rotation);
                int wait = (int)(enemy.rate * 1000);
                await Task.Delay(wait);
            }
        }
        switchInWaveClientRPC();
    }

    public void spawnEnemy(string tag, Vector3 position, Quaternion rotation)
    {
        int i = 0;

        if(tag == "enemy1")
        {
            i = 0;
        }
        else if( tag == "enemy1armor")
        {
            i = 1;
        }
        else if (tag == "enemy2")
        {
            i = 2;
        }
        else if (tag == "enemy2armor")
        {
            i = 3;
        }
        else if (tag == "enemy3")
        {
            i = 4;
        }
        else if (tag == "enemy3armor")
        {
            i = 5;
        }

        prefab_instance = Instantiate(prefabs_here[i]);
        network_prefab_instance = prefab_instance.GetComponent<NetworkObject>();
        prefab_instance.transform.position = position;
        prefab_instance.transform.rotation = rotation;
        queue_enemies_spawned.Enqueue(prefab_instance);
        network_prefab_instance.Spawn();

    }

    public void removeEnemy(GameObject gameObject)
    {
        queue_enemies_spawned.Dequeue();
        if (IsServer) gameObject.GetComponent<NetworkObject>().Despawn();
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