using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMove : NetworkBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private NetworkVariable<float> playerHealth = new NetworkVariable<float>(100f);
    private SpriteRenderer sprite;

    Vector3 moveDir = new Vector3();
    [SerializeField]private Rigidbody2D rb;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    private void Update()
    {
        if (IsOwner) {

            Vector3 rotation = new Vector3();

            moveDir.x = Input.GetAxisRaw("Horizontal");
            moveDir.y = Input.GetAxisRaw("Vertical");

            //for-the-looks
            if ((moveDir.x + moveDir.y) < 0 && moveDir.y < 0) sprite.flipX = true;
            else if( moveDir.x != 0 || moveDir.y != 0)sprite.flipX = false;


            if(moveDir.x == 0 && moveDir.y != 0 )
            {
                rotation = new Vector3(0, 0, 90f);
                transform.localRotation = Quaternion.Euler(rotation);
            }
            else if (moveDir.y == 0 && moveDir.x != 0){
                rotation = new Vector3(0, 0, 0);
                transform.localRotation = Quaternion.Euler(rotation);
            }
            else if ((moveDir.x)*(moveDir.y) > 0)
            {
                rotation = new Vector3(0, 0, 45f);
                transform.localRotation = Quaternion.Euler(rotation);
            } else if( moveDir != Vector3.zero)
            {
                rotation = new Vector3(0, 0, 135f);
                transform.localRotation = Quaternion.Euler(rotation);
            }


        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDir * moveSpeed * Time.fixedDeltaTime;
    }
}
