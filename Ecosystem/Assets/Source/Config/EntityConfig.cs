using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityConfig : MonoBehaviour
{
    public static EntityConfig instance; // make the instance visble and usable

        private void Awake() {
            instance = this;
        }

        // ### [++] Config List [++] ###
        // ---- [++] Entity Genetics [++] ----
        public float MutationFactor = 0.05f; // default 5%
        // ---- [--] Entity Genetics [--] ----

        // ---- [++] Creature Brain [++] ----
        public float UpdateIntervalOfCreatureBrain = 0.2f; // default 200ms
        // ---- [--] Creature Brain [--] ----
        // ### [--] Config List [--] ###
}
