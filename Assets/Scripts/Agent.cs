using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public GameObject Cross;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector3 MakeTurn(Dictionary<Vector3, bool> list, int bounds)
    {
        List<Vector3> list2 = new List<Vector3>();
        Vector3 vec;
        vec = new Vector3(UnityEngine.Random.Range(0, bounds), Mathf.RoundToInt(UnityEngine.Random.Range(0, bounds)), 0);
        return vec;
    }
}
