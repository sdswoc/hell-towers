using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private string nameTag;
    GameManager gameManager;
    [SerializeField] private bool canArmored;
    [SerializeField] private float damage;

    OfflineObjectPooler pooler;

    private void Awake()
    {
        pooler = OfflineObjectPooler.Instance;
        gameManager = GameManager.gameManager;
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
            Debug.Log("enemyHit!");
            obj.GetComponent<EnemyTemplate>().takeDamage(damage, canArmored);
            Debug.Log("damage sent!");
            pooler.destroyFromPool(nameTag, gameObject);
        }
        else if (obj.CompareTag("Map"))
        {
            pooler.destroyFromPool(nameTag, gameObject);
        }
    }
}
