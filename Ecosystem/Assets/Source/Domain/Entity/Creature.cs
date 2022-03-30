using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace EntityDomain
{
    public class Creature : LivingEntity
    {
        // #### [++] Attributes [++] ####
        const int NumberOfGenes = 0; // number of attributes that can be mutated
        static int creatureCounter = 0;
        // #### [--] Attributes [--] ####


        // #### [++] Initialization [++] ####
        public override void Initialize(int2 newCoordinates)
        {
            base.Initialize(newCoordinates);
            Creature.creatureCounter++;
        }
        // #### [--] Initialization [--] ####


        // #### [++] Brain [++] ####
        public void Update()
        {
            Debug.Log("Works");
        }
        // #### [--] Brain [--] ####


        // #### [++] Behaviour [++] ####
        protected override void eat(LivingEntity entity)
        {

        }
        // #### [--] Behaviour [--] ####
    }

}
