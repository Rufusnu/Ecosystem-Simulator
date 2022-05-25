 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridDomain;
using Unity.Mathematics;
using EntityDomain;
using GeneticsDomain;


namespace SmellDomain
{
    public static class SmellSystem
    {
        private static List<Smell> smells = new List<Smell>();
        public static void executeUpdate()
        {
            if (Configs.Debugging_EnergySystem())
            {
                Debug.Log("Energy System Update");
            }
            updateSmells();
        }

        public static void addSmell(Smell smell)
        {
            smells.Add(smell);
        }
        public static void removeSmell(Smell smell)
        {
            smells.Remove(smell);
        }

        public static void updateSmells()
        {
            foreach(Smell smell in smells)
            {
                smell.dissipate();
                smell.spread();
            }
        }
    }

    public abstract class Smell
    {
        protected float _transmissionRate;
        protected float _dissipationRate;
        protected float _smellQuantity;
        protected LivingEntity _whose;


        public Smell(LivingEntity whose)
        {
            this._whose = whose;

            if (whose.GetType() == typeof(Creature))
            {
                Creature creature = (Creature)whose;
                float transmissionFactor = (1 + creature.getGene_FoodPreference()) + (1 + (-1)*creature.getGene_MotorSpeed());
                float dissipationFactor = 0.5f * (1 + (-1)*creature.getGene_FoodPreference()) + (1 + creature.getGene_MotorSpeed());

                // calculate these variables using <whose> <chromosome> stats
                this._transmissionRate = 1 * transmissionFactor;
                this._dissipationRate = 1 * dissipationFactor;

                this._smellQuantity =  10 * creature.getGene_FoodPreference();
            }
            else if (whose.GetType() == typeof(Plant))
            {
                Plant plant = (Plant)whose;
                this._transmissionRate = 2;
                this._dissipationRate = 0.5f;
            }
        }
        public Smell(float transmissionRate, float dissipationRate, LivingEntity whose)
        {
            this._whose = whose;

            this._transmissionRate = transmissionRate;
            this._dissipationRate = dissipationRate;
            this._smellQuantity = this._dissipationRate;
        }


        public float getTransmissionRate()
        {
            return this._transmissionRate;
        }
        public float getDissipationRate()
        {
            return this._dissipationRate;
        }
        public System.Type getEntityType()
        {
            return this._whose.GetType();
        }


        public void spread()
        {
            // code to transmit smell to neighbour cells using GridMap
        }
        public void dissipate()
        {
            this._smellQuantity -= this._dissipationRate;
        }
    }

    public class SmellSource : Smell
    {
        private int2 _coordinates;

        public SmellSource(LivingEntity whose) : base(whose)
        {
            this._coordinates = whose.getCoordinates();
        }
    }

    public class SmellDissipation : Smell 
    {
        private SmellSource _source;

        public SmellDissipation(float transmissionRate, float dissipationRate, LivingEntity whose, SmellSource source) : base(transmissionRate, dissipationRate, whose)
        {
            this._source = source;
        }
    }
}   
