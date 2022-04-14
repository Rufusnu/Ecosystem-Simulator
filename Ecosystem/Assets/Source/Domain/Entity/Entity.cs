using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace EntityDomain
{
    public abstract class Entity : MonoBehaviour
    {
        // #### [++] Attributes [++] ####
        public static int entityCounter = 0;
        protected int2 _coordinates;
        // #### [--] Attributes [--] ####


        // #### [++] Initialization [++] ####
        public virtual void Initialize(int2 newCoordinates)
        {
            setCoordinates(newCoordinates);
        }
        // #### [--] Initialization [--] ####


        // #### [++] Getters & Setters [++] ####
        // ---- [++] Coordinates [++] ---- 
        public virtual int2 getCoordinates()
        {
            return this._coordinates;
        }
        public virtual void setCoordinates(int2 newCoordinates)
        {
            this._coordinates = newCoordinates;
        }
        // ---- [--] Coordinates [--] ---- 
        // #### [--] Getters & Setters [--] ####
    }
}
