using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private void Update()
    {
        transform.position += transform.right * movementSpeed * Time.deltaTime;
    }

}
