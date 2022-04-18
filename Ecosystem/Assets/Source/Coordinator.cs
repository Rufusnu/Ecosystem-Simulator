using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Energy;
using GridDomain;
using GameConfigDomain;
using EntityDomain;
using Unity.Mathematics;

public class Coordinator : MonoBehaviour
{
    // Coordinates the whole game behaviour
    
    // set update interval; less resource intensive

    // !! Need to implement different intervals for the various type of updates
    Creature test;
    GridMap grid;
    private float _time;

    void Awake()
    {

    }
    void Start()
    {
        this._time = 0;
        drawGrid();
    }

    // #### [++] Updates [++] ####
    // Update is called once per frame
    void FixedUpdate()
    {
        this.grid.updateCreatures();
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
            this.grid = new GridMap(GameConfig.instance.GridRows, GameConfig.instance.GridColumns, GameConfig.instance.GridCellSize);
            this.grid.positionTo(this.grid.getCenter());
        } catch (System.Exception exception) {
            Debug.Log("Error: " + exception);
        }
    }
}
