using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMap : GameManager
{
    protected override Grid<int> generateMap(Grid<int> mapGrid)
    {
        //this is temporary chage it later when u have time
        for( int x = 0; x< 36; x++)
        {
            mapGrid.SetValue(x, 4, 1);
        }
        return mapGrid;
    }
}
