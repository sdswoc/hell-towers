using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1 : Testing
{
    //public static Transform initialPos;
    //[SerializeField] private GameObject enemyPrefab;


    protected override Grid<int> generateMap(Grid<int> mapGrid)
    {

        for(int x = 0; x < 17; x++)
        {
            //if( x== 0){
            //    initialPos.position = mapGrid.GetCellCentre(0, 2);
            //}
            mapGrid.SetValue(x, 2, 1);
        }
        for (int y = 2; y < 11; y++)
        {
            mapGrid.SetValue(16, y, 1);
        }
        for(int x = 16; x < 26; x++)
        {
            mapGrid.SetValue(x, 10, 1);
        }
        for(int y = 12; y < 15; y++)
        {
            mapGrid.SetValue(25, y, 1);
        }
        for(int x = 25; x < 34; x++)
        {
            mapGrid.SetValue(x, 14, 1);
        }
        return mapGrid;


    }

    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.Q))
    //    {
    //        Instantiate(enemyPrefab, initialPos);
    //    }
    //}
}