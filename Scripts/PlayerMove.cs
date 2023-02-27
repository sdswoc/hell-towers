using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMove : NetworkBehaviour
{

    [SerializeField] private float moveSpeed = 5f;

    private void Start()
    {
        Debug.Log("PlayerCreated!");
    }

    private void Update()
    {
        if (IsOwner) {
            Vector3 moveDir = new Vector3();

            if (Input.GetKey(KeyCode.W)) moveDir.y = +1f;
            if (Input.GetKey(KeyCode.S)) moveDir.y = -1f;
            if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
            if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

            transform.position += moveDir * moveSpeed * Time.fixedDeltaTime;
        }

    }
}
