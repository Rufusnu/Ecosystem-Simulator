using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace EntityDomain
{
    public abstract class Entity : MonoBehaviour
    {
        // #### [++] Attributes [++] ####
        static int entityCounter = 0;
        private int2 _coordinates;
        // #### [--] Attributes [--] ####


        // #### [++] Initialization [++] ####
        public virtual void Initialize(int2 newCoordinates)
        {
            setCoordinates(newCoordinates);
        }
        // #### [--] Initialization [--] ####


        // #### [++] Getters & Setters [++] ####
        // ---- [++] Coordinates [++] ---- 
        public int2 getCoordinates()
        {
            return this._coordinates;
        }
        public void setCoordinates(int2 newCoordinates)
        {
            this._coordinates = newCoordinates;
        }
        // ---- [--] Coordinates [--] ---- 
        // #### [--] Getters & Setters [--] ####
    }
}
