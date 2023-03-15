using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunTower : TowerTemplate,ITowerTemplate
{
    [SerializeField] private float spread;
    private GameObject nearestEnemy;
    [SerializeField] private AudioSource shot;

    //in-script variables
    private bool canShoot = true;
    private float temp = 0;

    //Interface Functions;
    public void Shoot()
    {
        for(int i = 0; i < numberOfBulletsPerShot; i++)
        {
            float angle = attackPoint.eulerAngles.z;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + Random.Range(-spread, spread)));
            BulletPooler.spawnFromPool("type2", attackPoint.position, rotation);
        }
        temp = timeBetweenShots;
        shot.Play();
    }
    public void Rotate()
    {
        if (nearestEnemy != null)
        {
            Vector2 relative = nearestEnemy.transform.position - transform.position;
            float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
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
            Debug.Log(enemies);
            canShoot = false;
            Shoot();
        }
        else canShoot = true;

    }
}
