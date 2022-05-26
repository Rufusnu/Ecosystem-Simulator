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
        protected new float _nutritionValue;
        protected new LivingState _livingState;
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
        public new void setEnergy(float newEnergy) {}
        public new float getEnergy()
        {
            return this._energy;
        }
        public new void modifyEnergyBy(float amount) {}
        protected new virtual void consumeEnergy(ActionType action) {}
        // ---- [--] Energy [--] ----

        // ---- [++] Nutrition [++] ----
        public new float getNutrition()
        {
            return this._nutritionValue;
        }
        // ---- [--] Nutrition [--] ----
        // #### #### [--] Getters & Setters [--] #### ####

        protected override void eat(LivingEntity entity) {} // to be implemented by Plants and Creatures
        public override void eatenBy(LivingEntity entity) {}
        protected override void initializeNutritionValue() {}
        public override void updateBrain() {}
        public override void updateStats() {}
        public override int getSmellIntensity() {return 0;}
        protected override void destroySmell() {}
    }
}