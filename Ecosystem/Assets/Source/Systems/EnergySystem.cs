using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridDomain;
using EntityDomain;

namespace Energy
{
    public static class EnergySystem
    {
        // Responsable for every LivingEntity energy changes

        // #### #### [++] Attributes [++] #### ####
        public static float defaultEnergyValue = 0.8f; // 80%
        public static float UpdateIntervalOfEnergySystem = 3.0f; // default 3 seconds
        public static float EnergyConsumedOnUpdateEnergySystem = -0.01f;
        public static float MoveConsumption = -0.01f;
        public static float ThinkConsumption = -0.005f;
        public static float MateConsumption = -0.3f;

        // #### #### [--] Attributes [--] #### ####


        // methods to be implemented
        public static void executeUpdate()
        {
            if (Configs.Debugging_EnergySystem())
            {
                Debug.Log("Energy System Update");
            }
            GridMap.currentGridInstance.updateLivingEntitiesEnergyClock(EnergyConsumedOnUpdateEnergySystem);
        }

        public static float kcalToEnergy(float kcal)
        {
            return kcal/EntityConfigDomain.EntityConfig.instance.maxKcalEnergy;
        }
    }
}

