using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using GeneticsDomain;
using GridDomain;
using SmellDomain;

namespace EntityDomain
{
    public class Creature : LivingEntity
    {
        // #### #### [++] Attributes [++] #### ####
        public static int creatureCounter = 0;
        //private GameObject _creatureObject = null;

        private float _randomInterval;
        private float _smellPlaceInterval;
        protected BrainState _brainState;
        protected CreatureState _physicalState;
        private bool _isInitializing = true;
        protected int id;
        protected float _age = 30;
        protected float _deathAge = 60;
        protected CreatureGender _gender;
        protected Vector3 _moveDirection;
        private int2[] _moveDirections = {new int2(0,-1), new int2(-1,0), new int2(0,1), new int2(1,0)};
        protected int2 _currentMoveDir;
        private LivingEntity _targetFood;
        private float _foodPreferenceModifier; // positive number for herbivores | negative for carnivores
        private Creature _targetMate;
        //private Entity _targetSmell;
        private float _urgeToMate;
        private float _matingCooldown;
        private Queue<Creature> _rejectedBy;
        private List<Entity> _visibleEntities;
        private List<SmellNode> _sensedSmells;
        private Queue<int2> _path;

        // ---- [++] Genes [++] ----
        protected LivingEntity _father;
        protected LivingEntity _mother;
        protected float _geneMotorSpeed;
        protected float _geneBrainSpeed;
        protected float _geneSensorialSmell;
        protected float _geneSensorialSight;
        protected float _geneFoodPreference;
        protected float _geneBehaviour;

        protected SmellNode _smell = null;
        protected float _time_smell;
        // ---- [--] Genes [--] ----

        protected Vector3 _animationStartPos;
        protected Vector3 _animationEndPos;
        protected bool _isAnimatingMovement;
        protected bool _isAnimatingDeath;
        protected float _animationElapsedTime;
        protected float _eatingTime;
        // #### #### [--] Attributes [--] #### ####


        // #### #### [++] Initialization [++] #### ####
        public Creature(int2 newCoordinates) : base(newCoordinates)
        {
            this._animationElapsedTime = 0;
            this._randomInterval = randomizeInterval(Configs.BrainUpdateInterval_Creature());
            this._smellPlaceInterval = Configs.SmellPlaceInterval();
            this._time = UnityEngine.Random.Range(-this._randomInterval, this._randomInterval);
            this._time_smell = 0;
            
            initializeTargets();
            initializePath();
            initializeStates();
            initializeMoveDirection();
            initializeRandomGender();
            setRandomAge();
            initializeDeathAge();
            initializeDefaultGenes();
            initializeCreatureObject(this._gender);
            updateObjectColor();
            initializeRandomUrge();
            initializeVisibleEntities();
            updateSize();

            Creature.creatureCounter++;
            this.id = creatureCounter;
        }
        public Creature(Creature mother, Creature father) : this(mother.getCoordinates())
        {
            initializeInheritedGenes(mother, father);
            initializeAge();
            initializeChildScale();
            initializeRandomUrge();
            updateObjectColor();    // re-update the colors since the genes changes
        }

        private void initializeSmell()
        {
            if (this._smell == null)
            {
                this._smell = new SmellNode(this);
            }
            if (this._smell == null)
            {
                throw new System.Exception("NULL SMELL CREATION");
            }
        }

