using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Unity.Mathematics;
using EntityDomain;
using GameConfigDomain;

namespace GridDomain
{
    public class GridMap
    {
        public static GridMap currentGridInstance;

        // #### [++] Attributes [++] ####
        private GameObject _gridObject = null;
        private float _cellSize;
        private int _cols;
        private int _rows;
        private Cell[,] gridArray;
        ArrayList grid;
        //private List<LivingEntity> _creatures = new List<LivingEntity>();
        // #### [--] Attributes [--] #### 


        // #### [++] Constructor [++] ####
        public GridMap(int rows, int cols, float cellSize)
        {
            // if there is already a gridmap -> do nothing -> previous gridmap needs to be destroyed

            // create grid object and put it inside game container
            this._gridObject = new GameObject("Grid");
            this._gridObject.transform.SetParent(GameObject.Find("GameObjects").transform);

            setRowsNumber(rows);
            setColsNumber(cols);
            setCellSize(cellSize);
            ConstructGridArray();
            addRandomCreatures();
            GridMap.currentGridInstance = this;
        }

        private void ConstructGridArray()
        {
            gridArray = new Cell[this._cols, this._rows];
            // create a <Cell> and <Tile> pointing to null to change its value for constructing the array 
            Cell cell;
            Tile tile;
            int2 coordinates;

            
            for (int col = 0; col < gridArray.GetLength(0); col++)
            {
                for (int row = 0; row < gridArray.GetLength(1); row++)
                {
                    try {
                        // setting coordinates for the <Cell> and assigning it to the corresponding array position
                        coordinates = new int2(col, row);

                        // creating tile to insert into cell; setting its sprite from the Asset Service Class
                        tile = new Tile(GridTileSet_AssetService.instance.tile_default, coordinates);
                        cell = new Cell(this._gridObject, coordinates, this._cellSize, tile);

                        gridArray[col, row] = cell;
                    } catch (System.Exception exception) {
                        throw new System.Exception("<GridMap> ->" + exception);
                    }
                }
            }
        }

        private void addRandomCreatures()
        {
            Creature newCreature;
            for (int col = 0; col < gridArray.GetLength(0); col++)
            {
                for (int row = 0; row < gridArray.GetLength(1); row++)
                {
                    int doSpawn = UnityEngine.Random.Range(1, GameConfig.instance.SpawnEntityProbability);
                    if (doSpawn == 1)
                    {
                        Debug.Log("Spawning creature...");
                        newCreature = new Creature(new int2(col, row));

                        Transform newCreatureTransform = newCreature.getObject().transform;
                        newCreatureTransform.SetParent(this._gridObject.transform);
                        
                        // set cell in the corresponding grid position
                        newCreatureTransform.localPosition = new Vector3(col * this._cellSize, row * this._cellSize, newCreatureTransform.position.z - 5);
                        //this._creatures.Add(newCreature);
                        this.gridArray[col, row].setEntity(newCreature);

                        /*Debug.Log("prefab: " + EntityConfig.instance.CreaturePrefab);
                        newCreature = GameObject.Instantiate(EntityConfig.instance.CreaturePrefab, gridArray[col, row].getObject().transform.position, gridArray[col, row].getObject().transform.rotation);
                        newCreature.GetComponent<Creature>().Initialize(new int2(col, row));
                        Debug.Log(newCreature.name + " coordinates " + newCreature.GetComponent<Creature>().getCoordinates());
                        gridArray[col, row].setEntity(newCreature.GetComponent<Creature>());*/
                    }
                }
            }
        }
        // #### [--] Constructor [--] ####


        // #### [++] Getters & Setters [++] #### 
        // ---- [++] Cell Size [++] ----
        public float getCellSize()
        {
            return this._cellSize;
        }
        private void setCellSize(float newCellSize)
        {
            if (newCellSize < 0.1f)
            {
                throw new System.Exception("<GridMap> Cannot set cell size with a value less than 0.1f.");
            }
            this._cellSize = newCellSize;
        }
        // ---- [--] Cell Size [--] ----0

        // ---- [++] Rows [++] ----
        public int getRowsNumber()
        {
            return this._rows;
        }
        private void setRowsNumber(int newRows)
        {
            if (newRows < 8)
            {
                throw new System.Exception("<GridMap> Cannot set height/width with a value less than 8.");
            }
            this._rows = newRows;
        }
        // ---- [--] Rows [--] ----
        
        // ---- [++] Cols [++] ----
        public int getColsNumber()
        {
            return this._cols;
        }
        private void setColsNumber(int newCols)
        {
            if (newCols < 8)
            {
                throw new System.Exception("<GridMap> Cannot set height/width with a value less than 8.");
            }
            this._cols = newCols;
        }
        // ---- [--] Cols [--] ----

        // ---- [++] Alive Creature List [++] ----
        /*public List<LivingEntity> getCreatures()
        {
            return this._creatures;
        }*/
        // ---- [--] Alive Creature List [--] ----
        // #### [--] Getters & Setters [--] ####

        // ---- [++] Change Grid Position [++] ----
        public void positionTo(Vector3 newPosition)
        {
            if (newPosition == null)
            {
                throw new System.Exception("<GridMap> Cannot set null Vector3 transform position.");
            }
            this._gridObject.transform.position = newPosition;
        }

        public Vector3 getCenter()
        {
            // numbering from 0
            int rows = this._rows - 1;
            int cols = this._cols - 1;
            return new Vector3(-cols * this._cellSize/2, -rows * this._cellSize /2, 0);
        }
        // ---- [--] Change Grid Position [--] ----


        // #### [++] Methods [++] ####
        public void updateCreatures()
        {
            /*foreach(Creature creature in this._creatures)
            {
                creature.updateBrain();
            }*/
            if (this.gridArray != null)
            {
                foreach(Cell cell in this.gridArray)
                {
                    if(cell.getEntity().GetType() == typeof(Creature))
                    {   // if cell contains a living entity, consume its energy by the configured amount in GameConfig
                        Creature foundLivingEntity = (Creature)cell.getEntity();
                        foundLivingEntity.updateBrain();
                    }
                }
            }
        }
        public void consumeAllCreaturesEnergy(float amount)
        {
            /*foreach(LivingEntity creature in this._creatures)
            {
                creature.addEnergy(amount);
            }*/
            if (this.gridArray != null)
            {
                foreach(Cell cell in this.gridArray)
                {
                    if(cell.getEntity().GetType() == typeof(LivingEntity))
                    {   // if cell contains a living entity, consume its energy by the configured amount in GameConfig
                        LivingEntity foundLivingEntity = (LivingEntity)cell.getEntity();
                        foundLivingEntity.addEnergy(amount);
                    }
                }
            }
        }
        // #### [--] Methods [--] ####
    }
}
