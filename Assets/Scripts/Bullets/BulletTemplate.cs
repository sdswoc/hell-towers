using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTemplate : MonoBehaviour
{
   
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Tower"))
        {
            Destroy(gameObject);
        }
    }
}
