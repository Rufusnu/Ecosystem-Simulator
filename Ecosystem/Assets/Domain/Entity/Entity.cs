using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public abstract class Entity
    {
        // #### [++] Attributes [++] ####
        private Vector2Int _coordinates;
        // #### [--] Attributes [--] ####

        // #### [++] Constructor [++] ####
        public Entity(Vector2Int newCoordinates)
        {
            setCoordinates(newCoordinates);
        }
        // #### [--] Constructor [--] ####


        // #### [++] Getters & Setters [++] ####
        // ---- [++] Coordinates [++] ---- 
        public Vector2Int getCoordinates()
        {
            return this._coordinates;
        }
        private void setCoordinates(Vector2Int newCoordinates)
        {
            this._coordinates = newCoordinates;
        }
        // ---- [--] Coordinates [--] ---- 
        // #### [--] Getters & Setters [--] ####
    }
}
