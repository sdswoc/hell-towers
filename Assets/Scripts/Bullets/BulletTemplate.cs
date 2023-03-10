using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTemplate : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    [SerializeField] private float damage;
    [SerializeField] private bool canArmored;

    private void Update()
    {
        transform.position += transform.right * movementSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyTemplate>().takeDamage(damage, canArmored);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Tower"))
        {
            OfflineObjectPooler.Instance.destroyFromPool("type1", gameObject);
        }
    }
}
