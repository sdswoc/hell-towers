using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private Vector3 cellcentre;
    private TGridObject[,] gridArray;

   public Grid (int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];

        Debug.Log(width + " " + height);

        for(int i = 0; i < gridArray.GetLength(0); i++)
        {
            for(int j = 0; j < gridArray.GetLength(1); j++)
            {
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        cellcentre = new Vector3(cellSize / 2, cellSize / 2);

    }

   

    //related to cellposition and cellindex (use it for snapping)
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }
    public Vector3 GetCellCentre(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return (new Vector3(x, y) * cellSize) + originPosition + cellcentre;
    }
    public Vector3 GetCellCentre(int x,  int y)
    {
        return (new Vector3(x, y) * cellSize) + originPosition + cellcentre;
    }
    public Vector3Int GetCellIndex(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return new Vector3Int(x, y);

    }



    //setting and getting values
    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    public void SetValue(int x, int y, TGridObject value)
    {   if(x >= 0 && y>= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
        }
    }
    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public TGridObject GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else return default(TGridObject);
        
    }

}
