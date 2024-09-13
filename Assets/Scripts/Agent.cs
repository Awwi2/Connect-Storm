using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;



public class Agent : MonoBehaviour
{
    static System.Random rn;
    public GameObject Cross;
    public TestScript tscript;
    public int bounds;
    public int minEval;
    private System.Diagnostics.Stopwatch timer;
    // Start is called before the first frame update
    void Start()
    {
        tscript = GetComponent<TestScript>();
        rn = new System.Random();
        bounds = tscript.bounds;
        timer = new System.Diagnostics.Stopwatch();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Move(Dictionary<Vector3Int, bool> list)
    {
        timer.Reset();
        timer.Start();
        Debug.Log(MiniMax(list, bounds, 4, true));
        Debug.Log("Time Taken: " + timer.Elapsed);
        timer.Stop();

    }
    int MiniMax(Dictionary<Vector3Int, bool> list, int bounds, int depth, bool maxPlayer)
    {
        if (depth == 0) {
            //return EvaluateV2(list, !maxPlayer);
            return EvaluateV1(list);
        }
        List<Vector3Int> neighbours = GetNeighbours(list);
        //true means AI Agents Turn False means Players Turn
        if (maxPlayer) 
        {
            minEval = -100000000;
            foreach (Vector3Int v in neighbours) 
            {
                list.Add(v, false);
                //Debug.Log("Calling MiniMax with depth: " + (depth - 1) + "and list length. " + list.Count);
                int eval = MiniMax(list, bounds, depth - 1, true);
                list.Remove(v);
                minEval = Math.Max(eval, minEval);
                
            }
            return minEval;
        }
        else
        {
            minEval = 100000000;
            foreach (Vector3Int v in neighbours)
            {
                list.Add(v, true);
                int eval = MiniMax(list, bounds, depth - 1, false);
                list.Remove(v);
                minEval = Math.Min(eval, minEval);

            }
        }
        return -1;

    }

    int EvaluateV1(Dictionary<Vector3Int, bool> l)
    {
        //return 10;
        return rn.Next(10000);
    }

    int EvaluateV2(Dictionary<Vector3Int, bool> l, bool turn)
    {
        //turn = false is the AI Agent Move, turn = true is the players turn
        int value = 0;
        List<String> SList = GetLines(l);
        foreach (String s in SList) 
        {
            if (s.Contains("XXXXX")) { value += 50000000; }
            if (s.Contains("-XXXX-") && !turn) { value += 1000000; }
            if (s.Contains("0XXXX-") && !turn) { value += 1000000; }
            if (s.Contains("0XXXX-") && turn) { value += 1000; }
            if (s.Contains("-XXX-") && !turn) { value += 200; }

            if (s.Contains("00000")) { value += -50000000; }
            if (s.Contains("-0000-") && turn) { value += -1000000; }
            if (s.Contains("-0000-") && !turn) { value += -100000; }
            if (s.Contains("X0000-") && turn) { value += -1000000; }
            if (s.Contains("X0000-") && !turn) { value += -1000; }
            if (s.Contains("-000-") && turn) { value += -200; }
        }

        return value;
    }
    List<String> GetLines(Dictionary<Vector3Int, bool> l)
    {
        List<String> lines = new List<String>();
        for (int i = 0; i < bounds; i++) {
            String s = "";
            for (int j = 0; i < bounds; j++) {
                if (l.ContainsKey(new Vector3Int(i, j, 0))) {
                    if (l[new Vector3Int(i, j, 0)] == true)
                    {
                        s += "0";
                    }
                    else { s += "X"; }
                }
                else { s += "-"; }
            }
            //check for patterns 

            lines.Add(s);
        }
        for (int i = 0; i < bounds; i++)
        {
            String s = "";
            for (int j = 0; i < bounds; j++)
            {
                if (l.ContainsKey(new Vector3Int(j, i, 0)))
                {
                    if (l[new Vector3Int(j, i, 0)] == true)
                    {
                        s += "0";
                    }
                    else { s += "X"; }
                }
                else { s += "-"; }
            }
            lines.Add(s);
        }
        return lines;
    }

    public List<Vector3Int> GetNeighbours(Dictionary<Vector3Int, bool> l)
    {
        List<Vector3Int> keyList = new List<Vector3Int>(l.Keys);
        List<Vector3Int> o = new List<Vector3Int> ();
        foreach (Vector3Int v in keyList)
        {
            if(!o.Contains(new Vector3Int(v.x +1, v.y, 0)) && !l.ContainsKey(new Vector3Int(v.x + 1, v.y, 0)) && (v.x+1 <bounds)) 
            {
                o.Add(new Vector3Int(v.x + 1, v.y, 0));
            }
            if (!o.Contains(new Vector3Int(v.x - 1, v.y, 0)) && !l.ContainsKey(new Vector3Int(v.x - 1, v.y, 0)) && (v.x - 1 >= 0))
            {
                o.Add(new Vector3Int(v.x - 1, v.y, 0));
            }
            if (!o.Contains(new Vector3Int(v.x, v.y+1, 0)) && !l.ContainsKey(new Vector3Int(v.x, v.y+1, 0)) && (v.y + 1 < bounds))
            {
                o.Add(new Vector3Int(v.x, v.y+1, 0));
            }
            if (!o.Contains(new Vector3Int(v.x, v.y-1, 0)) && !l.ContainsKey(new Vector3Int(v.x, v.y-1, 0)) && (v.y -1 >= 0))
            {
                o.Add(new Vector3Int(v.x, v.y-1, 0));
            }
            if (!o.Contains(new Vector3Int(v.x+1, v.y+1, 0)) && !l.ContainsKey(new Vector3Int(v.x+1, v.y+1, 0)) && (v.x + 1 < bounds && v.y +1 < bounds))
            {
                o.Add(new Vector3Int(v.x+1, v.y+1, 0));
            }
            if (!o.Contains(new Vector3Int(v.x+1, v.y-1, 0)) && !l.ContainsKey(new Vector3Int(v.x+1, v.y-1, 0)) && (v.x + 1 < bounds && v.y - 1 >= 0))
            {
                o.Add(new Vector3Int(v.x+1, v.y-1, 0));
            }
            if (!o.Contains(new Vector3Int(v.x-1, v.y+1, 0)) && !l.ContainsKey(new Vector3Int(v.x-1, v.y+1, 0)) && (v.y + 1 < bounds && v.x -1 >= 0))
            {
                o.Add(new Vector3Int(v.x-1, v.y+1, 0));
            }
            if (!o.Contains(new Vector3Int(v.x-1, v.y-1, 0)) && !l.ContainsKey(new Vector3Int(v.x-1, v.y-1, 0)) && (v.y - 1 >= 0 && v.x -1 >= 0))
            {
                o.Add(new Vector3Int(v.x-1, v.y-1, 0));
            }
        }
        
        //Debug.Log("Num neighbours " + o.Count);
        return o;
        
    }
    List<Vector3Int> GetFreeTiles(Dictionary<Vector3Int, bool> list)
    {
        List<Vector3Int> output = new List<Vector3Int>();
        for (int i = 0; i < bounds; i++)
        {
            for (int j = 0; j < bounds; j++)
            {
                if (!list.ContainsKey(new Vector3Int(i, j, 0)))
                {
                    output.Add(new Vector3Int(i, j, 0));
                }
            }
        }
        return output;
    }
}
