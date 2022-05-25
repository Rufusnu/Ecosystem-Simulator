using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace EntityDomain
{
    public class Plant : LivingEntity
    {
        public static int plantCounter = 0;
        private float _age;
        private float _randomInterval;
        private BrainState _brainState;


        public Plant(int2 newCoordinates) : base(newCoordinates)
        {
            this._randomInterval = randomizeInterval(Configs.BrainUpdateInterval_Creature());
            this._time = UnityEngine.Random.Range(-this._randomInterval, this._randomInterval);

            initializePlantObject();
            initializeNutritionValue();
            initializeAge();
            initializeStates();
            Plant.plantCounter++;
        }
        private void initializePlantObject()
        {
            this.setObject(new GameObject("[Plant" + Plant.plantCounter + "]"));
            this.getObject().AddComponent<SpriteRenderer>().sprite = Entity_AssetsService.instance.plant_default;
            this.getObject().transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            this.getObject().GetComponent<SpriteRenderer>().color = new Color(0.10f, 0.6f, 0.10f, 1.0f);      // green color
        }
        protected override void initializeNutritionValue()
        {
            // value [-20%; +20%] * Average Nutrition Value (Default value from entity config)
            float min = Configs.NutritionValueMin_Plant();
            float max = Configs.NutritionValueMax_Plant();

            this._nutritionValue = (float)UnityEngine.Random.Range(min, max);
        }
        private void initializeAge()
        {
            this._age = 0.1f;
        }
        private void initializeStates()
        {
            this._brainState = BrainState.Idle;
        }


        // #### #### [++] Brain [++] #### ####
        // might need some changing because im going to work with state pattern (switch-case)
        // this updating method might not wait for currently active action to finish -> find a method -> maybe actuallty states
        public override void updateBrain()
        {
            if (this.isAlive())
            {
                /*if (isOld())
                {
                    // TO DO
                    this.die();
                    return;
                }*/

                executeEvery(this._randomInterval);
            }
        }
        private void toBeExecutedEvery()
        {
            // put here what needs to be executed after given seconds
            if (this._brainState == BrainState.Idle)
            {   
                // creature is not doing anything
                this._brainState = BrainState.Busy;   // Creature is doing something
                this._randomInterval = randomizeInterval(Configs.BrainUpdateInterval_Creature());
                //act();
            }
            else if (this._brainState == BrainState.Busy)
            {
                // creature is doing something
                // TO DO : check if attacked or if there are predators nearby
            }
        }

        private void executeEvery(float seconds)
        {   // execute every given seconds
            this._time += Time.deltaTime;    // only increment if creature is idle

            if (this._time >= seconds)
            {
                this._time = 0;
                if (Configs.Debugging())
                {
                    Debug.Log(this.getObject().name + " brain update after " + seconds + " seconds");
                    Debug.Log(this.getObject().name + " Energy: " + this.getEnergy());
                }
                toBeExecutedEvery();
            }
        }
        private float randomizeInterval(float seconds)
        {   
            // should add a modifier (genetics) to change the randomize interval to
            // make the creature "smarter" by thinking faster for each individual
            // return UnityEngine.Random.Range(seconds/2 - this._chromosome.getGenes()[1]/4, seconds*2 - this._chromosome.getGenes()[1]/4);
            return UnityEngine.Random.Range(seconds/1.35f, seconds*1.35f);
        }
        // #### #### [--] Brain [--] #### ####


        protected override void eat(LivingEntity entity)
        {
            // TO DO
        }
        public override void eatenBy(LivingEntity entity)
        {
            this.die();
        }
        public override void updateStats()
        {
            
        }
    }
}

