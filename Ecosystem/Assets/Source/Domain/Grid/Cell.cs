using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityDomain;
using Unity.Mathematics;

namespace GridDomain
{
    public class Cell
    {
        // #### [++] Attributes [++] ####
        private int2 _coordinates;
        private GameObject _cellObject = null;
        private Tile _tile = null;
        private Entity _entity = null; // what entity is on the cell; is it occupied? used for pathfinding
        // #### [--] Attributes [--] #### 


        // #### [++] Constructor [++] ####
        public Cell(GameObject parent, int2 newCoordinates, float cellSize)
        {
            this._coordinates = newCoordinates;
            setObject();
            this._cellObject.transform.SetParent(parent.transform);
            // set cell in the corresponding grid position
            this._cellObject.transform.localPosition = getWorldPosition(this._coordinates.x, this._coordinates.y, cellSize);
        }

        public Cell(GameObject parent, int2 newCoordinates, float cellSize, Tile tile)
        {
            this._coordinates = newCoordinates;
            setObject();
            setTile(tile);
            this._cellObject.transform.SetParent(parent.transform);

            // set cell in the corresponding grid position
            this._cellObject.transform.localPosition = getWorldPosition(this._coordinates.x, this._coordinates.y, cellSize);
        }

        private void setObject()
        {
            this._cellObject = new GameObject("Cell " + this._coordinates);
        }
        // #### [--] Constructor [--] ####


        // #### [++] Getters & Setters [++] ####
        // ---- [++] Coordinates [++] ---- 
        public int2 getCoordinates()
        {
            return this._coordinates;
        }
        private void setCoordinates(int2 newCoordinates)
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

        // ---- [++] Entity [++] ---- 
        public void setEntity(Entity newEntity)
        { // can be null (empty)
            this._entity = newEntity;
        }
        public Entity getEntity()
        {
            return this._entity;
        }
        // ---- [--] Entity [--] ---- 
        // #### [--] Getters & Setters [--] ####


        // #### [++] Utils [++] #### 
        private Vector3 getWorldPosition(int x, int y, float cellSize)
        {
            return new Vector3(x, y) * cellSize;
        }

        public bool isOccupied()
        {
            if (this._entity != null)
            {
                return true;
            }
            return false;
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