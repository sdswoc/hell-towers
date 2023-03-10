using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTower : TowerTemplate,ITowerTemplate
{
    //in-script variables
    private bool canShoot;
    private float temp;
    [SerializeField] private Transform[] attackPoints = new Transform[6];

    //interface functions
    public void Shoot()
    {
        foreach( Transform attackPoint in attackPoints)
        {
            BulletPooler.spawnFromPool("type3", attackPoint.position, attackPoint.rotation);
        }
        temp = timeBetweenShots;
    }

    public void Rotate() { }

    //in-built functions

    private void Update()
    {
        if (temp > 0)
        {
            temp -= Time.deltaTime;
        }
        else if (enemies.Count != 0 && canShoot)
        {
            Debug.Log(enemies);
            canShoot = false;
            Shoot();
        }
        else canShoot = true;
    }
}
