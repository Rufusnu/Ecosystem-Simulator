using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Energy
{
    public abstract class EnergySystem
    {
        // Responsable for every LivingEntity energy changes

        // #### [++] Attributes [++] ####
        public static float defaultEnergyValue = 0.8f; // 80%

        // #### [--] Attributes [--] ####


        // #### [++] Constructor [++] ####
        public EnergySystem()
        {

        }
        // #### [--] Constructor [--] ####

        // methods to be implemented
        public static void executeUpdate()
        {

        }
    }
}

