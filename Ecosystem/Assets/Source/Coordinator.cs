using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Energy;
using GridDomain;
using EntityDomain;
using Unity.Mathematics;

public class Coordinator : MonoBehaviour
{
    // Coordinates the whole game behaviour
    
    // set update interval; less resource intensive
    
    Creature test;
    GridMap grid;
    private float _time;

    void Start()
    {
        this.name = "Coordinator";
        Time.timeScale = Time.timeScale * Configs.GameSpeedMultiplier();
        this._time = 0;
        drawGrid();
    }

    // #### #### [++] Updates [++] #### ####
    // Update is called once per frame
    void FixedUpdate()
    {
        this.grid.updateLivingEntitiesList();
        executeSpawnEvery(EnergySystem.UpdateIntervalOfEnergySystem);
    }
    
    private void executeRepeatableFunctions()
    {
        // Energy System
        EnergySystem.executeUpdate();
        
        // entities collector
        
        
        this.grid.spawnRandomPlants();
    }

    private void executeSpawnEvery(float seconds)
    {
        // execute what needs to be executed every [updateInterval_EnergySystem] seconds
        this._time += Time.deltaTime;
        if (this._time >= EnergySystem.UpdateIntervalOfEnergySystem)
        {
            executeRepeatableFunctions();
            this._time = 0;
        }
    }
    // #### #### [--] Updates [--] #### ####


    private void drawGrid()
    {
        try {
            this.grid = GridMap.createGridMap(Configs.GridRows(), Configs.GridColumns(), Configs.GridCellSize());
        } catch (System.Exception exception) {
            Debug.Log("Error: " + exception);
        }
    }
}