        private void initializeTargets()
        {
            //this._targetFood = new NullLivingEntity();
            //this._targetMate = new NullCreature();
            this._rejectedBy = new Queue<Creature>();
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
            this._currentMoveDir = this._moveDirections[UnityEngine.Random.Range(0,4)];
        }
        private void initializeCreatureObject()
        {
            /*this._creatureObject = new GameObject("[Creature" + Creature.creatureCounter + "]");
            this._creatureObject.AddComponent<SpriteRenderer>().sprite = Entity_AssetsService.instance.creature_default;
            this._creatureObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1.0f);      // gray color
            */
            this.setObject(new GameObject("[Creature" + Creature.creatureCounter + "]"));
            this.getObject().AddComponent<SpriteRenderer>().sprite = Entity_AssetsService.instance.creature_default;
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
        }
        private void updateObjectColor()
        {
            //  Sprite color is gene dependant
            float yellow = (21 * this._geneMotorSpeed + 21 * this._geneBrainSpeed)/255; // fast
            float blue = (-1)*(21 * this._geneMotorSpeed + 21 * this._geneBrainSpeed)/255; // slow

            float magenta = (-1)*(21 * this._geneFoodPreference + 21 * this._geneBehaviour)/255; // carnivore
            float green = (21 * this._geneFoodPreference + 21 * this._geneBehaviour)/255;; // herbivore

            float cyan = (-1)*(21 * this._geneSensorialSight + 21 * this._geneSensorialSmell)/255; // bad senses
            float red = (21 * this._geneSensorialSight + 21 * this._geneSensorialSmell)/255; // good senses
            this.getObject().GetComponent<SpriteRenderer>().color = new Color(0.5f + yellow + magenta + red, 0.5f + yellow + green + cyan, 0.5f + blue + magenta + cyan, 1.0f);      // black color
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
            float min = Configs.DeathAgeMin();
            float max = Configs.DeathAgeMax();

            this._deathAge = UnityEngine.Random.Range(min, max);
        }
        public void setRandomAge()
        {
            this._age = (int)UnityEngine.Random.Range(19,40);
        }
        private void initializeDefaultGenes()
        {
            this._father = new NullLivingEntity();
            this._mother = new NullLivingEntity();
            this._chromosome = new Chromosome();
                                                                                //               Energy consumption
                                                                                //     less       |    default   |    more

            this._geneMotorSpeed = UnityEngine.Random.Range(-1f, 1f);        // -1.0 slowest   | 0.0 default  | 1.0 fastest
            this._geneBrainSpeed = UnityEngine.Random.Range(-1f, 1f);        // -1.0 slowest   | 0.0 default  | 1.0 fastest
            this._geneSensorialSmell = UnityEngine.Random.Range(-1f, 1f);    // -1.0 worst     | 0.0 default  | 1.0 best
            this._geneSensorialSight = UnityEngine.Random.Range(-1f, 1f);    // -1.0 worst     | 0.0 default  | 1.0 best
            this._geneFoodPreference = UnityEngine.Random.Range(-1f, 1f);    // -1.0 carnivore | 0.0 omnivore | 1.0 erbivore
            this._geneBehaviour = UnityEngine.Random.Range(-1f, 1f);         // -1.0 bad       | 0.0 both     | 1.0 good

            this._chromosome.addGene(this._geneMotorSpeed);         // 0 MOTOR SPEED
            this._chromosome.addGene(this._geneBrainSpeed);         // 1 BRAIN SPEED
            this._chromosome.addGene(this._geneSensorialSmell);     // 2 SMELL
            this._chromosome.addGene(this._geneSensorialSight);     // 3 SIGHT
            this._chromosome.addGene(this._geneFoodPreference);     // 4 FOOD PREFERENCE
            this._chromosome.addGene(this._geneBehaviour);          // 5 BEHAVIOUR

            this._foodPreferenceModifier = this._geneFoodPreference * 100;
        }

        private void initializeInheritedGenes(Creature mother, Creature father)
        {
            this._mother = mother;
            this._father = father;
            this._chromosome.inheritGenes(mother.getChromosome(), father.getChromosome());

            this._foodPreferenceModifier = this._geneFoodPreference * 100;
        }
        private void initializeRandomUrge()
        {
            this._urgeToMate = UnityEngine.Random.Range(0.0f, 1.0f);
        }

        private void initializeVisibleEntities()
        {
            this._visibleEntities = new List<Entity>();
        }

