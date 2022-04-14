using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace EntityDomain
{
    public enum CreatureState 
    {   // to be moved in a separate file
        None,
        Thinking,
        Searching,
        GoingToEat,
        Eating,
        GoingToDrink,
        Drinking,
        Hunting,
        Fleeing,
        GoingToMate,
        Mating
    }

    public class Creature : LivingEntity
    {
        // #### [++] Attributes [++] ####
        public static int creatureCounter = 0;
        const int NumberOfGenes = 0; // number of attributes that can be mutated
        private float _time;
        private float _randomInterval;
        protected CreatureState _state;
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
        protected void act()
        {
            switch(this._state)
            {
                case CreatureState.None:
                    this._state = CreatureState.Thinking;
                    break;
                case CreatureState.Thinking:
                    // checks what does it need
                    break;
                case CreatureState.Searching:
                    // moves one cell then resets to thinking
                    break;
                case CreatureState.GoingToEat:
                    // moves to food one cell then resets to thinking
                    break;
                case CreatureState.Eating:
                    // eats food then resets to thinking
                    break;
                case CreatureState.GoingToDrink:
                    // moves to water one cell then resets to thinking
                    break;
                case CreatureState.Drinking:
                    // drinks then resets to thinking
                    break;
                case CreatureState.Hunting:
                    // moves towards pray one cell then resets to thinking
                    break;
                case CreatureState.Fleeing:
                    // runs away from hunter one cell then resets to thinking
                    break;
                case CreatureState.GoingToMate:
                    // moves towards mate one cell then resets to thinking
                    break;
                case CreatureState.Mating:
                    // mates then resets to thinking
                    break;
            }
        }

        protected override void eat(LivingEntity entity)
        {
            // TO DO
        }
        // #### [--] Behaviour [--] ####
    }

}
