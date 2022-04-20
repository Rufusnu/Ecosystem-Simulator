using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
// project domains
using Energy;
using GeneticsDomain;

namespace EntityDomain
{
    
    public abstract class LivingEntity : Entity
    {
        
        // can send signals to energy system and energy system decides what to do with this object's energy levels

        // #### [++] Attributes [++] ####
        public static int livingEntityCounter = 0;
        private Chromosome _chromosome;
        private float _energy;
        protected LivingState _livingState;
        // #### [--] Attributes [--] ####


        // #### [++] Initialization [++] ####
        public LivingEntity(int2 newCoordinates) : base(newCoordinates)
        {
            LivingEntity.livingEntityCounter++;
            this._energy = EnergySystem.defaultEnergyValue;
            this._livingState = LivingState.Alive;
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
        public void modifyEnergyBy(float amount)
        {
            if (this._energy + amount <= 0)
            {
                // newEnergy >  0
                die();
            }
            else                                
            {
                // newEnergy <= 0
                this._energy += amount;
            }
        }
        protected virtual void consumeEnergy(ActionType action)
        {
            modifyEnergyBy(action.energyNeeded);
        }
        // ---- [--] Energy [--] ----

        public LivingState getLivingState()
        {
            return this._livingState;
        } 
        public bool isAlive()
        {
            if (this._livingState == LivingState.Alive)
            {
                return true;
            }
            return false;
        }
        // #### [--] Getters & Setters [--] ####


        // #### [++] Behaviour [++] ####
        protected void die()
        {
            if (this.isAlive())
            {
                //  status is set to DEAD and Coordinator will check for dead entities every few seconds and remove them
                if (GameConfigDomain.GameConfig.instance.Debugging)
                {
                    Debug.Log(this.getObject().name + " died.");
                }
                this._livingState = LivingState.Dead;
                MonoBehaviour.Destroy(this.getObject());
            }
        }
        protected abstract void eat(LivingEntity entity); // to be implemented by Plants and Creatures
        // #### [--] Behaviour [--] ####
    }
}