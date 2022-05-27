using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityDomain;
using Unity.Mathematics;
using SmellDomain;

namespace GridDomain
{
    public class Cell
    {
        // #### #### [++] Attributes [++] #### ####
        private int2 _coordinates;
        private int _cellSize;
        private GameObject _cellObject = null;
        private Tile _tile = null;
        private Entity _entity = new NullEntity(); // what entity is on the cell; is it occupied? used for pathfinding
        private List<SmellNode> _smells = new List<SmellNode>();
        // #### #### [--] Attributes [--] #### #### 


        // #### #### [++] Constructor [++] #### ####
        public Cell()
        {
            this._coordinates = new int2(0, 0);
            setObject();
        }
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

        // ---- [++] Copy constructor [++] ----
        public Cell(Cell otherCell) : this(otherCell.getObject().transform.parent.gameObject, otherCell.getCoordinates(), otherCell.getCellSize(), otherCell.getTile())
        {   
            // might need some validation
        }
        // #### #### [--] Constructor [--] #### ####


        // #### #### [++] Getters & Setters [++] #### ####
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

        // ---- [++] Cell Size [++] ---- 
        public int getCellSize()
        {
            return this._cellSize;
        }
        private void setCellSize(int newCellSize)
        {
            this._cellSize = newCellSize;
        }
        // ---- [--] Cell Size [--] ---- 

        // ---- [++] Tile [++] ---- 
        public Tile getTile()
        {
            return this._tile;
        }
        public void setTile(Tile newTile)
        {
            this._tile = newTile;

            // make the new Tile object child of this Cell object
            this._tile.getObject().transform.SetParent(this._cellObject.transform);
            this._tile.getObject().transform.localPosition = new Vector3(0, 0, 0); // reset tile object position relative to parent
        }
        // ---- [--] Tile [--] ---- 

        // ---- [++] Cell Object[++] ---- 
        private void setObject()
        {
            this._cellObject = new GameObject("Cell " + this._coordinates);
        }
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

        // ---- [++] Smells [++] ---- 
        public bool hasSmell()
        {
            if (this._smells.Count > 0)
            {
                return true;
            }
            return false;
        }
        public List<SmellNode> getSmells()
        {
            return this._smells;
        }
        public void addSmell(SmellNode smellNode)
        {
            if (smellNode == null)
            {
                return;
            }
            this._smells.Add(smellNode);
        }
        public void destroySmell(SmellNode smellNode)
        {
            if (smellNode == null)
            {
                return;
            }
            this._smells.Remove(smellNode);
        }
        // ---- [--] Smells [--] ---- 
        // #### #### [--] Getters & Setters [--] #### ####


        // #### #### [++] Utils [++] #### #### 
        private Vector3 getWorldPosition(int x, int y, float cellSize)
        {
            return new Vector3(x, y) * cellSize;
        }

        public bool isOccupied()
        {
            if (this._entity.GetType() != typeof(NullEntity))
            {
                return true;
            }
            return false;
        }
        // #### #### [--] Utils [--] #### #### 


        // #### #### [++] Overrides [++] #### #### 
        public override string ToString()
        {
            return "Cell " + this._coordinates.ToString();
        }
        // #### #### [--] Overrides [--] #### #### 
    }
}