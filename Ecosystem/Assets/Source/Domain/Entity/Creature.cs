using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using GeneticsDomain;

namespace EntityDomain
{
    public enum BrainState
    {
        Idle,
        Busy
    }
    public enum CreatureState 
    {   // to be moved in a separate file
        None,
        Thinking,
        Exploring,
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
        private GameObject _creatureObject = null;

        private float _time;
        private float _randomInterval;
        private BrainState _brainState;
        private CreatureState _state;

        private float _age;
        private CreatureGender _gender;

        // ---- [++] Genes [++] ----
        private Chromosome _chromosome;
        private float _geneMotorSpeed;
        private float _geneBrainSpeed;
        private float _geneSensorialSmell;
        private float _geneSensorialSight;
        private float _geneFoodPreference;
        // ---- [--] Genes [--] ----
        // #### [--] Attributes [--] ####


        // #### [++] Initialization [++] ####
        public Creature(int2 newCoordinates) : base(newCoordinates)
        {
            this._time = 0;
            this._randomInterval = randomizeInterval(EntityConfig.instance.UpdateIntervalOfCreatureBrain);
            
            initializeCreatureObject();
            initializeRandomGender();
            initializeRandomAge();
            initializeDefaultGenes();

            this._brainState = BrainState.Idle;
            this._state = CreatureState.None;

            Creature.creatureCounter++;
        }

        private void initializeCreatureObject()
        {
            this._creatureObject = new GameObject("[Creature" + Creature.creatureCounter + "]");
            this._creatureObject.AddComponent<SpriteRenderer>().sprite = Entity_AssetsService.instance.creature_default;
            this._creatureObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);      // black color
        }

        private void initializeRandomGender()
        {
            int gender = UnityEngine.Random.Range(0,1);
            if (gender == 0)
            {
                this._gender = CreatureGender.Male;
            }
            else if (gender == 1)
            {
                this._gender = CreatureGender.Female;
            }
        }
        private void initializeRandomAge()
        {
            this._age = (int)UnityEngine.Random.Range(10,20);
        }
        private void initializeDefaultGenes()
        {
            this._chromosome = new Chromosome();
                                                //               Energy consumption
                                                //     less       |    default   |    more

            this._geneMotorSpeed = 1.0f;        // -1.0 slowest   | 0.0 default  | 1.0 fastest
            this._geneBrainSpeed = 1.0f;        // -1.0 slowest   | 0.0 default  | 1.0 fastest
            this._geneSensorialSmell = 1.0f;    // -1.0 worst     | 0.0 default  | 1.0 best
            this._geneSensorialSight = 1.0f;    // -1.0 worst     | 0.0 default  | 1.0 best
            this._geneFoodPreference = 0.0f;    // -1.0 carnivore | 0.0 omnivore | 1.0 erbivore

            this._chromosome.addGene(this._geneMotorSpeed);         // 0 MOTOR SPEED
            this._chromosome.addGene(this._geneBrainSpeed);         // 1 BRAIN SPEED
            this._chromosome.addGene(this._geneSensorialSmell);     // 2 SMELL
            this._chromosome.addGene(this._geneSensorialSight);     // 3 SIGHT
            this._chromosome.addGene(this._geneFoodPreference);     // 4 FOOD PREFERENCE
        }
        // #### [--] Initialization [--] ####


        // #### [++] Brain [++] ####
        // might need some changing because im going to work with state pattern (switch-case)
        // this updating method might not wait for currently active action to finish -> find a method -> maybe actuallty states
        public void updateBrain()
        {
            executeEvery(this._randomInterval);
        }
        private void executeEvery(float seconds)
        {   // execute every given seconds
            this._time += Time.deltaTime;    // only increment if creature is idle

            if (this._time >= seconds)
            {   
                // put here what needs to be executed after given seconds
                this._time = 0;
                Debug.Log(this._creatureObject.name + " brain action after " + seconds + " seconds.");

                if (this._brainState == BrainState.Idle)
                {   
                    // creature is not doing anything
                    this._brainState = BrainState.Busy;   // Creature is doing something
                    this._randomInterval = randomizeInterval(EntityConfig.instance.UpdateIntervalOfCreatureBrain);
                    act();
                }
                else if (this._brainState == BrainState.Busy)
                {
                    // creature is doing something
                    // TO DO : check if attacked or if there are predators nearby
                }
            }
        }
        private float randomizeInterval(float seconds)
        {   
            // should add a modifier (genetics) to change the randomize interval to
            // make the creature "smarter" by thinking faster for each individual
            // return UnityEngine.Random.Range(seconds/2 - this._chromosome.getGenes()[1]/4, seconds*2 - this._chromosome.getGenes()[1]/4);
            return UnityEngine.Random.Range(seconds/2, seconds*2);
        }
        // #### [--] Brain [--] ####


        // #### [++] Getters & Setters [++] ####
        public CreatureGender getGender()
        {
            return this._gender;
        }
        public GameObject getObject()
        {
            return this._creatureObject;
        }
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
                case CreatureState.Exploring:
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
            this._brainState = BrainState.Idle; // action finished
        }

        protected override void eat(LivingEntity entity)
        {
            // TO DO
        }
        // #### [--] Behaviour [--] ####
    }

}
