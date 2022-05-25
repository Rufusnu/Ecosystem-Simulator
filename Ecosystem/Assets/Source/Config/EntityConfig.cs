using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityConfigDomain
{
    public class EntityConfig : MonoBehaviour
    {
        public static EntityConfig instance; // make the instance visble and usable
        private static bool _alive = false;

        private void Awake() {
            if (_alive == false)
            {
                instance = this;
                _alive = true;
            }
        }

        // ### [++] Config List [++] ###
        //public GameObject CreaturePrefab;

        // ---- [++] Entity Genetics [++] ----
        public float MutationFactor = 0.02f; // default
        // ---- [--] Entity Genetics [--] ----


        public float maxKcalEnergy = 3000; // kcal
        // ---- [++] Creatures [++] ----
        public Color CreatureNeutralColor = new Color(0.5f, 0.5f, 0.5f, 1.0f); 
        public float UpdateIntervalOfCreatureBrain = 0.5f; // default 200ms
        public int SightDistanceInCells = 4;
        public float MoveDuration = 1.0f; // seconds
        public float MoveSpeed = 3.0f;
        public float minDeathAge = 60;
        public float maxDeathAge = 110;
        public float CreatureMinNutritionValue = 300; // kcal
        public float CreatureMaxNutritionValue = 500; // kcal
        // ---- [--] Creatures [--] ----

        // ---- [++] Plants [++] ----
        public float PlantMinNutritionValue = 250; // kcal
        public float PlantMaxNutritionValue = 450; // kcal
        // ---- [--] Plants [--] ----

        public GameObject AnimatedFoodPrefab;

        // ### [--] Config List [--] ###
    }
}
