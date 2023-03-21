using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.EventSystems;

public class TowerPlacementScript : NetworkBehaviour
{
    public List<GameObject> towers;
    private List<NetworkObject> net_towers;

    GameObject tower;
    [SerializeField] private Camera cam;

    int i = 0;
    bool tower_being_placed;


    private void Awake()
    {
        for(int i = 0; i < towers.Count; i++)
        {
            net_towers[i] = towers[i].GetComponent<NetworkObject>();
        }
    }

    private void Update()
    {
        if (tower_being_placed)
        {
            while (Input.GetMouseButton(0))
            {
                tower.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
            }
        }
    }



    private void Spawn(int i)
    {
        tower = towers[i - 1];
        net_towers[i - 1].Spawn();
        tower_being_placed = true;
    }
}
