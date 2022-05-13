using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace EntityDomain
{
    public abstract class Entity
    {
        // #### #### [++] Attributes [++] #### ####
        public static int entityCounter = 0;
        private int2 _coordinates;
        private GameObject _object = null;
        // #### #### [--] Attributes [--] #### ####


        // #### #### [++] Initialization [++] #### ####
        public Entity(int2 newCoordinates)
        {
            setCoordinates(newCoordinates);
        }
        // #### #### [--] Initialization [--] #### ####


        // #### #### [++] Getters & Setters [++] #### ####
        // ---- [++] Coordinates [++] ---- 
        public virtual int2 getCoordinates()
        {
            return this._coordinates;
        }
        public virtual void setCoordinates(int2 newCoordinates)
        {
            this._coordinates = newCoordinates;
        }
        public GameObject getObject()
        {
            return this._object;
        }
        protected void setObject(GameObject newObject)
        {
            this._object = newObject;
        }
        // ---- [--] Coordinates [--] ---- 
        // #### #### [--] Getters & Setters [--] #### ####
    }
}
