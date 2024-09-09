using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public bool Turn = true;
    public Grid grid;
    public int bounds = 5;

    public GameObject Circle;
    public GameObject Cross;

    private Agent Agent;

    private void Start()
    {
        Agent = GetComponent<Agent>();
    }
    //false = cross true = circle
    public Dictionary<Vector3, bool> SymbolsDict = new Dictionary<Vector3, bool>();
    void Update()
    {
        if (Turn)
        {
            Vector2 mouseToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //mouseToWorldPoint.z = 0
            Vector3Int StorageValue = grid.WorldToCell(mouseToWorldPoint);
            Debug.Log(StorageValue);
            StorageValue.z = 0;
            Vector3 mouseCell = GetGridCoords(StorageValue);
            this.transform.position = mouseCell;

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(SymbolsDict);
                if (!SymbolsDict.ContainsKey(StorageValue))
                {
                    Debug.Log(StorageValue);
                    //placed = true;
                    Turn = false;
                    Instantiate(Circle).transform.position = mouseCell;
                    SymbolsDict.Add(StorageValue, true);

                }

            }
        }
        else //Agents Turn
        {
            MakeMove();
        }
    }

    void MakeMove()
    {
        for (int i = 0; i < bounds; i++)
        {
            for (int j = 0; j < bounds; j++)
            {
                if (!SymbolsDict.ContainsKey(new Vector3(i, j, 0)))
                {
                    Vector3Int pos = new Vector3Int(i, j, 0);
                    Vector3 gridCoords = pos;
                    gridCoords = GetGridCoords(gridCoords);
                    Debug.Log(pos);
                    Instantiate(Cross).transform.position = gridCoords;
                    SymbolsDict.Add(pos, false);
                    Turn = true;
                    return;

                }
            }
        }
    }
    Vector3 GetGridCoords(Vector3Int pos)
    {
        Vector3 temp = grid.CellToWorld(pos);
        temp.x += (0.5f * grid.cellSize.x);
        temp.y += (0.5f * grid.cellSize.y);
        return temp;
    }

    Vector3 GetGridCoords(Vector3 pos)
    {
        Vector3 temp = grid.CellToWorld(grid.WorldToCell(pos));
        temp.x += (0.5f * grid.cellSize.x);
        temp.y += (0.5f * grid.cellSize.y);
        return temp;
    }

}

