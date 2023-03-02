using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CoinPickup : NetworkBehaviour
{
    private bool collected = false;
    private Transform playerPos;
    private float t = 0f;
    [SerializeField] private float collectSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bob"))
        {
            playerPos = collision.transform;
            collected = true;
        }
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
            //trigger increase score here
            Destroy(gameObject);
        }
    }
}
