using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Energy
{
    public class EnergySystem
    {
        // Responsable for every LivingEntity energy changes
        public static EnergySystem instance; // make the instance visble and usable

        private void Awake() {
            instance = this;
        }

        // methods to be implemented
        public void executeUpdate()
        {

        }
    }
}

