using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Energy;
using Unity.Mathematics;

namespace EntityDomain
{
    public abstract class LivingEntity : Entity
    {
        // can send signals to energy system and energy system decides what to do with this object's energy levels

        // #### [++] Attributes [++] ####
        static int livingEntityCounter = 0;
         private Chromosome _chromosome;
        private float _energy;
        // #### [--] Attributes [--] ####


        // #### [++] Initialization [++] ####
        public override void Initialize(int2 newCoordinates)
        {
            base.Initialize(newCoordinates);
            LivingEntity.livingEntityCounter++;
            this._energy = EnergySystem.defaultEnergyValue;
        }
        // #### [--] Initialization [--] ####

        
        // #### [++] Getters & Setters [++] ####
        // ---- [++] Energy [++] ---- 
        public void setEnergy(float newEnergy)
        {
            if (newEnergy <= 0)
            {
                throw new System.Exception("<LivingEntity> Cannot set negative Energy.");
            }
        }
        public float getEnergy()
        {
            return this._energy;
        }
        // ---- [++] Energy [++] ---- 
        // #### [--] Getters & Setters [--] ####


        // #### [++] Behaviour [++] ####
        protected abstract void eat(LivingEntity entity); // to be implemented by Plants and Creatures
        // #### [--] Behaviour [--] ####
    }
}