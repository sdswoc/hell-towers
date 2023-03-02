using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTower : TowerTemplate, ITowerTemplate
{
    private GameObject nearestPlayer;

    //in-script variables
    private bool canShoot = true;
    private float temp = 0f;
    private int i = 0;
    private bool shooting = false;

    //Interace-functions
    public void Shoot()
    {
        InvokeRepeating("callShoot", 0f, timeBetweenShooting);
        temp = timeBetweenShots;

    }
    public void Rotate()
    {
        if (nearestPlayer != null)
        {
            Vector2 relative = nearestPlayer.transform.position - transform.position;
            float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
            Vector3 newRotation = new Vector3(0, 0, angle);
            transform.localRotation = Quaternion.Euler(newRotation);
        }
    }

    private void findNearestPlayer()
    {
        float distance = range;
        nearestPlayer = null;
        foreach (GameObject player in players)
        {
            float _distance = (transform.position - player.transform.position).magnitude;

            if (_distance < distance)
            {
                nearestPlayer = player;
                distance = _distance;

            }
        }
    }

    private void callShoot()
    {
        Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
        i++;
    }

    //in-built functions
    private void Update()
    {
        findNearestPlayer();
        Rotate();
        if (temp > 0 && !shooting)
        {
            temp -= Time.deltaTime;
        }
        else if (enemies.Count != 0 && canShoot)
        {
            Debug.Log(enemies);
            canShoot = false;
            Shoot();
            shooting = true;
        }
        else canShoot = true;

        if(i == numberOfBulletsPerShot)
        {
            i = 0;
            CancelInvoke();
            shooting = false;
        }
    }
}
