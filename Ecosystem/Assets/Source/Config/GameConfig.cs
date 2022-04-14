using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameConfigDomain
{
    public class GameConfig : MonoBehaviour
    {
        public static GameConfig instance; // make the instance visble and usable

        private void Awake() {
            instance = this;
        }

        // ### [++] Config List [++] ###
        // ---- [++] Grid [++] ---- 
        public int GridRows = 8; // default
        public int GridColumns = 8; // default
        public float GridCellSize = 1.2f; // default
        // ---- [--] Grid [--] ---- 


        // ---- [++] Coordinator [++] ----
        public float UpdateIntervalOfEnergySystem = 3.0f; // default 3 seconds
        public float EnergyConsumedOnUpdateEnergySystem = 0.05f;
        // ---- [--] Coordinator [--] ----
        // ### [--] Config List [--] ###
    }

}
