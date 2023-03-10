using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private string nameTag;

    OfflineObjectPooler pooler;

    private void Awake()
    {
        pooler = OfflineObjectPooler.Instance;
    }

    private void Update()
    {
        transform.position += transform.right * movementSpeed * Time.deltaTime;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Tower"))
        {
            pooler.destroyFromPool(nameTag, gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("ThePlayer"))
        {
            Debug.Log("PlayerHit!!");
            pooler.destroyFromPool(nameTag, gameObject);
        }
        else if (obj.CompareTag("Enemy"))
        {
            Debug.Log("EnemyHit!");
            pooler.destroyFromPool(nameTag, gameObject);
        }
        else if (obj.CompareTag("Map"))
        {
            pooler.destroyFromPool(nameTag, gameObject);
        }
    }
}
