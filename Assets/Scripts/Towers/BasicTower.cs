using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : TowerTemplate,ITowerTemplate
{
    private GameObject nearestEnemy;
    float angle;
    [SerializeField] float offset;

    //in-script variables
    private bool canShoot = true;
    private float temp = 0;


    //Interface-functions
    public void Shoot()
    {
        angle += offset;
        Vector3 newRotation = new Vector3(0, 0, angle);
        transform.localRotation = Quaternion.Euler(newRotation);
        BulletPooler.spawnFromPool("type1", attackPoint.position, attackPoint.rotation);
        temp = timeBetweenShots;   
    }

    public void Rotate()
    {
        if (nearestEnemy != null)
        {
            Vector2 relative = nearestEnemy.transform.position - transform.position;
            angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
            Vector3 newRotation = new Vector3(0, 0, angle);
            transform.localRotation = Quaternion.Euler(newRotation);
        }
    }

    private void findNearestEnemy()
    {
        float distance = range;
        nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float _distance = (transform.position - enemy.transform.position).magnitude;

            if (_distance < distance)
            {
                nearestEnemy = enemy;
                distance = _distance;

            }
        }
    }



    //in-built functions
    private void Update()
    {
        findNearestEnemy();
        Rotate();
        if (temp > 0)
        {
            temp -= Time.deltaTime;
        }
        else if (enemies.Count != 0 && canShoot)
        {
            canShoot = false;
            Shoot();
        }
        else canShoot = true;
   }
}


