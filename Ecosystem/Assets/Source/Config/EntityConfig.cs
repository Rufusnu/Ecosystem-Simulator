using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityDomain
{
    public class EntityConfig : MonoBehaviour
    {
        public static EntityConfig instance; // make the instance visble and usable

            private void Awake() {
                instance = this;
            }

            // ### [++] Config List [++] ###
            //public GameObject CreaturePrefab;

            // ---- [++] Entity Genetics [++] ----
            public float MutationFactor = 0.05f; // default
            // ---- [--] Entity Genetics [--] ----


            public float maxKcalEnergy = 3000; // kcal
            // ---- [++] Creatures [++] ----
            public float UpdateIntervalOfCreatureBrain = 0.5f; // default 200ms
            public int SightDistanceInCells = 3;
            public float MoveDuration = 1.0f; // seconds
            public float MoveSpeed = 3.0f;
            public float minDeathAge = 60;
            public float maxDeathAge = 110;
            public float CreatureMinNutritionValue = 200; // kcal
            public float CreatureMaxNutritionValue = 300; // kcal
            // ---- [--] Creatures [--] ----

            // ---- [++] Plants [++] ----
            public float PlantMinNutritionValue = 150; // kcal
            public float PlantMaxNutritionValue = 250; // kcal
            // ---- [--] Plants [--] ----

            // ### [--] Config List [--] ###
    }
}
