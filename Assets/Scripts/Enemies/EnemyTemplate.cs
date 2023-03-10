using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class EnemyTemplate : NetworkBehaviour
{

    //enemystats
    [SerializeField] private float enemyHealth;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int killReward;
    [SerializeField] private float playerDamage;
    [SerializeField] private float armorHealth;
    [SerializeField] private GameObject coin;

    //change this during playtesting
    private float spread = 1f;
    private int coinValue = 10;

    //variables for pathfinding
    private Vector3 initialpos = new Vector3();
    private Vector3 nextpos = new Vector3();
    private Vector3 endPos = new Vector3();
    private Vector2 randomPos;

    //networking variables
    private WaveSpawnner wave;
    private NetworkObject self;

    //just-for-the-looks
    [SerializeField] private SpriteRenderer sprite;


    //my functions
    private void checkNextCell()
    {
        initialpos = nextpos;


        //checking if enemy reached the end
        if (nextpos != endPos)
        {
            int currentIndexX = Testing.mapGrid.GetCellIndex(nextpos).x;
            int currentIndexY = Testing.mapGrid.GetCellIndex(nextpos).y;
            if (Testing.mapGrid.GetValue(currentIndexX, currentIndexY + 1) == 1)
            {
                nextpos = Testing.mapGrid.GetCellCentre(currentIndexX, currentIndexY + 1);
            }
            else if (Testing.mapGrid.GetValue(currentIndexX, currentIndexY - 1) == 1)
            {
                nextpos = Testing.mapGrid.GetCellCentre(currentIndexX, currentIndexY + 1);
            }
            else
            {
                nextpos = Testing.mapGrid.GetCellCentre(currentIndexX + 1, currentIndexY);
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
            randomPos = transform.position + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread));
            Instantiate(coin, randomPos, Quaternion.identity);
        }
        self = gameObject.GetComponent<NetworkObject>();
        self.Despawn();
    }

    public void takeDamage(float damage, bool canArmored)
    {
        if(canArmored && armorHealth > 0)
        {
            armorHealth -= damage;
        }
        else if(armorHealth == 0)
        {
            enemyHealth -= damage;
        }
    }

    private void reachedEnd()
    {
        //do something with player damage
        die();
    }

    private void findStartPos()
    {
        for(int y = 0; y < 20; y++)
        {
            if(Testing.mapGrid.GetValue(0,y) == 1)
            {
                initialpos = Testing.mapGrid.GetCellCentre(0, y);
                nextpos = Testing.mapGrid.GetCellCentre(1, y);
                break; //change this if you want multiple starting points
            }
        }
        
    }
    private void findEndPos()
    {
        int x = 33;
        for(int y = 0; y < 20; y++)
        {
            if (Testing.mapGrid.GetValue(x, y) == 1) {
                endPos = Testing.mapGrid.GetCellCentre(x, y);
                Debug.Log("endpos is :" + endPos);
            }
        }
    }



    //in-built functions
    private void Awake()
    {
        findStartPos();
        findEndPos();
        Debug.Log(transform.position);
        transform.position.Set(initialpos.x, initialpos.y, 0);
        Debug.Log(transform.position);
    }


    private void Update()
    {
        move();
        if (transform.position == nextpos)
        {
            checkNextCell();
        }


        //testing-purposes
        if (Input.GetKeyDown(KeyCode.K)) die();
        
    }
}


