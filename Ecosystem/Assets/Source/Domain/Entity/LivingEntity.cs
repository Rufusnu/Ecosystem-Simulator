using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Energy;


namespace EntityDomain
{
    public abstract class LivingEntity : Entity
    {
        // can send signals to energy system and energy system decides what to do with this object's energy levels

        // #### [++] Attributes [++] ####
        private float _energy;
        // #### [--] Attributes [--] ####


        // #### [++] Constructor [++] ####
        public LivingEntity(Vector2Int newCoordinates, float newEnergy) : base (newCoordinates) // used to call base class <Entity> constructor for inherited attributes
        {
            if (newEnergy <= 0)
            {
                throw new System.Exception("<Entity> Cannot set negative or equal to zero energy.");
            }
            this._energy = newEnergy;
        }

        public LivingEntity(Vector2Int newCoordinates) : base (newCoordinates) // used to call base class <Entity> constructor for inherited attributes
        {
            this._energy = EnergySystem.defaultEnergyValue;
        }
        // #### [--] Constructor [--] ####

        
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

    }

}