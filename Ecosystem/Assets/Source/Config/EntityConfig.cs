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
        public float MutationFactor = 0.01f; // default
        public float AverageDifferenceValidator = 0.5f;
        public float RejectionProbability = 0.3f;
        // ---- [--] Entity Genetics [--] ----

        // ---- [++] Toggles [++] ----
        public bool CreatureSense_Sight_Creature = true;
        public bool CreatureSense_Sight_Plant = true;
        public bool CreatureSense_Smell_Creature = true;
        public bool CreatureSense_Smell_Plant = true;
        // ---- [--] Toggles [--] ----


        public float maxKcalEnergy = 3000; // kcal
        // ---- [++] Creatures [++] ----
        public Color CreatureNeutralColor = new Color(0.5f, 0.5f, 0.5f, 1.0f); 
        public float UpdateIntervalOfCreatureBrain = 0.5f; // default 200ms
        public int SightDistanceInCells = 4;
        public int SmellDistanceInCells = 2;
        public int SmellLowestToSense = 4;    // default lowest smell intensity value that can be sensed
        public float MoveDuration = 1.0f; // seconds
        public float MoveSpeed = 3.0f;
        public float minDeathAge = 90;
        public float maxDeathAge = 150;
        public float CreatureMinNutritionValue = 300; // kcal
        public float CreatureMaxNutritionValue = 500; // kcal
        // ---- [--] Creatures [--] ----

        // ---- [++] Plants [++] ----
        public float PlantMinNutritionValue = 250; // kcal
        public float PlantMaxNutritionValue = 450; // kcal
        // ---- [--] Plants [--] ----

        public GameObject AnimatedFoodPrefab;

        // ---- [++] Smell [++] ----
        public float SmellSpriteAlpha = 0.2f;
        public float SmellPlaceInterval = 1f; // seconds
        // ---- [--] Smell [--] ----

        // ### [--] Config List [--] ###
    }
}
