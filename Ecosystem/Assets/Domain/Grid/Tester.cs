using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridDomain;


public class Tester : MonoBehaviour
{
    public int rows = 8;
    public int columns = 8;
    public int cellSize = 5;
    // Start is called before the first frame update
    void Start()
    {
        try {
            GridMap grid = new GridMap(rows, columns, cellSize);
        } catch (System.Exception exception) {
            Debug.Log("Error: " + exception);
        }
    }
}
