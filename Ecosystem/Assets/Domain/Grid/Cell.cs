using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GridDomain
{
    public class Cell
    {
        // #### [++] Attributes [++] ####
        private Vector2Int _coordinates;
        public Cell(Vector2Int coordinates)
        {
            this._coordinates = coordinates;
        }
        // #### [--] Attributes [--] #### 


        // #### [++] Getters & Setters [++] ####
        // ---- [++] Coordinates [++] ---- 
        public Vector2Int getCoordinates()
        {
            return this._coordinates;
        }
        private void setCoordinates(Vector2Int coordinates)
        {
            this._coordinates = coordinates;
        }
        // ---- [--] Coordinates [--] ---- 
        // #### [--] Getters & Setters [--] ####

        public override string ToString()
        {
            return "Tile " + this._coordinates.ToString();
        }
    }
}