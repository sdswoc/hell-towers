using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public static Grid<int> mapGrid;

    public static GameManager gameManager;

    private void Awake()
    {
        gameManager = this;       
    }

    private void Start()
    {
        mapGrid = new Grid<int>(34, 20, 1f, new Vector3(-18, -10));
        generateMap(mapGrid);


    }


    //functions to edit the grid
    [ServerRpc]
    public void changeGridValueServerRpc(Vector2 cell, int value)
    {
        mapGrid.SetValue(cell, value);
        changeGridValueClientRPC(cell, value);
    }
    [ClientRpc]
    public void changeGridValueClientRPC(Vector2 cell, int value)
    {
        mapGrid.SetValue(cell, value);
    }

    #region generate-map
    protected virtual Grid<int> generateMap(Grid<int> mapGrid)
    {
        for (int x = 0; x < 17; x++)
        {

            mapGrid.SetValue(x, 2, 1);
        }
        for (int y = 2; y < 11; y++)
        {
            mapGrid.SetValue(16, y, 1);
        }
        for (int x = 16; x < 26; x++)
        {
            mapGrid.SetValue(x, 10, 1);
        }
        for (int y = 12; y < 15; y++)
        {
            mapGrid.SetValue(25, y, 1);
        }
        for (int x = 25; x < 34; x++)
        {
            mapGrid.SetValue(x, 14, 1);
        }
        return mapGrid;
    }
    #endregion

    #region stats
    public TMP_Text currencyTxt;
    public TMP_Text playerHealthTxt;

    public static NetworkVariable<int> currency = new NetworkVariable<int>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public static NetworkVariable<int> playerHealth = new NetworkVariable<int>(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        playerHealth.OnValueChanged += (int oldval, int newval) =>
        {
            //currencyTxt.text = newval.ToString();
            changePlayerHealthValueClientRPC(newval);
        };
        currency.OnValueChanged += (int oldval, int newval) =>
        {
            //playerHealthTxt.text = newval.ToString();
            changeCurrencyValueClientRPC(newval);
        };
        currency.Value = 10;
        playerHealth.Value = 10;

        Debug.Log(currency.Value + "  " + playerHealth.Value);
        base.OnNetworkSpawn();
    }


    [ClientRpc]
    void changeCurrencyValueClientRPC(int newval)
    {
        if(!IsServer) currency.Value = newval;
        currencyTxt.text = newval.ToString();
    }
    [ClientRpc]
    void changePlayerHealthValueClientRPC(int newval)
    {
        if(!IsServer)playerHealth.Value = newval;
        playerHealthTxt.text = newval.ToString();
    }
    #endregion

    //bullet-sends-info-to-this-script-and-then-this-object-reduces-health-of-the-enemy-or-the-player
    #region Tower-spawnning
    [Header("Towers-prefabs")]

    [SerializeField] GameObject tower1prefab;
    [SerializeField] GameObject tower2prefab;
    [SerializeField] GameObject tower3prefab;
    [SerializeField] GameObject tower4prefab;

    public Camera cam;

    GameObject tower_to_spawn;
    Vector3 mousePos;

    bool tower1;
    bool tower2;
    bool tower3;
    bool tower4;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            if (mapGrid.GetValue(mousePos) == 0 )
            {
                if (tower1)
                {
                    Vector2 cell = new Vector2(mapGrid.GetCellIndex(mousePos).x, mapGrid.GetCellIndex(mousePos).y);
                    tower_to_spawn = tower1prefab;
                    if (IsServer)
                    {
                        
                        changeGridValueClientRPC(cell, 2);
                        SpawnTower();
                    }
                    else
                    {
                        changeGridValueServerRpc(cell, 2);
                        SpawnTowerServerRPC();
                    }
                    tower1 = false;
                }
                else if (tower2)
                {
                    Vector2 cell = new Vector2(mapGrid.GetCellIndex(mousePos).x, mapGrid.GetCellIndex(mousePos).y);
                    tower_to_spawn = tower2prefab;
                    if (IsServer)
                    {

                        changeGridValueClientRPC(cell, 2);
                        SpawnTower();
                    }
                    else
                    {
                        changeGridValueServerRpc(cell, 2);
                        SpawnTowerServerRPC();
                    }
                    tower2 = false;
                }
                else if (tower3)
                {
                    Vector2 cell = new Vector2(mapGrid.GetCellIndex(mousePos).x, mapGrid.GetCellIndex(mousePos).y);
                    tower_to_spawn = tower3prefab;
                    if (IsServer)
                    {

                        changeGridValueClientRPC(cell, 2);
                        SpawnTower();
                    }
                    else
                    {
                        changeGridValueServerRpc(cell, 2);
                        SpawnTowerServerRPC();
                    }
                    tower3 = false;
                }
                else if (tower4)
                {
                    Vector2 cell = new Vector2(mapGrid.GetCellIndex(mousePos).x, mapGrid.GetCellIndex(mousePos).y);
                    tower_to_spawn = tower4prefab;
                    if (IsServer)
                    {

                        changeGridValueClientRPC(cell, 2);
                        SpawnTower();
                    }
                    else
                    {
                        changeGridValueServerRpc(cell, 2);
                        SpawnTowerServerRPC();
                    }
                    tower4 = false;
                }
            }
        }
    }

    public void spawnTower1()
    {
        tower1 = true;
    }
    public void spawnTower2()
    {
        tower2 = true;
    }
    public void spawnTower3()
    {
        tower3 = true;
    }
    public void spawnTower4()
    {
        tower4 = true;
    }

    [ServerRpc]
    void SpawnTowerServerRPC()
    {
        SpawnTower();
    }

    void SpawnTower()
    {
        tower_to_spawn.transform.position = mapGrid.GetCellCentre(mousePos);
        tower_to_spawn.GetComponent<NetworkObject>().Spawn();
    }
    #endregion
}