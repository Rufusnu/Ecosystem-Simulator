using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Energy;

public class Coordinator : MonoBehaviour
{
    // Coordinates the whole game behaviour
    
    // set update interval; less resource intensive

    // !! Need to implement different intervals for the various type of updates
    public float updateInterval_EnergySystem = 3f;
    private float _time;

    void Start()
    {
        this._time = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        executeEvery(updateInterval_EnergySystem);
    }

    void executeEvery(float seconds)
    {
        // execute what needs to be executed every [updateInterval_EnergySystem] seconds
        this._time += Time.deltaTime;
        if (this._time >= this.updateInterval_EnergySystem)
        {
            executeRepeatableFunctions();
            this._time = 0;
        }
    }

    void executeRepeatableFunctions()
    {
        // Energy System
        EnergySystem.instance.executeUpdate();
    }
}
