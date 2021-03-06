using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameConfigDomain
{
    public class GameConfig : MonoBehaviour
    {
        public static GameConfig instance; // make the instance visble and usable
        private static bool _alive = false;

        private void Awake() {
            if (_alive == false)
            {
                instance = this;
                _alive = true;
            }
        }

        // ### [++] Config List [++] ###
        public float GameSpeedMultiplier = 1;
        public bool Debugging = false;
        public bool Debugging_EnergySystem = false;

        // ---- [++] Grid [++] ---- 
        public int GridRows = 8; // default
        public int GridColumns = 8; // default
        public float GridCellSize = 1.2f; // default
        // ---- [--] Grid [--] ---- 


        // ---- [++] Coordinator [++] ----
        public int SpawnEntityProbability = 40; // spawn 1 every [SpawnEntityProbability] tiles
        public int SpawnPlantProbability = 1000; // spawn 1 every [SpawnPlantProbability] tiles
        // ---- [--] Coordinator [--] ----
        // ### [--] Config List [--] ###
    }

}
