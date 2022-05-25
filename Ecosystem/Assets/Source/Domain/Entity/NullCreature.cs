using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneticsDomain;
using Unity.Mathematics;

namespace EntityDomain
{
    /*public class NullCreature : Creature
    {
        // Start is called before the first frame update
        public NullCreature() : base(new int2(-1, -1))
        {
            this._energy = 0;
            this._livingState = LivingState.Dead;
        }

        
        // #### #### [++] Initialization [++] #### ####
        private void initializeTargets()
        {
            this._targetFood = new NullLivingEntity();
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
            this.setObject(new GameObject("[Creature" + Creature.creatureCounter + "]"));
        }
        private void initializeCreatureObject(CreatureGender gender)
        {
            this.setObject(new GameObject("[Creature" + Creature.creatureCounter + "]"));
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
            this._urgeToMate = 0;
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
            this._age = (int)UnityEngine.Random.Range(10,20);
        }
        private void initializeDefaultGenes() {}
        // #### #### [--] Initialization [--] #### ####


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
        }
        public float getGene_SensorialSmell()
        {
            return this._geneSensorialSmell;
        }
        public float getGene_FoodPreference()
        {
            return this._geneFoodPreference;
        }
        public float getGene_Behaviour()
        {
            return this._geneBehaviour;
        }
        // ---- [--] Genes [--] ----
        

        /*public float getMoveDuration()
        {
            return EntityConfig.instance.MoveDuration + this._geneMotorSpeed/1.25f;
        }*/

        /*public int2 getMoveDirection()
        {
            return this._currentMoveDir;
        }
        public override void setMoveDirection(int2 currentMoveDir) {}


        // ---- [++] Energy [++] ---- 
        public new void setEnergy(float newEnergy) {}
        public new void modifyEnergyBy(float amount) {}
        protected new virtual void consumeEnergy(ActionType action) {}
        // ---- [--] Energy [--] ----

        // ---- [++] Nutrition [++] ----
        public new float getNutrition()
        {
            return this._nutritionValue;
        }
        // ---- [--] Nutrition [--] ----

        protected override void eat(LivingEntity entity) {} // to be implemented by Plants and Creatures
        public override void eatenBy(LivingEntity entity) {}
        public override void mateWith(LivingEntity entity) {}
        protected override void initializeNutritionValue() {}
        // #### #### [--] Getters & Setters [--] #### ####
    }*/
}
