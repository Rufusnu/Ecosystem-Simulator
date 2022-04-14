using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridDomain;
using GameConfigDomain;

namespace Energy
{
    public static class EnergySystem
    {
        // Responsable for every LivingEntity energy changes

        // #### [++] Attributes [++] ####
        public static float defaultEnergyValue = 0.8f; // 80%
        

        // #### [--] Attributes [--] ####


        // methods to be implemented
        public static void executeUpdate()
        {

            GridMap.currentGridInstance.consumeAllCreaturesEnergy(GameConfig.instance.EnergyConsumedOnUpdateEnergySystem);
        }
    }
}

