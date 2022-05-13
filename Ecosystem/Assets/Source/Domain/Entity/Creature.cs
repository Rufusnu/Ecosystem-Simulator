using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using GeneticsDomain;
using GridDomain;
using GameConfigDomain;

namespace EntityDomain
{
    public class Creature : LivingEntity
    {
        // #### #### [++] Attributes [++] #### ####
        public static int creatureCounter = 0;
        //private GameObject _creatureObject = null;

        private float _time;
        private float _randomInterval;
        private BrainState _brainState;
        private CreatureState _physicalState;
        

        private float _age;
        private float _deathAge;
        private CreatureGender _gender;
        private Vector3 _moveDirection;
        private int2 _oldTargetFood;
        private LivingEntity _targetFood;
        private List<Entity> _visibleEntities;
        private Queue<int2> _path;

        // ---- [++] Genes [++] ----
        private Chromosome _chromosome;
        private float _geneMotorSpeed;
        private float _geneBrainSpeed;
        private float _geneSensorialSmell;
        private float _geneSensorialSight;
        private float _geneFoodPreference;
        private float _geneBehaviour;
        // ---- [--] Genes [--] ----

        private float _animationElapsedTime;
        private Vector3 _animationStartPos;
        private Vector3 _animationEndPos;
        private bool _isAnimating;
        // #### #### [--] Attributes [--] #### ####


        // #### #### [++] Initialization [++] #### ####
        public Creature(int2 newCoordinates) : base(newCoordinates)
        {
            this._animationElapsedTime = 0;
            this._randomInterval = randomizeInterval(EntityConfig.instance.UpdateIntervalOfCreatureBrain);
            this._time = UnityEngine.Random.Range(-this._randomInterval, this._randomInterval);
            
            initializePath();
            initializeStates();
            initializeMoveDirection();
            initializeRandomGender();
            initializeCreatureObject(this._gender);
            initializeAge();
            initializeDefaultGenes();

            Creature.creatureCounter++;
        }

