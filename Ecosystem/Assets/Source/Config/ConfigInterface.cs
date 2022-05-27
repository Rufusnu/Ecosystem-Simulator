using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityConfigDomain;
using GameConfigDomain;

public static class Configs
{
    // ### ### [++] GameConfig List [++] ### ###
        // ---- [++] Debugging [++] ---- 
        public static float GameSpeedMultiplier()
        {
            return GameConfig.instance.GameSpeedMultiplier;
        }
        public static bool Debugging()
        {
            return GameConfig.instance.Debugging;
        }
        public static void setDebugging(bool state)
        {
            GameConfig.instance.Debugging = state;
        }
        public static bool Debugging_EnergySystem()
        {
            return GameConfig.instance.Debugging_EnergySystem;
        }
        public static void setDebugging_EnergySystem(bool state)
        {
            GameConfig.instance.Debugging_EnergySystem = state;
        }
        // ---- [--] Debugging [--] ---- 


        // ---- [++] Grid [++] ---- 
        public static int GridRows()
        {
            return GameConfig.instance.GridRows;
        }
        public static int GridColumns()
        {
            return GameConfig.instance.GridColumns;
        }
        public static float GridCellSize()
        {
            return GameConfig.instance.GridCellSize;
        }
        // ---- [--] Grid [--] ---- 


        // ---- [++] Spawn Probabilities [++] ----
        public static int SpawnProbability_Creature()
        {
            return GameConfig.instance.SpawnEntityProbability;
        }
        public static int SpawnProbability_Plant()
        {
            return GameConfig.instance.SpawnPlantProbability;
        }
        // ---- [--] Spawn Probabilities [--] ----
    // ### ### [--] GameConfig List [--] ### ###


    // ### ### [++] EntityConfig List [++] ### ###
        // ---- [++] Genetics [++] ----
        public static float MutationFactor()
        {
            return EntityConfig.instance.MutationFactor;
        }
        public static void setMutationFactor(float factor)
        {
            EntityConfig.instance.MutationFactor = factor;
        }
        // ---- [--] Genetics [--] ----


        // ---- [++] EnergySystem [++] ----
        public static float MaxKcalEnergy()
        {
            return EntityConfig.instance.maxKcalEnergy;
        }
        // ---- [--] EnergySystem [--] ----


        // ---- [++] Creature [++] ----
        public static Color NeutralColor_Creature()
        {
            return EntityConfig.instance.CreatureNeutralColor;
        }
        public static float BrainUpdateInterval_Creature()
        {
            return EntityConfig.instance.UpdateIntervalOfCreatureBrain;
        }
        public static int SightDistance()
        {
            // Return sight distance in cells (integer)
            return EntityConfig.instance.SightDistanceInCells;
        }
        public static int SmellDistance()
        {
            // Return sight distance in cells (integer)
            return EntityConfig.instance.SmellDistanceInCells;
        }
        public static int SmellDefaultSense()
        {
            return EntityConfig.instance.SmellLowestToSense;
        }
        public static float MoveDurationDefault()
        {
            return EntityConfig.instance.MoveDuration;
        }
        public static float MoveSpeedDefault()
        {
            return EntityConfig.instance.MoveSpeed;
        }
        public static float DeathAgeMin()
        {
            return EntityConfig.instance.minDeathAge;
        }
        public static float DeathAgeMax()
        {
            return EntityConfig.instance.maxDeathAge;
        }
        public static float NutritionValueMin_Creature()
        {
            return EntityConfig.instance.CreatureMinNutritionValue;
        }
        public static float NutritionValueMax_Creature()
        {
            return EntityConfig.instance.CreatureMaxNutritionValue;
        }

        public static bool CreatureSense_Sight_Creature()
        {
            return EntityConfig.instance.CreatureSense_Sight_Creature;
        }
        public static bool CreatureSense_Sight_Plant()
        {
            return EntityConfig.instance.CreatureSense_Sight_Plant;
        }
        public static bool CreatureSense_Smell_Creature()
        {
            return EntityConfig.instance.CreatureSense_Smell_Creature;
        }
        public static bool CreatureSense_Smell_Plant()
        {
            return EntityConfig.instance.CreatureSense_Smell_Plant;
        }

        public static float AverageDifferenceValidator()
        {
            return EntityConfig.instance.AverageDifferenceValidator;
        }
        public static float RejectionProbability()
        {
            return EntityConfig.instance.RejectionProbability;
        }
        // ---- [--] Creature [--] ----


        // ---- [++] Plant [++] ----
        public static float NutritionValueMin_Plant()
        {
            return EntityConfig.instance.PlantMinNutritionValue;
        }
        public static float NutritionValueMax_Plant()
        {
            return EntityConfig.instance.PlantMaxNutritionValue;
        }
        // ---- [--] Plant [--] ----

        public static GameObject AnimatedFoodPrefab()
        {
            return EntityConfig.instance.AnimatedFoodPrefab;
        }

        // ---- [++] Smell [++] ----
        public static float SmellSpriteAlpha()
        {
            return EntityConfig.instance.SmellSpriteAlpha;
        }
        public static float SmellPlaceInterval()
        {
            return EntityConfig.instance.SmellPlaceInterval;
        }
        // ---- [--] Smell [--] ----
    // ### ### [--] EntityConfig List [--] ### ###
}
