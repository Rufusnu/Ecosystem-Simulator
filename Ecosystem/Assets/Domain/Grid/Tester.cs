using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridDomain;


public class Tester : MonoBehaviour
{
    public int rows = 8;
    public int columns = 8;
    public float cellSize = 5;
    // Start is called before the first frame update
    void Start()
    {
        try {
            GridMap grid = new GridMap(rows, columns, cellSize);
            grid.positionTo(getCenter());
        } catch (System.Exception exception) {
            Debug.Log("Error: " + exception);
        }
    }

    private Vector3 getCenter()
    {
        // numbering from 0
        int rows = this.rows - 1;
        int cols = this.columns - 1;
        return new Vector3(-cols * this.cellSize/2, -rows * this.cellSize /2, 0);
    }
}
