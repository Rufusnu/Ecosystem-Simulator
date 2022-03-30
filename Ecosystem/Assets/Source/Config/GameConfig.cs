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
        public int GridRows = 8;
        public int GridColumns = 8;
        public float GridCellSize = 5;
        // ---- [--] Grid [--] ---- 


        // ---- [++] Coordinator [++] ----
        public float UpdateIntervalOfEnergySystem = 3f;
        // ---- [--] Coordinator [--] ----
        // ### [--] Config List [--] ###
    }

}
