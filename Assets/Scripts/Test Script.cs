using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public bool Turn = true;
    public Grid grid;
    public int bounds = 5;
    public bool canPlace = true;

    public GameObject Circle;
    public GameObject Cross;

    public Transform SideDown;
    public Transform SideLeft;
    public Transform SideRight;
    public Transform SideUp;
    public Camera cam;

    private Agent Agent;
    
    private SpriteRenderer circlePrew;
    static System.Random rnd;

    private void Start()
    {
        rnd = new System.Random();
        circlePrew = GetComponent<SpriteRenderer>();
        Agent = GetComponent<Agent>();
        SideDown.position = new Vector3(bounds / 2, -0.3f, 0);
        SideUp.position = new Vector3(bounds / 2, bounds + 0.3f, 0);
        SideLeft.position = new Vector3(-0.3f, bounds/2, 0);
        SideRight.position = new Vector3(bounds + 0.3f, bounds/2, 0);
        cam.transform.position = new Vector3(bounds / 2, bounds / 2, -10);
    }
    //false = cross true = circle
    public Dictionary<Vector3Int, bool> SymbolsDict = new Dictionary<Vector3Int, bool>();
    void Update()
    {
        if (Turn)
        {
            Vector2 mouseToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int StorageValue = grid.WorldToCell(mouseToWorldPoint);
            StorageValue.z = 0;
            Vector3 mouseCell = GetGridCoords(StorageValue);
            mouseCell.z = -0.9f;
            this.transform.position = mouseCell;
            if (SymbolsDict.ContainsKey(StorageValue) || StorageValue.x < 0 || StorageValue.y < 0 || StorageValue.x >= bounds || StorageValue.y >= bounds){
                Color col = Color.red;
                col.a = 0.7f;
                circlePrew.color = col;
                canPlace = false;
            }
            else {
                Color col = Color.white;
                col.a = 0.31f;
                circlePrew.color = col;
                canPlace = true;
            }

            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                if (!SymbolsDict.ContainsKey(StorageValue))
                {
                    //placed = true;
                    Turn = false;
                    Instantiate(Circle).transform.position = mouseCell;
                    SymbolsDict.Add(StorageValue, true);
                    checkWin(SymbolsDict);
                }

            }
        }
        else //Agents Turn
        {
            MakeIntelligentMove();
            
        }
    }

    void MakeIntelligentMove()
    {
        Agent.Move(SymbolsDict);
        Turn = true;
    }
    void MakeRandomMove()
    {
        List<Vector3Int> moves = GetFreeTiles(SymbolsDict);
        int r = rnd.Next(moves.Count);

        Vector3Int move = moves[r];
        SymbolsDict.Add(move, false);

        Vector3 gridCoords = GetGridCoords(move);
        Instantiate(Cross).transform.position = gridCoords;

        Turn = true;
        checkWin(SymbolsDict);
    }

    void MakeMoveWList()
    {
        for (int i = 0; i < bounds; i++)
        {
            for (int j = 0; j < bounds; j++)
            {
                if (!SymbolsDict.ContainsKey(new Vector3Int(i, j, 0)))
                {
                    Vector3Int pos = new Vector3Int(i, j, 0);
                    Vector3 gridCoords = pos;
                    gridCoords = GetGridCoords(gridCoords);
                    Instantiate(Cross).transform.position = gridCoords;
                    SymbolsDict.Add(pos, false);
                    Turn = true;
                    checkWin(SymbolsDict);
                    return;

                }
            }
        }
    }
    List<Vector3Int> GetFreeTiles(Dictionary<Vector3Int, bool> list)
    {
        Vector3Int temp = new Vector3Int();
        List<Vector3Int> output = new List<Vector3Int>();
        for (int i = 0; i < bounds; i++)
        {
            for (int j = 0; j < bounds; j++)
            {
                if (!list.ContainsKey(new Vector3Int(i, j, 0))) { 
                    output.Add(new Vector3Int(i,j,0));
                }
            }
        }
        return output;
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

    void checkWin(Dictionary<Vector3Int, bool> list)
    {
        List<KeyValuePair<Vector3, bool>> tempL = new List<KeyValuePair<Vector3, bool>>();
        foreach (var pair in list)
        {
            //Right/Left

            try
            {
                if (!(pair.Key.x + 4 >= bounds))
                {
                    if ((list[new Vector3Int(pair.Key.x + 1, pair.Key.y, 0)] == pair.Value) &&
                        (list[new Vector3Int(pair.Key.x + 2, pair.Key.y, 0)] == pair.Value) &&
                        (list[new Vector3Int(pair.Key.x + 3, pair.Key.y, 0)] == pair.Value) &&
                        (list[new Vector3Int(pair.Key.x + 4, pair.Key.y, 0)] == pair.Value))
                    {
                        Debug.Log("Horizontal Win");
                        return;
                    }
                }
            }
            catch (KeyNotFoundException)
            {
            }
            try 
            {
                if (!(pair.Key.y + 4 >= bounds))
                {
                    if ((list[new Vector3Int(pair.Key.x, pair.Key.y + 1, 0)] == pair.Value) &&
                        (list[new Vector3Int(pair.Key.x, pair.Key.y + 2, 0)] == pair.Value) &&
                        (list[new Vector3Int(pair.Key.x, pair.Key.y + 3, 0)] == pair.Value) &&
                        (list[new Vector3Int(pair.Key.x, pair.Key.y + 4, 0)] == pair.Value))
                    {
                        Debug.Log("Vertical Win");
                        return;
                    }
                }
                    
            }
            catch (KeyNotFoundException)
            {
            }
            try
            {
                if (!(pair.Key.x + 4 >= bounds || pair.Key.y + 4 >= bounds))
                {
                    if ((list[new Vector3Int(pair.Key.x+1, pair.Key.y + 1, 0)] == pair.Value) &&
                        (list[new Vector3Int(pair.Key.x + 2, pair.Key.y + 2, 0)] == pair.Value) &&
                        (list[new Vector3Int(pair.Key.x + 3, pair.Key.y + 3, 0)] == pair.Value) &&
                        (list[new Vector3Int(pair.Key.x + 4, pair.Key.y + 4, 0)] == pair.Value))
                    {
                        Debug.Log("Diagonal Win (right leaning)");
                        return;
                    }
                }
            }
            catch (KeyNotFoundException) { }
            try
            {
                if (!(pair.Key.x - 4 >= bounds || pair.Key.y + 4 >= bounds))
                {
                    if ((list[new Vector3Int(pair.Key.x - 1, pair.Key.y + 1, 0)] == pair.Value) &&
                        (list[new Vector3Int(pair.Key.x - 2, pair.Key.y + 2, 0)] == pair.Value) &&
                        (list[new Vector3Int(pair.Key.x - 3, pair.Key.y + 3, 0)] == pair.Value) &&
                        (list[new Vector3Int(pair.Key.x - 4, pair.Key.y + 4, 0)] == pair.Value))
                    {
                        Debug.Log("Diagonal Win (left leaning)");
                        return;
                    }
                }
            }
            catch (KeyNotFoundException) { }
            
            
        }
    }
}

