using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public static Grid<int> mapGrid;

    private void Start()
    {
        mapGrid = new Grid<int>(36, 20, 1f, new Vector3(-18, -10));
        generateMap(mapGrid);
    }


    //functions to edit the grid
    [ServerRpc]
    public void changeGridValueServerRpc(Vector2 cell, int value)
    {
        mapGrid.SetValue(cell, value);
    }

    protected virtual Grid<int> generateMap(Grid<int> mapGrid)
    {
        return mapGrid;
    }
    
}
