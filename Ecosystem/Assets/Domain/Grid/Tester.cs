using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridDomain;


public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try {
            GridMap grid = new GridMap(10, 20, 10);
        } catch (System.Exception exception) {
            Debug.Log("Error: " + exception);
        }
    }
}
