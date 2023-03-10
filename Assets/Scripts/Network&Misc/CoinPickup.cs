using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CoinPickup : NetworkBehaviour, IgetObjectType
{
    private bool collected = false;
    private Transform playerPos;
    private float t = 0f;
    [SerializeField] private float collectSpeed;
    WaveSpawnner wave;
    public string nameTag = "coin";
    float time;
    [SerializeField] float max_time = 4f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ThePlayer"))
        {
            playerPos = collision.transform;
            collected = true;
        }
    }

    private void Start()
    {
        wave = WaveSpawnner.Instance;
    }

    private void OnEnable()
    {
        time = 0;
    }

    private void Update()
    {
        if (collected)
        {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, playerPos.position.x, t), Mathf.Lerp(transform.position.y, playerPos.position.y, t), 0);
            t += collectSpeed * Time.deltaTime;
        }
        if(t >= 0.9f)
        {
            //increase the currency here!!
            wave.Destroy(gameObject.GetComponent<NetworkObject>());
        }

        time += Time.deltaTime;

        if(time > max_time)
        {
            wave.Destroy(gameObject.GetComponent<NetworkObject>());
        }
    }

    public string isEquals()
    {
        return nameTag;
    }
}
