using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityDomain
{
    public class Creature : LivingEntity
    {
        // #### [++] Attributes [++] ####
        static int creatureCounter = 0;
        private GameObject _creatureObject = null;
        // #### [--] Attributes [--] ####


        // #### [++] Constructor [++] ####
        public Creature(Vector2Int newCoordinates, float newEnergy) : base (newCoordinates)
        {
            initializeCreatureObject();
        }
        public Creature(Vector2Int newCoordinates) : base (newCoordinates)
        {
            initializeCreatureObject();
        }
        private void initializeCreatureObject()
        {
            this._creatureObject = new GameObject("Creature" + Creature.creatureCounter);
            Creature.creatureCounter++;
            this._creatureObject.AddComponent<CreatureBrain>().setCreature(this);
        }
        // #### [--] Constructor [--] ####


        public void brainUpdate()
        {   // function is called by CreatureBrain Monobehaviour script on every Update()
            Debug.Log("Works");
        }


    }

}
