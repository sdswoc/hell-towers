using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TowerTemplate : NetworkBehaviour
{
    protected List<GameObject> enemies;
    protected List<GameObject> players;


    //towerstats
    [SerializeField] protected float range;
    [SerializeField] protected float numberOfBulletsPerShot;
    [SerializeField] protected int numberOfShots;
    [SerializeField] protected float timeBetweenShooting;
    [SerializeField] protected float timeBetweenShots;
    [SerializeField] protected float damage;
    [SerializeField] protected float towerCost;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform attackPoint;

    //variables globally may be needed
    private CircleCollider2D area;


    //for-pooling
    protected OfflineObjectPooler BulletPooler;


    //in-built functions

    private void Awake()
    {
        area = GetComponent<CircleCollider2D>();
        area.radius = range;
        area.isTrigger = true;
        enemies = new List<GameObject>();
        players = new List<GameObject>();
    }

    private void Start()
    {
        BulletPooler = OfflineObjectPooler.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemies.Add(collision.gameObject);
        }else if (collision.CompareTag("ThePlayer"))
        {
            players.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemies.Remove(collision.gameObject);
        }else if (collision.CompareTag("ThePlayer"))
        {
            players.Remove(collision.gameObject);
        }
    }
}

public interface ITowerTemplate
{
    public void Shoot();
    public void Rotate();
}