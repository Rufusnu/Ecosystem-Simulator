using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Energy;
using GridDomain;
using GameConfigDomain;
using EntityDomain;

public class Coordinator : MonoBehaviour
{
    // Coordinates the whole game behaviour
    
    // set update interval; less resource intensive

    // !! Need to implement different intervals for the various type of updates
    Creature test;
    private float _time;

    void Awake()
    {

    }
    void Start()
    {
        this._time = 0;
        drawGrid();
        this.test = new Creature(new Vector2Int(1,2));
    }

    // #### [++] Updates [++] ####
    // Update is called once per frame
    void FixedUpdate()
    {
        executeEvery(GameConfig.instance.UpdateIntervalOfEnergySystem);
    }

    private void executeRepeatableFunctions()
    {
        // Energy System
        EnergySystem.executeUpdate();
    }

    private void executeEvery(float seconds)
    {
        // execute what needs to be executed every [updateInterval_EnergySystem] seconds
        this._time += Time.deltaTime;
        if (this._time >= GameConfig.instance.UpdateIntervalOfEnergySystem)
        {
            executeRepeatableFunctions();
            this._time = 0;
        }
    }
    // #### [--] Updates [--] ####

    private void drawGrid()
    {
        try {
            GridMap grid = new GridMap(GameConfig.instance.GridRows, GameConfig.instance.GridColumns, GameConfig.instance.GridCellSize);
            grid.positionTo(grid.getCenter());
        } catch (System.Exception exception) {
            Debug.Log("Error: " + exception);
        }
    }
}
