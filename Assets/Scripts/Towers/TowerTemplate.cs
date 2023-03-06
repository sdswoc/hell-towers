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

    //variablesfortheshooting
    //protected ParticleSystem system;
    //[SerializeField] private Material bulletMaterial;
    //[SerializeField] private Sprite bulletSprite;
    //[SerializeField] private LayerMask layer;

    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform attackPoint;


    //variables only for this script
    [SerializeField] private NetworkObject netobj;
     

    //variables globally may be needed
    private CircleCollider2D area;



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
    
    //my functions
    //protected void Summon(float speed, float lifetime, float no_of_cols, float spread)
    //{
    //    float angle = 360 / no_of_cols;
    //    for(int i = 0; i < no_of_cols; i++)
    //    {
    //        Material particleMaterial = bulletMaterial;
    //        var go = new GameObject("Particle System");
    //        go.transform.Rotate(angle * i, 90, 0); //rotate so the system emits upwards
    //        //recheck this part of the code
    //        go.transform.parent = this.transform;
    //        go.transform.position = this.transform.position;
    //        system = go.AddComponent<ParticleSystem>();
    //        go.GetComponent<ParticleSystemRenderer>().material = particleMaterial;
    //        var mainModule = system.main;
    //        mainModule.startSize = 0.5f; //size of all bullets can be collectively changed from here
    //        mainModule.startSpeed = speed;
    //        mainModule.maxParticles = 100000;
    //        mainModule.simulationSpace = ParticleSystemSimulationSpace.World;

    //    }
    //}

}

public interface ITowerTemplate
{
    public void Shoot();
    public void Rotate();
}