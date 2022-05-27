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

        // #### #### [++] Attributes [++] #### ####
        public static int livingEntityCounter = 0;
        protected Chromosome _chromosome;
        protected float _energy;
        protected float _nutritionValue;
        protected LivingState _livingState;
        protected float _time;
        // #### #### [--] Attributes [--] #### ####


        // #### #### [++] Initialization [++] #### ####
        public LivingEntity(int2 newCoordinates) : base(newCoordinates)
        {
            LivingEntity.livingEntityCounter++;
            this._energy = EnergySystem.defaultEnergyValue;
            this._livingState = LivingState.Alive;
        }
        // #### #### [--] Initialization [--] #### ####

        
        // #### #### [++] Getters & Setters [++] #### ####
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
            else if (this._energy + amount >= 1.0f)
            {
                this._energy = 1.0f;
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
        protected virtual void consumeEnergy(ActionType action, float modifier)
        {
            modifyEnergyBy(action.energyNeeded + action.energyNeeded*modifier);
        }
        // ---- [--] Energy [--] ----

        // ---- [++] Nutrition [++] ----
        public float getNutrition()
        {
            return this._nutritionValue;
        }
        // ---- [--] Nutrition [--] ----

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

        public Chromosome getChromosome()
        {
            return this._chromosome;
        }
        // #### #### [--] Getters & Setters [--] #### ####


        // #### #### [++] Behaviour [++] #### ####
        protected void die()
        {
            if (this.isAlive())
            {
                //  status is set to DEAD and Coordinator will check for dead entities every few seconds and remove them
                if (Configs.Debugging())
                {
                    Debug.Log(this.getObject().name + " died.");
                }
                this._livingState = LivingState.Dead;
                this.destroySmell();
                this.destroyObject();
                GridDomain.GridMap.currentGridInstance.killLivingEntity(this);
            }
        }
        protected abstract void eat(LivingEntity entity); // to be implemented by Plants and Creatures
        public abstract void eatenBy(LivingEntity entity);
        public abstract void updateBrain();
        public abstract void updateStats();
        
        protected abstract void initializeNutritionValue();
        public abstract int getSmellIntensity();
        protected abstract void destroySmell();
        protected abstract bool isSimilar(LivingEntity creature);
        // #### #### [--] Behaviour [--] #### ####
    }
}