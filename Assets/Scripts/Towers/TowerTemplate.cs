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


    //variables only for this script
    [SerializeField] private NetworkObject netobj;
     

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
    }

    //public override void OnNetworkSpawn()
    //{
    //    transform.position = Testing.mapGrid.GetCellCentre(transform.position);
    //    netobj.transform.position = transform.position;

    //    base.OnNetworkSpawn();
    //}

    private void Start()
    {
        BulletPooler = OfflineObjectPooler.Instance;

        transform.position = Testing.mapGrid.GetCellCentre(transform.position);
        netobj.transform.position = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemies.Add(collision.gameObject);
            Debug.Log("Enemy Added!");
        }else if (collision.CompareTag("ThePlayer"))
        {
            players.Add(collision.gameObject);
            Debug.Log("Players Added!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemies.Remove(collision.gameObject);
            Debug.Log("Enemey Removed!");
        }else if (collision.CompareTag("ThePlayer"))
        {
            enemies.Remove(collision.gameObject);
            Debug.Log("Player Removed!");
        }
    }
}

public interface ITowerTemplate
{
    public void Shoot();
    public void Rotate();
}