using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class EnemyTemplate : NetworkBehaviour, IgetObjectType
{

    //enemystats
    [SerializeField] private float enemyHealth;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int killReward;
    [SerializeField] private float playerDamage;
    [SerializeField] private float armorHealth;
    [SerializeField] private GameObject coin;
    [SerializeField] public string enemyName;

    private NetworkVariable<float> enemyHealth_net = new NetworkVariable<float>();
    private NetworkVariable<float> armorHealth_net = new NetworkVariable<float>();

    //change this during playtesting
    private float spread = 1f;
    private int coinValue = 10;

    //variables for pathfinding
    private Vector3 initialpos = new Vector3();
    private Vector3 nextpos = new Vector3();
    private Vector3 endPos = new Vector3(17.5f, 6.5f, 0f);
    private Vector2 randomPos;

    //networking variables
    private WaveSpawnner wave;
    private NetworkObject self;
    private GameManager gameManager;

    //just-for-the-looks
    [SerializeField] private SpriteRenderer sprite;

    //interface functions
    public string isEquals()
    {
        return enemyName;
    }

    //my functions
    private void checkNextCell()
    {
        initialpos = nextpos;


        //checking if enemy reached the end
        if (nextpos != endPos)
        {
            int currentIndexX = GameManager.mapGrid.GetCellIndex(nextpos).x;
            int currentIndexY = GameManager.mapGrid.GetCellIndex(nextpos).y;
            if (GameManager.mapGrid.GetValue(currentIndexX, currentIndexY + 1) == 1)
            {
                nextpos = GameManager.mapGrid.GetCellCentre(currentIndexX, currentIndexY + 1);
            }
            else if (GameManager.mapGrid.GetValue(currentIndexX, currentIndexY - 1) == 1)
            {
                nextpos = GameManager.mapGrid.GetCellCentre(currentIndexX, currentIndexY + 1);
            }
            else
            {
                nextpos = GameManager.mapGrid.GetCellCentre(currentIndexX + 1, currentIndexY);
            }
        }
        else reachedEnd();
        

    }

    private void move()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextpos, movementSpeed * Time.deltaTime);
        Vector3 lookDir = nextpos - transform.position;
        Vector3 rotation = new Vector3();

        if (lookDir.x == 0 && lookDir.y > 0)
        {
            sprite.flipY = false;
            transform.localRotation = Quaternion.Euler(rotation);
        }
        else if (lookDir.x == 0 && lookDir.y < 0)
        {
            sprite.flipY = true;
            transform.localRotation = Quaternion.Euler(rotation);
        }
        else if (lookDir.y == 0 && lookDir.x > 0)
        {
            rotation = Vector3.forward * -90f;
            transform.localRotation = Quaternion.Euler(rotation);
        }
        else
        {
            rotation = Vector3.forward * 90f;
            transform.localRotation = Quaternion.Euler(rotation);
        }
    }

    private void die()
    {
        int no_of_coins = killReward / coinValue;
        for(int i = 0; i < no_of_coins; i++)
        {
            if (!IsServer) break;
            randomPos = transform.position + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread));
            GameObject obj = Instantiate(coin);
            obj.transform.position = randomPos;
            obj.transform.rotation = Quaternion.identity;
            obj.GetComponent<NetworkObject>().Spawn();
        }
        self = gameObject.GetComponent<NetworkObject>();
        wave.removeEnemy(gameObject);
        
    }

    public void takeDamage(float damage, bool canArmored)
    {
        if(canArmored && armorHealth > 0)
        {
            armorHealth_net.Value -= damage;
        }
        else if(armorHealth == 0)
        {
            Debug.Log("initial value: " + enemyHealth_net.Value);
            enemyHealth_net.Value -= damage;
            enemyHealth = enemyHealth_net.Value;
            Debug.Log("final value: " + enemyHealth_net.Value);
            Debug.Log("enemyHealth is : " + enemyHealth);
        }
    }

    private void reachedEnd()
    {
        gameManager.decreasePlayerHealthClientRPC((int)playerDamage);
        wave.removeEnemy(gameObject);
    }

    private void findStartPos()
    {
        for (int y = 0; y < 20; y++)
        {
            if (GameManager.mapGrid.GetValue(0, y) == 1)
            {
                initialpos = GameManager.mapGrid.GetCellCentre(0, y);
                nextpos = GameManager.mapGrid.GetCellCentre(1, y);
                break; //change this if you want multiple starting points
            }
        }
     }
        //private void findEndPos()
        //{
        //    int x = 33;
        //    for(int y = 0; y < 20; y++)
        //    {
        //        if (GameManager.mapGrid.GetValue(x, y) == 1) {
        //            endPos = GameManager.mapGrid.GetCellCentre(x, y);
        //            Debug.Log("endpos is :" + endPos);
        //        }
        //    }
        //}



        //in-built functions
    private void OnEnable()
    {
        findStartPos();
        transform.position.Set(initialpos.x, initialpos.y, 0);
    }

    private void Start()
    {
        wave = WaveSpawnner.Instance;
        gameManager = GameManager.gameManager;
    }

    public override void OnNetworkSpawn()
    {
        enemyHealth_net.Value = enemyHealth;
        armorHealth_net.Value = armorHealth;
        base.OnNetworkSpawn();
    }

    private void Update()
    {
        if(IsServer && enemyHealth == 0)
        {
            Debug.Log("die called!");
            die();
        }

        move();
        if (transform.position == nextpos)
        {
            checkNextCell();
        }


        //testing-purposes
        if (Input.GetKeyDown(KeyCode.K)) die();
    }
}
public interface IgetObjectType{
    string isEquals();
}