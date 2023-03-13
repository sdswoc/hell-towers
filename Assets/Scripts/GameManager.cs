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

        //setting initial scores n health
        if (IsServer)
        {
            currency.Value = 10;
            playerHealth.Value = 10;
        }
        Debug.Log(currency.Value + "  " + playerHealth.Value);
    }


    //functions to edit the grid
    [ServerRpc]
    public void changeGridValueServerRpc(Vector2 cell, int value)
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
        //if (!IsServer) return;


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
        base.OnNetworkSpawn();
    }


    [ClientRpc]
    void changeCurrencyValueClientRPC(int newval)
    {
        currency.Value = newval;
        currencyTxt.text = newval.ToString();
    }
    [ClientRpc]
    void changePlayerHealthValueClientRPC(int newval)
    {
        playerHealth.Value = newval;
        playerHealthTxt.text = newval.ToString();
    }
    #endregion

    //bullet-sends-info-to-this-script-and-then-this-object-reduces-health-of-the-enemy-or-the-player



    


}