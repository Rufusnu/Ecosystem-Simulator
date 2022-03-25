using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GridDomain
{
    public class Cell
    {
        // #### [++] Attributes [++] ####
        private Vector2Int _coordinates;
        private GameObject _cellObject = null;
        private Tile _tile = null;
        // #### [--] Attributes [--] #### 


        // #### [++] Constructor [++] ####
        public Cell(Vector2Int newCoordinates, float cellSize)
        {
            if (newCoordinates == null)
            {
                throw new System.Exception("<Cell> Cannot set null (Vector2Int)coordinates.");
            }
            this._coordinates = newCoordinates;
            setObject();

            // set cell in the corresponding grid position
            this._cellObject.transform.localPosition = getWorldPosition(this._coordinates.x, this._coordinates.y, cellSize);
        }
        private void setObject()
        {
            this._cellObject = (GameObject) new GameObject("Cell " + this._coordinates);
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

        // ---- [++] Tile [++] ---- 
        public Tile getTile()
        {
            return this._tile;
        }
        public void setTile(Tile newTile)
        {
            if (newTile == null)
            {
                throw new System.Exception("<Cell " + this._coordinates.ToString() + "> Cannot set a null Tile.");
            }
            this._tile = newTile;

            // make the new Tile object child of this Cell object
            this._tile.getObject().transform.SetParent(this._cellObject.transform);
            this._tile.getObject().transform.localPosition = new Vector3(0, 0, 0); // reset tile object position relative to parent
        }
        // ---- [--] Tile [--] ---- 

        // ---- [++] Cell Object[++] ---- 
        public GameObject getObject()
        {
            return this._cellObject;
        }
        // ---- [--] Cell Object [--] ---- 
        // #### [--] Getters & Setters [--] ####


        // #### [++] Utils [++] #### 
        private Vector3 getWorldPosition(int x, int y, float cellSize)
        {
            return new Vector3(x, y) * cellSize;
        }
        // #### [--] Utils [--] #### 


        // #### [++] Overrides [++] #### 
        public override string ToString()
        {
            return "Cell " + this._coordinates.ToString();
        }
        // #### [--] Overrides [--] #### 
    }
}