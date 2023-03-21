using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class GameManager : NetworkBehaviour
{
    public static Grid<int> mapGrid;

    #region singleton
    public static GameManager gameManager;

    private void Awake()
    {
        gameManager = this;
    }
    #endregion

    private void Start()
    {
        mapGrid = new Grid<int>(34, 20, 1f, new Vector3(-18, -10));
        generateMap(mapGrid);
        playerHealthTxt.text = playerHealth.ToString();
        currencyTxt.text = currency.ToString();
    }

    //functions to edit the grid
    [ServerRpc(RequireOwnership = false)]
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


    //bullet-sends-info-to-this-script-and-then-this-object-reduces-health-of-the-enemy-or-the-player
    #region Tower-spawnning
    [Header("Towers-prefabs")]

    [SerializeField] GameObject tower1prefab;
    [SerializeField] int tower1cost;
    [SerializeField] GameObject tower2prefab;
    [SerializeField] int tower2cost;
    [SerializeField] GameObject tower3prefab;
    [SerializeField] int tower3cost;
    [SerializeField] GameObject tower4prefab;
    [SerializeField] int tower4cost;

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
            if (mapGrid.GetValue(mousePos) == 0)
            {
                if (tower1)
                {
                    Vector2 cell = new Vector2(mapGrid.GetCellIndex(mousePos).x, mapGrid.GetCellIndex(mousePos).y);
                    tower_to_spawn = tower1prefab;
                    if (!IsServer) decreaseHealthServerRPC(tower1cost);
                    else decreaseCurrency(tower1cost);
                    tower1 = false;
                    changeGridValueClientRPC(cell, 2);
                    if (!IsServer) SpawnTowerServerRPC(1, mousePos);
                    else SpawnTowerClientRPC();
                }
                else if (tower2)
                {
                    Vector2 cell = new Vector2(mapGrid.GetCellIndex(mousePos).x, mapGrid.GetCellIndex(mousePos).y);
                    tower_to_spawn = tower2prefab;
                    if (!IsServer) decreaseHealthServerRPC(tower2cost);
                    else decreaseCurrency(tower2cost);
                    tower2 = false;
                    changeGridValueClientRPC(cell, 2);
                    if (!IsServer) SpawnTowerServerRPC(2, mousePos);
                    else SpawnTowerClientRPC();
                }
                else if (tower3)
                {
                    Vector2 cell = new Vector2(mapGrid.GetCellIndex(mousePos).x, mapGrid.GetCellIndex(mousePos).y);
                    tower_to_spawn = tower3prefab;
                    if (!IsServer) decreaseHealthServerRPC(tower3cost);
                    else decreaseCurrency(tower3cost);
                    tower3 = false;
                    changeGridValueClientRPC(cell, 2);
                    if (!IsServer) SpawnTowerServerRPC(3, mousePos);
                    else SpawnTowerClientRPC();
                }

                else if (tower4)
                {
                    Vector2 cell = new Vector2(mapGrid.GetCellIndex(mousePos).x, mapGrid.GetCellIndex(mousePos).y);
                    tower_to_spawn = tower4prefab;
                    if (!IsServer) decreaseHealthServerRPC(tower4cost);
                    else decreaseCurrency(tower4cost);
                    tower4 = false;
                    changeGridValueClientRPC(cell, 2);
                    if (!IsServer) SpawnTowerServerRPC(4, mousePos);
                    else SpawnTowerClientRPC();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            decreaseHealth(1);
        }
        if (playerHealth_net.Value < 0)
        {
            Application.Quit();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnTowerServerRPC(int i, Vector3 clickedPos)
    {
        if (i == 1)
        {
            tower_to_spawn = tower1prefab;
        }
        else if (i == 2)
        {
            tower_to_spawn = tower2prefab;
        }
        else if (i == 3)
        {
            tower_to_spawn = tower3prefab;
        }
        else if (i == 4)
        {
            tower_to_spawn = tower4prefab;
        }
        else tower_to_spawn = null;
        Instantiate(tower_to_spawn);
        tower_to_spawn.GetComponent<NetworkObject>().Spawn();
        tower_to_spawn.transform.position = mapGrid.GetCellCentre(clickedPos);
    }

    [ClientRpc]
    void SpawnTowerClientRPC()
    {
        tower_to_spawn.transform.position = mapGrid.GetCellCentre(mousePos);
        if (IsServer)
        {
            Instantiate(tower_to_spawn);
            tower_to_spawn.GetComponent<NetworkObject>().Spawn();
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
    #endregion


    #region Statistics
    NetworkVariable<int> playerHealth_net = new NetworkVariable<int>();
    NetworkVariable<int> currency_net = new NetworkVariable<int>();

    [SerializeField] private int playerHealth;
    [SerializeField] private int currency;

    [SerializeField] private TMP_Text playerHealthTxt;
    [SerializeField] private TMP_Text currencyTxt;

    public override void OnNetworkSpawn()
    {
        playerHealth_net.Value = playerHealth;
        currency_net.Value = currency;

        playerHealth_net.OnValueChanged += changePlayerHealthTxt;
        currency_net.OnValueChanged += changeCurrencyTxt;

        base.OnNetworkSpawn();
    }

    private void changePlayerHealthTxt(int oldval, int newval)
    {
        playerHealthTxt.text = newval.ToString();
    }
    private void changeCurrencyTxt(int oldval, int newval)
    {
        currencyTxt.text = newval.ToString();
    }


    public void decreaseHealth(int damage)
    {
        if (IsServer) playerHealth_net.Value -= damage;
    }
    public void increaseCurrency()
    {
        if (IsServer) currency_net.Value += 10;
    }
    public void decreaseCurrency(int cost)
    {
        if (IsServer) currency_net.Value -= cost;
    }

    [ServerRpc(RequireOwnership = false)]
    void decreaseHealthServerRPC(int cost)
    {
        decreaseCurrency(cost);
    }

    #endregion
}