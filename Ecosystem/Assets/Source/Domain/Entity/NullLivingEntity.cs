using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
// project domains
using Energy;
using GeneticsDomain;

namespace EntityDomain
{
    public class NullLivingEntity : LivingEntity
    {
        
        // can send signals to energy system and energy system decides what to do with this object's energy levels

        // #### #### [++] Attributes [++] #### ####
        private Chromosome _chromosome;
        private float _energy;
        protected float _nutritionValue;
        protected LivingState _livingState;
        // #### #### [--] Attributes [--] #### ####


        // #### #### [++] Initialization [++] #### ####
        public NullLivingEntity() : base(new int2(-1, -1))
        {
            this._energy = 0;
            this._livingState = LivingState.Dead;
        }
        // #### #### [--] Initialization [--] #### ####

        
        // #### #### [++] Getters & Setters [++] #### ####
        // ---- [++] Energy [++] ---- 
        public void setEnergy(float newEnergy) {}
        public float getEnergy()
        {
            return this._energy;
        }
        public void modifyEnergyBy(float amount) {}
        protected virtual void consumeEnergy(ActionType action) {}
        // ---- [--] Energy [--] ----

        // ---- [++] Nutrition [++] ----
        public float getNutrition()
        {
            return this._nutritionValue;
        }
        // ---- [--] Nutrition [--] ----
        // #### #### [--] Getters & Setters [--] #### ####

        protected override void eat(LivingEntity entity) {} // to be implemented by Plants and Creatures
        public override void eaten() {}
        protected override void initializeNutritionValue() {}
    }
}