        private void initializePath()
        {
            this._path = new Queue<int2>();
        }
        private void initializeStates()
        {
            this._physicalState = CreatureState.None;
            this._brainState = BrainState.Idle;
        }
        private void initializeMoveDirection()
        {
            this._moveDirection = new Vector3(UnityEngine.Random.Range(-1,1), UnityEngine.Random.Range(-1,1), 0);
        }
        private void initializeCreatureObject()
        {
            /*this._creatureObject = new GameObject("[Creature" + Creature.creatureCounter + "]");
            this._creatureObject.AddComponent<SpriteRenderer>().sprite = Entity_AssetsService.instance.creature_default;
            this._creatureObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1.0f);      // black color
            */
            this.setObject(new GameObject("[Creature" + Creature.creatureCounter + "]"));
            this.getObject().AddComponent<SpriteRenderer>().sprite = Entity_AssetsService.instance.creature_default;
            this.getObject().GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1.0f);      // black color
        }
        private void initializeCreatureObject(CreatureGender gender)
        {
            this.setObject(new GameObject("[Creature" + Creature.creatureCounter + "]"));
            if (gender == CreatureGender.Male)
            {
                this.getObject().AddComponent<SpriteRenderer>().sprite = Entity_AssetsService.instance.creature_male;
            }
            else    // Female
            {
                this.getObject().AddComponent<SpriteRenderer>().sprite = Entity_AssetsService.instance.creature_female;
            }
            this.getObject().GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1.0f);      // black color
        }
        private void initializeRandomGender()
        {
            int gender = UnityEngine.Random.Range(0,2);
            if (gender == 0)
            {
                this._gender = CreatureGender.Male;
            }
            else if (gender == 1)
            {
                this._gender = CreatureGender.Female;
            }
        }
        private void initializeAge()
        {
            this._age = 0.1f;
        }
        private void initializeDeathAge()
        {
            float min = EntityConfig.instance.minDeathAge;
            float max = EntityConfig.instance.maxDeathAge;

            this._deathAge = UnityEngine.Random.Range(min, max);
        }
        public void setRandomAge()
        {
            this._age = (int)UnityEngine.Random.Range(10,20);
        }
        private void initializeDefaultGenes()
        {
            this._chromosome = new Chromosome();
                                                //               Energy consumption
                                                //     less       |    default   |    more

            this._geneMotorSpeed = 0.0f;        // -1.0 slowest   | 0.0 default  | 1.0 fastest
            this._geneBrainSpeed = 0.0f;        // -1.0 slowest   | 0.0 default  | 1.0 fastest
            this._geneSensorialSmell = 0.0f;    // -1.0 worst     | 0.0 default  | 1.0 best
            this._geneSensorialSight = 0.0f;    // -1.0 worst     | 0.0 default  | 1.0 best
            this._geneFoodPreference = 0.0f;    // -1.0 carnivore | 0.0 omnivore | 1.0 erbivore
            this._geneBehaviour = 0.0f;         // -1.0 bad       | 0.0 both     | 1.0 good

            this._chromosome.addGene(this._geneMotorSpeed);         // 0 MOTOR SPEED
            this._chromosome.addGene(this._geneBrainSpeed);         // 1 BRAIN SPEED
            this._chromosome.addGene(this._geneSensorialSmell);     // 2 SMELL
            this._chromosome.addGene(this._geneSensorialSight);     // 3 SIGHT
            this._chromosome.addGene(this._geneFoodPreference);     // 4 FOOD PREFERENCE
            this._chromosome.addGene(this._geneBehaviour);          // 5 BEHAVIOUR
        }
        // #### #### [--] Initialization [--] #### ####


        // #### #### [++] Brain [++] #### ####
        // might need some changing because im going to work with state pattern (switch-case)
        // this updating method might not wait for currently active action to finish -> find a method -> maybe actuallty states
        public void updateBrain()
        {
            if (this.isAlive())
            {
                /*if (isOld())
                {
                    // TO DO
                    this.die();
                    return;
                }*/
                if (this._isAnimating)
                {
                    animate();
                }
                else
                {
                    executeEvery(this._randomInterval);
                }
            }
        }
        private void toBeExecutedEvery()
        {
            // put here what needs to be executed after given seconds
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

        private void executeEvery(float seconds)
        {   // execute every given seconds
            this._time += Time.deltaTime;    // only increment if creature is idle

            if (this._time >= seconds)
            {
                this._time = 0;
                if (GameConfig.instance.Debugging)
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
            return UnityEngine.Random.Range(seconds/1.35f - this._geneBrainSpeed*(0.9f) * seconds/1.35f, seconds*1.35f - this._geneBrainSpeed*(0.9f) * seconds *1.35f);
        }
        // #### #### [--] Brain [--] #### ####


        // #### #### [++] Getters & Setters [++] #### ####
        public CreatureGender getGender()
        {
            return this._gender;
        }
        public int getSightDistance()
        {
            return (int)(this._geneSensorialSight + EntityConfig.instance.SightDistanceInCells); // gene modifier * default sight distance
        }

        /*public float getMoveDuration()
        {
            return EntityConfig.instance.MoveDuration + this._geneMotorSpeed/1.25f;
        }*/

        public Vector3 getMoveDirection()
        {
            return this._moveDirection;
        }
        public void setMoveDirection(Vector3 newMoveDirection)
        {
            this._moveDirection = newMoveDirection;
        }
        // #### #### [--] Getters & Setters [--] #### ####


        // #### #### [++] Overrides [++] #### ####
        // ---- [++] Operators [++] ----
        public static bool operator ==(Creature creature1, Creature creature2)
        {
            if (!creature1.getCoordinates().Equals(creature2.getCoordinates()))
                return false;
            if (creature1._age != creature2._age)
                return false;
            if (creature1._gender != creature2._gender)
                return false;
            if (creature1._geneBehaviour != creature2._geneBehaviour)
                return false;
            if (creature1._geneBrainSpeed != creature2._geneBrainSpeed)
                return false;
            if (creature1._geneFoodPreference != creature2._geneFoodPreference)
                return false;
            if (creature1._geneMotorSpeed != creature2._geneFoodPreference)
                return false;
            if (creature1._geneSensorialSight != creature2._geneSensorialSight)
                return false;
            if (creature1._geneSensorialSmell != creature2._geneSensorialSmell)
                return false;
            
            return true;
        }
        public static bool operator !=(Creature creature1, Creature creature2)
        {
            return !(creature1 == creature2);
        }
        public bool Equals(Creature creature)
        {
            return this == creature;
        }
        public override bool Equals(object o)
        {
            if(o == null)
                return false;

            var creature = o as Creature;

            return creature != null && Equals(creature);
        }
        public override int GetHashCode()
        {
            return 0;
        }
        // ---- [--] Operators [--] ----

        protected override void eat(LivingEntity entity)
        {
            float kcal = entity.getNutrition();
            float energy = Energy.EnergySystem.kcalToEnergy(kcal);
            this.modifyEnergyBy(energy);
            entity.eaten();
        }
        public override void eaten()
        {
            this.die();
        }
        protected override void initializeNutritionValue()
        {
            // value [-20%; +20%] * Average Nutrition Value (Default value from entity config)
            float min = EntityConfig.instance.CreatureMinNutritionValue;
            float max = EntityConfig.instance.CreatureMaxNutritionValue;

            this._nutritionValue = (float)UnityEngine.Random.Range(min, max);
        }
        // #### #### [--] Overrides [--] #### ####


        // #### #### [++] Behaviour [++] #### ####
        protected void act()
        {
            if (GameConfig.instance.Debugging)
            {
                Debug.Log(this.getObject().name + " state: " + this._physicalState.ToString());
            }

            switch(this._physicalState)
            {
                case CreatureState.None:
                    this._physicalState = CreatureState.Thinking;
                    break;
                case CreatureState.Thinking:
                    // checks what does it need
                    if (this.getEnergy() < 0.78f)
                    {   
                        // check for food
                        bool foundFood = senseFood();
                        if (foundFood)
                        {   
                            if (foodIsClose())
                            {
                                eat(this._targetFood);
                            }
                            getPathTowards(this._targetFood.getCoordinates());
                            this._physicalState = CreatureState.GoingToEat;
                        }
                        else
                        {   // does not sense food => explores
                            this._physicalState = CreatureState.Exploring;
                        }
                        consumeEnergy(CreatureActions.Think);
                    }
                    else
                    {
                        // if food ok, search for others
                        
                    }
                    break;
                case CreatureState.Exploring:
                    // moves one cell then resets to thinking
                    explore();
                    consumeEnergy(CreatureActions.Move);
                    
                    this._physicalState = CreatureState.Thinking;
                    break;
                case CreatureState.GoingToEat:
                    // moves to food one cell then resets to thinking
                    int2 nextCell = new int2(-1, -1);
                    if (this._path.Count > 0)
                    {
                        nextCell = this._path.Dequeue();
                        if (GridMap.currentGridInstance.isCellFree(nextCell))
                        {
                            moveTo(nextCell);
                        }
                        else
                        {
                            this._physicalState = CreatureState.Thinking;
                        }
                    }
                    else
                    {
                        this._physicalState = CreatureState.Thinking;
                    }
                    
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


        // ---- [++] Senses [++] ----
        private bool foodIsClose()
        {
            if (EcoMath.Math.distanceBetween(this, this._targetFood) == 1)
            {
                return true;
            }
            return false;
        }
        private bool senseFood()
        {
            LivingEntity food = findFood();
            if (food.getCoordinates().Equals(new int2(-1, -1)))
                return false;
            this._targetFood = food;
            return true;
        }
        private LivingEntity findFood()
        {
            updateSight();
            updateSmell();
            return checkSensesForFood();
        }

        private void updateSight()
        {
            this._visibleEntities = GridMap.currentGridInstance.getVisibleEntities(this);
        }
        private void updateSmell()
        {
            // TO DO
        }

        private LivingEntity checkSensesForFood()
        {
            float minDistanceCreature = Mathf.Infinity;
            float minDistancePlant = Mathf.Infinity;
            LivingEntity target = new EntityDomain.NullLivingEntity();

            foreach(LivingEntity livingEntity in this._visibleEntities)
            {
                if (livingEntity.GetType() == typeof(Creature))
                {
                    float distance = EcoMath.Math.distanceBetween(this, livingEntity);
                    if (distance < minDistanceCreature)
                    {
                        minDistanceCreature = distance;
                    }
                }
                else if (livingEntity.GetType() == typeof(Plant)) // TO DO
                {
                    float distance = EcoMath.Math.distanceBetween(this, livingEntity);
                    if (distance < minDistancePlant)
                    {
                        minDistancePlant = distance;
                        target = livingEntity;
                    }
                }
            }
            
            return target;
        }
        // ---- [--] Senses [--] ----

        private void explore()
        {   
            GridMap.currentGridInstance.moveCreatureRandomly(this.getCoordinates(), this._moveDirection);
            startAnimateTo(this.getCoordinates());
        }
        private void moveTo(int2 targetCoordinates)
        {
            GridMap.currentGridInstance.moveCreatureTo(this.getCoordinates(), targetCoordinates);
            startAnimateTo(this.getCoordinates());
        }
        // #### #### [--] Behaviour [--] #### ####


        private void getPathTowards(int2 targetCoordinates)
        {
           this._path = new Queue<int2>(GridMap.currentGridInstance.pathTo(this.getCoordinates(), targetCoordinates));
        }

        private void startAnimateTo(int2 coordinates)
        {
            this._animationStartPos = this.getObject().transform.localPosition;
            this._animationEndPos = gridToWorldCoordinates(coordinates);
            this._isAnimating = true;
        }
        private void animate()
        {
            int2 worldToGridCoords = GridMap.currentGridInstance.worldToGridCoordinates(this.getObject().transform.localPosition);
            
            // animating
            animateMovement();
            // finished animating
            if (this._animationElapsedTime >= 1)
            {
                this._time = 0; // reset brain timer
                this._isAnimating = false;
                this._animationElapsedTime = 0;
            }
        }
        private void animateMovement()
        {
            this._animationElapsedTime = Mathf.Min(1, this._animationElapsedTime + Time.deltaTime * EntityConfig.instance.MoveSpeed + Time.deltaTime * this._geneMotorSpeed * 1.5f);
            this.getObject().transform.localPosition = Vector3.Lerp(this._animationStartPos, this._animationEndPos, this._animationElapsedTime);
        }

        public Vector3 gridToWorldCoordinates()
        {
            return new Vector3(this.getCoordinates().x * GameConfigDomain.GameConfig.instance.GridCellSize, this.getCoordinates().y * GameConfigDomain.GameConfig.instance.GridCellSize, this.getObject().transform.localPosition.z);
        }
        public Vector3 gridToWorldCoordinates(int2 coordinates)
        {
            return new Vector3(coordinates.x * GameConfigDomain.GameConfig.instance.GridCellSize, coordinates.y * GameConfigDomain.GameConfig.instance.GridCellSize, this.getObject().transform.localPosition.z);
        }
        private bool isOld()
        {
            if (this._age > this._deathAge)
            {
                return true;
            }
            return false;
        }
    }

}