        private void initializeChildScale()
        {
            this.getObject().transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        // #### #### [--] Initialization [--] #### ####


        // #### #### [++] Brain [++] #### ####
        // might need some changing because im going to work with state pattern (switch-case)
        // this updating method might not wait for currently active action to finish -> find a method -> maybe actuallty states
        public override void updateBrain()
        {
            if (this._isInitializing)
            {
                initializeSmell();
                this._isInitializing = false;
            }
            if (this.isAlive())
            {
                if (isOld())
                {
                    this.die();
                    return;
                }
                if (this._isAnimatingMovement)
                {
                    animate();
                }
                else
                {
                    placeSmell();
                    executeActEvery(this._randomInterval);
                }
                clocks();
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
                act();
            }
            else if (this._brainState == BrainState.Busy)
            {
                // creature is doing something
                // TO DO : check if attacked or if there are predators nearby
            }
        }

        private void executeActEvery(float seconds)
        {   
            // execute every given seconds
            this._time += Time.deltaTime;

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
        private void clocks()
        {
            // execute every given seconds
            this._time_smell += Time.deltaTime;
            this._matingCooldown += Time.deltaTime;
        }

        private float randomizeInterval(float seconds)
        {   
            // should add a modifier (genetics) to change the randomize interval to
            // make the creature "smarter" by thinking faster for each individual
            // return UnityEngine.Random.Range(seconds/2 - this._chromosome.getGenes()[1]/4, seconds*2 - this._chromosome.getGenes()[1]/4);
            return UnityEngine.Random.Range(seconds/1.35f - this._geneBrainSpeed*(0.9f) * seconds/1.35f, seconds*1.35f - this._geneBrainSpeed*(0.9f) * seconds *1.35f);
        }

        public override void updateStats()
        {
            // updated by energy system every few seconds
            updateUrgeToMate();
            updateAge();
        }
        private void updateUrgeToMate()
        {
            if (isMature())
            {
                if (this.getEnergy() > 0.6f)
                {
                    if(this._urgeToMate > 1)
                    {
                        this._urgeToMate = 1;
                    }
                    this._urgeToMate += 0.03f;
                }
            }
        }
        private void updateAge()
        {
            if (this._age == 0.1f)
            {
                this._age = 0.5f;
            }
            this._age += 0.5f;
            if (this._age % 2 == 0)
            {
                updateSize();
            }
        }

        private void updateSize()
        {   
            if (this._age < 18)
            {
                float minScale = 0.2f;
                float maxScale = 1.0f;
                float birthAge = 0.1f;
                float matureAge = 18;

                float newScale = (minScale - maxScale) * (this._age - matureAge) / (birthAge - matureAge) + maxScale;
                this.getObject().transform.localScale = new Vector3(newScale, newScale, newScale);
            }
            else if (this._age == 18)
            {
                this.getObject().transform.localScale = new Vector3(1, 1, 1);
            }
        }
        // #### #### [--] Brain [--] #### ####


        // #### #### [++] Getters & Setters [++] #### ####
        public CreatureGender getGender()
        {
            return this._gender;
        }

        // ---- [++] Genes [++] ----
        public float getGene_MotorSpeed()
        {
            return this._geneMotorSpeed;
        }
        public float getGene_BrainSpeed()
        {
            return this._geneSensorialSmell;
        }
        public int getGene_SightDistance()
        {
            return (int)(this._geneSensorialSight + Configs.SightDistance()); // gene modifier * default sight distance
                                                                              // lowest is better (can sense lower intensities)
        }
        public float getGene_SensorialSmell()
        {
            return ((-1)*(Configs.SmellDefaultSense() * this._geneSensorialSmell) + Configs.SmellDefaultSense())/2; // formula returns a value between [0; 2*SmellDefaultSense]/2
        }
        public float getGene_FoodPreference()
        {
            return this._geneFoodPreference;
        }
        public float getGene_Behaviour()
        {
            return this._geneBehaviour;
        }

        public override int getSmellIntensity()
        {
            if ((int)((this._geneFoodPreference + 1)*2 + (-1)*(this._geneSensorialSight - 1)*0.5f + (-1)*(this._geneSensorialSmell - 1) + (-1)*(this._geneBrainSpeed - 1)*0.5f + (-1)*(this._geneMotorSpeed - 1)*2 + (-1)*(this._geneBehaviour - 1)*0.5f)*2 <= 0)
            {
                return 1;
            }
            return (int)((this._geneFoodPreference + 1)*2 + (-1)*(this._geneSensorialSight - 1)*0.5f + (-1)*(this._geneSensorialSmell - 1) + (-1)*(this._geneBrainSpeed - 1)*0.5f + (-1)*(this._geneMotorSpeed - 1)*2 + (-1)*(this._geneBehaviour - 1)*0.5f)*2;
        }
        public void deleteSmellReference()
        {
            this._smell = null;
        }
        // ---- [--] Genes [--] ----
        

        /*public float getMoveDuration()
        {
            return EntityConfig.instance.MoveDuration + this._geneMotorSpeed/1.25f;
        }*/

        public int2 getMoveDirection()
        {
            return this._currentMoveDir;
        }
        public virtual void setMoveDirection(int2 currentMoveDir)
        {
            this._currentMoveDir = currentMoveDir;
        }
        // #### #### [--] Getters & Setters [--] #### ####


        // #### #### [++] Overrides [++] #### ####
        // ---- [++] Operators [++] ----
        /*public static bool operator ==(Creature creature1, Creature creature2)
        {
            /*if (!creature1.getCoordinates().Equals(creature2.getCoordinates()))
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
                return false;*/
            /*if (creature1.id != creature2.id)
            {
                return false;
            }
            
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
        }*/
        // ---- [--] Operators [--] ----

        protected override void eat(LivingEntity entity)
        {
            if (entity != null)
            {
                if (entity.GetType() == typeof(Creature))
                {
                    if (isSimilar((Creature)entity))
                    {
                        return;
                    }
                }
                
                float kcal = entity.getNutrition();
                if (entity.GetType() == typeof(Creature))
                {
                    kcal -= this._foodPreferenceModifier;
                }
                else if (entity.GetType() == typeof(Plant))
                {
                    kcal += this._foodPreferenceModifier;
                }


                animateEating(entity);
                
                float energy = Energy.EnergySystem.kcalToEnergy(kcal);
                this.modifyEnergyBy(energy);
                this._urgeToMate += energy/5;
                entity.eatenBy(this);
                //this._targetFood = new NullLivingEntity();
            }
        }
        public override void eatenBy(LivingEntity entity)
        {
            this.die();
        }
        
        public void mateWith(Creature female)
        {
            // function called only within males
            if (female != null && female.isMature() && this.isMature())
            {
                // male behaviour
                this._urgeToMate = 0;
                this._matingCooldown = 0;
                female.getPregnantWith(this);
                //animateMating(mate);
            }
            consumeEnergy(CreatureActions.Mate);
        }
        private void getPregnantWith(Creature male)
        {
            // function called only within females
            if (isMature())    // mature
            {
                // cant get pregnant if not mature enough
                if (male != null)
                {
                    // female behaviour
                    // get pregnant / give birth
                    this._urgeToMate = 0;
                    this._matingCooldown = 0;
                    giveBirthWith(male);
                }
            }
            consumeEnergy(CreatureActions.Mate);
        }
        private void giveBirthWith(Creature father)
        {
            // function called only within females
            int numberOfChildren = UnityEngine.Random.Range(1, 6); // 6 10
            List<Creature> children = new List<Creature>();

            for(int child = 1; child <= numberOfChildren; child++)
            {
                children.Add(new Creature(this, father));
            }
            GridMap.currentGridInstance.addBornCreatures(children);
        }

        private void handleRejection()
        {
            this._rejectedBy.Enqueue(this._targetMate);
            if (this._rejectedBy.Count > 2)
            {
                this._rejectedBy.Dequeue();
            }
        }

        protected override void initializeNutritionValue()
        {
            // value [-20%; +20%] * Average Nutrition Value (Default value from entity config)
            float min = Configs.NutritionValueMin_Creature();
            float max = Configs.NutritionValueMax_Creature();

            this._nutritionValue = (float)UnityEngine.Random.Range(min, max);
        }
        // #### #### [--] Overrides [--] #### ####


        // #### #### [++] Behaviour [++] #### ####
        protected void act()
        {
            if (Configs.Debugging())
            {
                Debug.Log(this.getObject().name + " age: " + this._age + "; state: " + this._physicalState.ToString()+ "; mateMetter: " + this._urgeToMate);
            }
            int2 nextCell;
            switch(this._physicalState)
            {
                case CreatureState.None:
                    this._physicalState = CreatureState.Thinking;
                    break;
                case CreatureState.Thinking:
                    // checks what does it need
                    if (this.getEnergy() < 0.6f)
                    {   
                        tryToEat();
                    }
                    else
                    {
                        // if food ok, search for other needs
                        if (this._gender == CreatureGender.Male)
                        {
                            if (this._urgeToMate > 0.8f && this._matingCooldown > 5.0f)
                            {
                                tryToMate();     
                            }
                            else
                            {
                                tryToEat();
                            }
                        }
                        else
                        {
                            tryToEat();
                        }
                    }
                    consumeEnergy(CreatureActions.Think, this._geneBrainSpeed);
                    break;
                case CreatureState.Exploring:
                    // moves one cell then resets to thinking
                    // obtains the next cell randomly from the gridMap and then its class 
                    // coordinates are moved to that cell
                    // (only after that, the creature's GameObject will move towards
                    //  the class's coordinates)
                    explore();
                    consumeEnergy(CreatureActions.Move, this._geneMotorSpeed);
                    
                    this._physicalState = CreatureState.Thinking;
                    break;
                case CreatureState.GoingToEat:
                    // if it has obtained a path, moves through the queue data structure
                    // and stops either if arrived at food OR if something blocks the path.
                    // The latter is the case in which it searches for another path to
                    // go around the obstacle
                    nextCell = new int2(-1, -1);
                    if (foodIsClose())
                    {
                        eat(this._targetFood);
                        //this._physicalState = CreatureState.Eating;
                        this._physicalState = CreatureState.Thinking;
                        break;      // stop moving through the path
                    }

                    if (this._path.Count > 0)
                    {
                        nextCell = this._path.Dequeue();
                        if (GridMap.currentGridInstance.isCellFree(nextCell))
                        {
                            moveTo(nextCell);
                            consumeEnergy(CreatureActions.Move, this._geneMotorSpeed);
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
                    eat(this._targetFood);
                    this._physicalState = CreatureState.Thinking;
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
                    // runs away from hunter then resets to thinking
                    // obtains a path (2-4 cells) that points in the oposite direction of the hunter
                    // OR
                    // calculates from which direction does the hunter come from and then
                    // starts to move in the oposite direction by transforming that direction
                    // into the 4 directions
                    break;
                case CreatureState.GoingToMate:
                    // moves towards mate one cell then resets to thinking
                    nextCell = new int2(-1, -1);

                    if (mateIsClose() && this._gender == CreatureGender.Male)
                    {
                        mateWith(this._targetMate);
                        this._physicalState = CreatureState.Thinking;
                        break;      // stop moving through the path
                    }

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
                case CreatureState.Mating:
                    // mates then resets to thinking
                    break;
            }
            this._brainState = BrainState.Idle; // action finished
        }


        // ---- [++] Senses [++] ----
        private void tryToEat()
        {
            // check for food
            bool foundFood = senseFood();
            if (foundFood)
            {   
                getPathTowards(this._targetFood.getCoordinates());
                this._physicalState = CreatureState.GoingToEat;
            }
            else
            {   // does not sense food => explores
                this._physicalState = CreatureState.Exploring;
            }
        }
        private void tryToMate()
        {
            // if male, search for females
            bool foundMate = senseMate();
            if (foundMate)
            {
                if (this._targetMate.willingToMate())
                {
                    this._targetMate.getTriggeredByMate(this);
                    // start own behaviour
                    getPathTowards(this._targetMate.getCoordinates());
                    this._physicalState = CreatureState.GoingToMate;
                }
                else
                {
                    handleRejection();
                }
            }
            else
            {   // does not sense food => explores
                this._physicalState = CreatureState.Exploring;
            }    
        }

        // [++] Surroundings [++]
        private bool foodIsClose()
        {
            if (this._targetFood != null)
            {
                if (EcoMath.Math.distanceBetween(this, this._targetFood) <= 1)
                {
                    return true;
                }
            }
            return false;
        }
        private bool mateIsClose()
        {
            if (this._targetMate != null)
            {
                if (EcoMath.Math.distanceBetween(this, this._targetMate) <= 1)
                {
                    return true;
                }
            }
            return false;
        }
        // [--] Surroundings [--]


        private bool senseFood()
        {
            LivingEntity food = findFood();

            // check if returned food is valid
            if (food.getCoordinates().Equals(new int2(-1, -1)) || food.GetType() == typeof(NullLivingEntity))
                return false;

            this._targetFood = food;
            return true;
        }
        private bool senseMate()
        {
            // returns true if found a valid FEMALE mate
            Creature mate = findMate();
            if (mate == null)
            {
                return false;
            }

            // check if returned mate is valid
            if (mate.getCoordinates().Equals(new int2(-1, -1)))
                return false;

            this._targetMate = mate;
            return true;
        }

        private LivingEntity findFood()
        {
            updateSenses();
            return checkSensesForFood();
        }
        private Creature findMate()
        {
            updateSenses();
            return checkSensesForMate();
        }

        // [++] Update [++]
        private void updateSenses()
        {
            updateSight();
            updateSmell();
        }
        private void updateSight()
        {
            this._visibleEntities = GridMap.currentGridInstance.getVisibleEntities(this);
        }
        private void updateSmell()
        {
            this._sensedSmells = GridMap.currentGridInstance.getSensedSmells(this);
        }
        // [--] Update [--]


        // [++] Checking [++]
        private LivingEntity checkSensesForFood()
        {
            LivingEntity targetCreature_Sight = new EntityDomain.NullLivingEntity();
            LivingEntity targetPlant_Sight = new EntityDomain.NullLivingEntity();

            float distance;

            // [++] Sight [++]
            float minDistanceCreature_Sight = Configs.GridColumns();
            float minDistancePlant_Sight = Configs.GridColumns();

            foreach(LivingEntity livingEntity in this._visibleEntities)
            {
                if (livingEntity.GetType() == typeof(Creature) && this.isMature())
                {
                    Creature creature = (Creature)livingEntity;
                    if (!isSimilar(creature) && !isChildOf(this))
                    {
                        // eat only if not similar
                        distance = EcoMath.Math.distanceBetween(this, livingEntity);
                        if (distance < minDistanceCreature_Sight)
                        {
                            minDistanceCreature_Sight = distance;
                            targetCreature_Sight = livingEntity;
                        }
                    }
                }
                else if (livingEntity.GetType() == typeof(Plant))
                {
                    distance = EcoMath.Math.distanceBetween(this, livingEntity);
                    if (distance < minDistancePlant_Sight)
                    {
                        minDistancePlant_Sight = distance;
                        targetPlant_Sight = livingEntity;
                    }
                }
            }
            // [--] Sight [--]

            // [++] Smell [++]
            LivingEntity targetCreature_Smell = new EntityDomain.NullLivingEntity();
            LivingEntity targetPlant_Smell = new EntityDomain.NullLivingEntity();
            
            float minDistanceCreature_Smell = Configs.GridColumns();
            float minDistancePlant_Smell = Configs.GridColumns();

            foreach(SmellNode smell in this._sensedSmells)
            {
                if (smell.sourceType() == typeof(Creature) && this.isMature())
                {
                    Creature creature = (Creature)smell.source();
                    if (!isSimilar(creature) && !isChildOf(this))
                    {
                        // eat only if not similar
                        distance = EcoMath.Math.distanceBetween(this, smell) + smell.getDistanceToSource();
                        if (distance < minDistanceCreature_Smell)
                        {
                            minDistanceCreature_Smell = distance;
                            targetCreature_Smell = smell.source();
                        }
                    }
                }
                else if (smell.sourceType() == typeof(Plant))
                {
                    distance = EcoMath.Math.distanceBetween(this, smell) + smell.getDistanceToSource();
                    if (distance < minDistancePlant_Smell)
                    {
                        minDistancePlant_Smell = distance;
                        targetPlant_Smell = smell.source();
                    }
                }
            }
            // [--] Smell [--]

            /*if (this._geneFoodPreference < -0.8)
            {
                // true carnivore
                Debug.Log("EATING CREATURE");
                return targetCreature;
            }
            if (this._geneFoodPreference > 0.8)
            {
                // true herbivore
                return targetPlant; 
            }*/

            // [++] Choosing best target [++]

            // use the food preference in this function to determine what food the creatures will be wanting to eat more
            float distanceToCreature_Sight = minDistanceCreature_Sight - (-1) * this._geneFoodPreference * minDistanceCreature_Sight;            
            float distanceToPlant_Sight = minDistancePlant_Sight - this._geneFoodPreference * minDistancePlant_Sight;
            float distanceToCreature_Smell = minDistanceCreature_Smell - (-1) * this._geneFoodPreference * minDistanceCreature_Smell;            
            float distanceToPlant_Smell = minDistancePlant_Smell - this._geneFoodPreference * minDistancePlant_Smell;

            // comparing distances
            float preferedDistance = Configs.GridColumns();
            int preferedFood = -1;
            List<float2> distances = new List<float2>();

            if (Configs.CreatureSense_Sight_Creature())
            {
                distances.Add(new float2(distanceToCreature_Sight, 0));     // Creature Sight index  ==  0
            }
            if (Configs.CreatureSense_Sight_Plant())                  
            {
                distances.Add(new float2(distanceToPlant_Sight, 1));        // Plant Sight index     ==  1
            }
            if (Configs.CreatureSense_Smell_Creature())
            {
                distances.Add(new float2(distanceToCreature_Smell, 2));     // Creature Smell index  ==  2
            }
            if (Configs.CreatureSense_Smell_Plant())
            {
                distances.Add(new float2(distanceToPlant_Smell, 3));        // Plant Smell index     ==  3
            }

            // obtain the minimum distance
            for (int i = 0; i < distances.Count; i++)
            {
                if (preferedDistance > distances[i][0])
                {
                    preferedDistance = distances[i][0];
                    preferedFood = (int)distances[i][1];
                }
            }
            
            switch (preferedFood) 
            {
                case -1:
                    return new EntityDomain.NullLivingEntity();
                case 0:
                    return targetCreature_Sight;
                case 1:
                    return targetPlant_Sight;
                case 2:
                    return targetCreature_Smell;
                case 3:
                    return targetPlant_Smell;
            }
            return new EntityDomain.NullLivingEntity();

            /*if (distanceToCreature < distanceToPlant)
            {
                // preferes to eat the creature
                if (!isMature())
                {
                    return targetPlant;
                }
                return targetCreature;
            }
            else
            {
                // preferes to eat the plant
                return targetPlant;
            }*/
            // [--] Choosing best target [--]
        }

        private Creature checkSensesForMate()
        {
            float distance;

            // [++] Sight [++]
            LivingEntity target_Sight = new EntityDomain.NullLivingEntity();
            float minDistance_Sight = Configs.GridColumns();

            foreach(LivingEntity livingEntity in this._visibleEntities)
            {
                if (livingEntity.GetType() == typeof(Creature))
                {
                    Creature creature = (Creature)livingEntity;
                    if (isValidMate(creature))
                    {
                        distance = EcoMath.Math.distanceBetween(this, livingEntity);
                        if (distance < minDistance_Sight)
                        {
                            minDistance_Sight = distance;
                            target_Sight = creature;
                        }
                    }
                }
            }
            // [--] Sight [--]

            // [++] Smell [++]
            LivingEntity target_Smell = new EntityDomain.NullLivingEntity();
            float minDistance_Smell = Configs.GridColumns();

            foreach(SmellNode smell in this._sensedSmells)
            {
                if (smell.sourceType() == typeof(Creature) && this.isMature())
                {
                    Creature creature = (Creature)smell.source();
                    if (isValidMate(creature))
                    {
                        // eat only if not similar
                        distance = EcoMath.Math.distanceBetween(this, smell) + smell.getDistanceToSource();
                        if (distance < minDistance_Smell)
                        {
                            minDistance_Smell = distance;
                            target_Smell = smell.source();
                        }
                    }
                }
            }
            // [--] Smell [--]
            
            // comparing distances
            float preferedDistance = Configs.GridColumns();
            int preferedMate = -1;
            List<float2> distances = new List<float2>();

            if (Configs.CreatureSense_Sight_Creature())
            {
                distances.Add(new float2(minDistance_Sight, 0));     // Creature Sight index  ==  0
            }
            if (Configs.CreatureSense_Smell_Creature())
            {
                distances.Add(new float2(minDistance_Smell, 2));     // Creature Smell index  ==  2
            }

            // obtain the minimum distance
            for (int i = 0; i < distances.Count; i++)
            {
                if (preferedDistance > distances[i][0])
                {
                    preferedDistance = distances[i][0];
                    preferedMate = (int)distances[i][1];
                }
            }
            
            switch (preferedMate) 
            {
                case 0:
                    return (Creature)target_Sight;
                case 2:
                    return (Creature)target_Smell;
            }
            return null;
        }

        private bool isValidMate(Creature potentialMate)
        {
            if (
               !isSimilar(potentialMate)
            || potentialMate.getGender() == CreatureGender.Male
            || this._rejectedBy.Contains(potentialMate)
            || potentialMate == this
            || potentialMate.isChildOf(this)
            || this.isChildOf(potentialMate)
            )
            {
                return false;
            }
            return true;
        }
        // [--] Checking [--]
        // ---- [--] Senses [--] ----

        private void explore()
        {   
            GridMap.currentGridInstance.moveCreatureRandomly(this.getCoordinates(), this.getMoveDirection(), this);
            startAnimateTo(this.getCoordinates());
        }
        private void moveTo(int2 targetCoordinates)
        {
            GridMap.currentGridInstance.moveCreatureTo(this.getCoordinates(), targetCoordinates, this);
            startAnimateTo(this.getCoordinates());
        }

        private bool getTriggeredByMate(Creature mate)
        {
            if (this.getEnergy() > 0.6f)
            {
                getPathTowards(mate.getCoordinates());
                this._physicalState = CreatureState.GoingToMate;
                return true;
            }
            return false;
        }
        
        private bool willingToMate()
        {
            if (!isMature())    // not mature
            {
                return false;
            }

            if (this.getEnergy() < 0.6f)    // hungry
            {
                return false;
            }

            // chance to mate
            int probability = UnityEngine.Random.Range(0,10);
            if (probability < Configs.RejectionProbability())    // 60% probability to be rejected
            {
                return false;
            }
            return true;
        }

        private void placeSmell()
        {
            if (this._time_smell >= Configs.SmellPlaceInterval())
            {
                this._time_smell = 0;

                // replace the current smell node with a new one
                this._smell = new SmellNode(this, this._smell);
                if (this._smell == null)
                {
                    throw new System.Exception("NULL SMELL CREATION");
                }
            }
        }

        protected override void destroySmell()
        {
            if (this._smell != null)
            {
                this._smell.destroySelf();
            }
            else
            {
                throw new System.Exception("NULL SMELL IN CREATURE");
            }
        }
        // #### #### [--] Behaviour [--] #### ####


        private void getPathTowards(int2 targetCoordinates)
        {
           this._path = new Queue<int2>(GridMap.currentGridInstance.pathTo(this.getCoordinates(), targetCoordinates));
        }

        private void startAnimateTo(int2 coordinates)
        {
            this._animationStartPos = this.getObject().transform.localPosition;
            this._animationEndPos = EcoMath.Math.gridToWorldCoordinates(coordinates, this.getObject());
            this._isAnimatingMovement = true;
        }
        private void animate()
        {
            int2 worldToGridCoords = GridMap.currentGridInstance.worldToGridCoordinates(this.getObject().transform.localPosition);
            
            // animating
            if (this._isAnimatingMovement)
                animateMovement();

            // finished animating
            if (this._animationElapsedTime >= 1)
            {
                this._time = 0; // reset brain timer
                this._isAnimatingMovement = false;
                this._animationElapsedTime = 0;
            }
        }

        private void animateMovement()
        {
            this._animationElapsedTime = Mathf.Min(1, this._animationElapsedTime + Time.deltaTime * Configs.MoveSpeedDefault() + Time.deltaTime * this._geneMotorSpeed * 1.5f);
            this.getObject().transform.localPosition = Vector3.Lerp(this._animationStartPos, this._animationEndPos, this._animationElapsedTime);
        }

        private void animateEating(LivingEntity entity)
        {
            if (entity != null && entity.getObject() != null)
            {
                Sprite sprite = entity.getObject().GetComponent<SpriteRenderer>().sprite;
                Color color = entity.getObject().GetComponent<SpriteRenderer>().color;
                Vector3 localPosition = entity.getObject().transform.localPosition;
                Vector3 localScale = entity.getObject().transform.localScale;
                Vector3 targetPosition = this.getObject().transform.localPosition;
                Object.Instantiate(Configs.AnimatedFoodPrefab()).GetComponent<AnimatedFood>().updateAttributes(sprite, color, localPosition, localScale, targetPosition);
            }
        }
        private void animateMating(LivingEntity entity)
        {
            // animation code
        }

        private bool isOld()
        {
            if (this._age > this._deathAge)
            {
                return true;
            }
            return false;
        }
        private bool isMature()
        {
            if (this._age >= 18)
            {
                return true;
            }
            return false;
        }
        protected override bool isSimilar(LivingEntity creature)
        {
            return UtilsGenetics.areSimilar(this, creature);
        }
        private bool isChildOf(Creature creature)
        {
            if (creature == this._father || creature == this._mother)
            {
                return true;
            }
            return false;
        }
    }

}
