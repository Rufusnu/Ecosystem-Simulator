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
                    if(cell.getEntity().GetType().BaseType == typeof(LivingEntity))
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

        public List<LivingEntity> getVisibleEntities(Creature askingEntity)
        {
            int2 entityCoordinates = askingEntity.getCoordinates();
            int sightDistance = askingEntity.getSightDistance();

            // computing sight limts
            int col_min, col_max, row_min, row_max;
            col_min = entityCoordinates.x - sightDistance;
            col_max = entityCoordinates.x + sightDistance;
            row_min = entityCoordinates.y - sightDistance;
            row_max = entityCoordinates.y + sightDistance;

            if (col_min < 0)
            {
                col_min = 0;
            }
            if (col_max > this._cols - 1)
            {
                col_max = this._cols;
            }
            if (row_min < 0)
            {
                row_min = 0;
            }
            if (row_max > this._rows - 1)
            {
                row_max = this._rows;
            }


            // going through the gridArray and return found LivingEntities
            List<LivingEntity> foundLivingEntities = new List<LivingEntity>();
            for (int col = col_min; col < col_max; col++)
            {
                for (int row = row_min; row < row_max; row++)
                {
                    if (gridArray[col, row].getEntity().GetType().BaseType == typeof(LivingEntity))
                    {
                        foundLivingEntities.Add((LivingEntity)gridArray[col, row].getEntity());
                    }
                }
            }

            return foundLivingEntities;
        }

        public void moveCreature(int2 creatureCoordinates, Vector2 moveDirection)
        {
            int2 cellCoordinates = getNextCellRaw(creatureCoordinates);
            
            if (cellCoordinates.x != -1 && cellCoordinates.y != -1)
            {
                //  if cell is free, move the creature
            gridArray[cellCoordinates.x, cellCoordinates.y].setEntity((Creature)gridArray[creatureCoordinates.x, creatureCoordinates.y].getEntity());   // move creature to next cell
            Creature movedEntity = (Creature)gridArray[cellCoordinates.x, cellCoordinates.y].getEntity();
            movedEntity.setCoordinates(new int2(cellCoordinates.x, cellCoordinates.y));
            movedEntity.setMoveDirection(moveDirection);
            gridArray[creatureCoordinates.x, creatureCoordinates.y].setEntity(new NullEntity());     // remove creature from previous cell                                                        // update creature coordinates

            //Transform movedEntityTransform = movedEntity.getObject().transform;  
            //Vector3 currentPos = movedEntityTransform.localPosition;
            //Vector3 nextPos = new Vector3(cellCoordinates.x * this._cellSize, cellCoordinates.y * this._cellSize, movedEntityTransform.position.z);
            
            //float moveSpeed = movedEntity.getMoveSpeed();

            //movedEntityTransform.localPosition = Vector3.Lerp(currentPos, nextPos, 1f);
            }
            
        }

        /*public static Vector2 rotate(Vector2 vector, float alpha)
        {
            return new Vector2(
                vector.x * Mathf.Cos(alpha) - vector.y * Mathf.Sin(alpha),
                vector.x * Mathf.Sin(alpha) + vector.y * Mathf.Cos(alpha)
            );
        }*/

        public Vector3 rotate(Vector3 vector, float degrees)
        {
            return Quaternion.Euler(0, 0, degrees) * vector;
        }

        private int2 getNextCellVector(int2 creatureCoordinates, Vector3 moveDirection)
        {
            // TO DO : NOT CHANGING DIRECTION ENOUGH
            float alphaDegrees = UnityEngine.Random.Range(-80, 80);
            moveDirection = rotate(moveDirection, alphaDegrees);

            //Redo:
            int2 adiacentDirection = directionVector2ToInt(moveDirection);
            int2 nextCellCoordinates = creatureCoordinates + adiacentDirection;

            Redo:
            if (!isInBounds(nextCellCoordinates))
            {   // not in bounds
                nextCellCoordinates = getRandomNextCell(creatureCoordinates);

                goto Redo;
            }
            if (!isCellFree(gridArray[nextCellCoordinates.x, nextCellCoordinates.y]))
            {   // not free
                nextCellCoordinates = getRandomNextCell(creatureCoordinates);

                goto Redo;
            }
            return nextCellCoordinates;
        }

        private int2 getNextCellRaw(int2 creatureCoordinates)
        {
            const int max_tries = 4;
            int tries = 0;

            int2 newCellCoordinates;
            int xOffset,yOffset;

            Redo:
            xOffset = UnityEngine.Random.Range(-1, 2);

            if (xOffset != 0) // x == {-1,1}; y == 0
            {
                yOffset = 0;
            }
            else        // x == 0; y == {-1,1}
            {
                yOffset = UnityEngine.Random.Range(-1, 2);
                while (yOffset == 0)
                {
                    yOffset = UnityEngine.Random.Range(-1, 2);
                }
            }
            tries++;
            newCellCoordinates = new int2(creatureCoordinates.x + xOffset, creatureCoordinates.y + yOffset);
            if (tries <= max_tries)
            {
                if (!isInBounds(newCellCoordinates))
                {
                    goto Redo;
                }
                if (!isCellFree(gridArray[newCellCoordinates.x, newCellCoordinates.y]))
                {
                    goto Redo;
                }
                return newCellCoordinates;
            }
            else
            {
                return new int2(-1, -1);
            }
        }

        private int2 directionVector2ToInt(Vector3 vectorDirection)
        {
            int x, y;
            if (Mathf.Abs(vectorDirection.x) >= Mathf.Abs(vectorDirection.y))
            {   // |x| >= |y|
                if (vectorDirection.x > 0)
                {
                    x = 1;          // (1,0)
                }
                else
                {
                    x = -1;         // (-1,0)
                }
                y = 0;
            }
            else
            {   // |x| <  |y|
                if (vectorDirection.y > 0)
                {
                    y = 1;          // (0,1)
                }
                else
                {
                    y = -1;         // (0,-1)
                }
                x = 0;
            }
            return new int2(x, y);
        }
        private bool isCellFree(Cell cell)
        {
            if (cell.getEntity().GetType() == typeof(NullEntity))
            {
                return true;
            }
            return false;
        }
        private bool isInBounds(int2 coordinates)
        {
            // check if in bounds of array
            if (coordinates.x < 0 || coordinates.x > this._cols - 1 || coordinates.y < 0 || coordinates.y > this._rows - 1)
            {
                return false;    // coordinates out of bounds
            }
            return true;         // coordinates in bounds
        }

        private int2 getRandomNextCell(int2 creatureCoordinates)
        {
            int2 newCellCoordinates;
            int xOffset,yOffset;

            Redo:
            xOffset = UnityEngine.Random.Range(-1, 2);

            if (xOffset != 0) // x == {-1,1}; y == 0
            {
                yOffset = 0;
            }
            else        // x == 0; y == {-1,1}
            {
                yOffset = UnityEngine.Random.Range(-1, 2);
                while (yOffset == 0)
                {
                    yOffset = UnityEngine.Random.Range(-1, 2);
                }
            }
            newCellCoordinates = new int2(creatureCoordinates.x + xOffset, creatureCoordinates.y + yOffset);
            if (!isInBounds(newCellCoordinates))
            {
                goto Redo;
            }
            if (!isCellFree(gridArray[newCellCoordinates.x, newCellCoordinates.y]))
            {
                goto Redo;
            }
            return newCellCoordinates;
        }

        public int2 worldToGridCoordinates(Vector3 position)
        {
            return new int2((int)(position.x / this._cellSize), (int)(position.y / this._cellSize));
        }
        // #### [--] Methods [--] ####
    }
}
