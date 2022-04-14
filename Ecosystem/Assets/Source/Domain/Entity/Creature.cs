using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace EntityDomain
{
    public class Creature : LivingEntity
    {
        // #### [++] Attributes [++] ####
        public static int creatureCounter = 0;
        const int NumberOfGenes = 0; // number of attributes that can be mutated
        private float _time;
        private float _randomInterval;
        // #### [--] Attributes [--] ####


        // #### [++] Initialization [++] ####
        private void Start()
        {
            Creature.creatureCounter++;
            this._time = 0;
            this._randomInterval = randomizeInterval(EntityConfig.instance.UpdateIntervalOfCreatureBrain);
        }
        public override void Initialize(int2 newCoordinates)
        {
            base.Initialize(newCoordinates);
            
        }
        // #### [--] Initialization [--] ####


        // #### [++] Brain [++] ####
        // might need some changing because im going to work with state pattern (switch-case)
        // this updating method might not wait for currently active action to finish -> find a method -> maybe actuallty states
        public void Update()
        {
            executeEvery(this._randomInterval);
        }
        private void executeEvery(float seconds)
        {   // execute every given seconds
            this._time += Time.deltaTime;
            if (this._time >= seconds)
            {   // put here what needs to be executed after given seconds
                // functionToBeExecuted();
                Debug.Log("Brain action every " + seconds + " seconds.");
                this._time = 0;
                this._randomInterval = randomizeInterval(EntityConfig.instance.UpdateIntervalOfCreatureBrain);
            }
        }
        private float randomizeInterval(float seconds)
        {   // should add a modifier (genetics) to change the randomize interval to
            // make the creature "smarter" by thinking faster for each individual
            return UnityEngine.Random.Range(seconds/2, seconds*2);
        }
        // #### [--] Brain [--] ####


        // #### [++] Getters & Setters [++] ####
        // #### [--] Getters & Setters [--] ####


        // #### [++] Behaviour [++] ####
        protected override void eat(LivingEntity entity)
        {
            // TO DO
        }
        // #### [--] Behaviour [--] ####
    }

}
