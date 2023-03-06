using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Testing : NetworkBehaviour
{
    //private NetworkVariable<Grid<int>> intGrid;
    public static Grid<int> mapGrid;
    public static List<GameObject> players;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject basicTowerPrefab;
    private NetworkObject basicTowerNet;
    private GameObject basicTowerInstance;

    private void Start()
    {
        mapGrid = new Grid<int> (34, 20, 1f, new Vector3(-17f, -10f));
        generateMap(mapGrid);
        Debug.Log(mapGrid.GetCellCentre(0,0));
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        mapGrid.SetValue(cam.ScreenToWorldPoint(Input.mousePosition), 1);

    //    }
    //    if (mapGrid.GetValue(cam.ScreenToWorldPoint(Input.mousePosition)) == 1)
    //    {
    //        Debug.Log("SelectedSquareIsTrue");
    //    }


    //}


    //this is only for testing purposes remove it later
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsServer) {
                return;
            }
            spawnTowerClientRpc();
        }
    }


    [ClientRpc]
    public void spawnTowerClientRpc()
    {
        basicTowerInstance = Instantiate(basicTowerPrefab);
        basicTowerInstance.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
        basicTowerInstance.transform.rotation = Quaternion.identity;
        basicTowerNet = basicTowerInstance.GetComponent<NetworkObject>();
        basicTowerNet.transform.position = basicTowerInstance.transform.position;
        basicTowerNet.Spawn();
    }


    protected virtual Grid<int> generateMap(Grid<int> mapGrid)
    {
        return mapGrid;
    }
}
 