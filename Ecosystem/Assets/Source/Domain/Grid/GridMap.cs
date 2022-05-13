using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Unity.Mathematics;
using EntityDomain;
using GameConfigDomain;
using PathFinding;

namespace GridDomain
{
    public class GridMap
    {
        public static GridMap currentGridInstance;
        private PathFinder pathFinder;

        // #### #### [++] Attributes [++] #### ####
        private GameObject _gridObject = null;
        private float _cellSize;
        private int _cols;
        private int _rows;
        private Cell[,] gridArray;
        ArrayList grid;
        List<Entity> foundLivingEntities;
        //private List<LivingEntity> _creatures = new List<LivingEntity>();
        // #### #### [--] Attributes [--] #### #### 


        // #### #### [++] Constructor [++] #### ####
        public GridMap(int rows, int cols, float cellSize)
        {
            // if there is already a gridmap -> do nothing -> previous gridmap needs to be destroyed

            // create grid object and put it inside game container
            this._gridObject = new GameObject("Grid");
            this._gridObject.transform.SetParent(GameObject.Find("GameObjects").transform);

            setRowsNumber(rows);
            setColsNumber(cols);
            setCellSize(cellSize);
            positionTo(getCenter());
            ConstructGridArray();
            addRandomCreatures();
            spawnRandomPlants();
            setPathFinder();
            GridMap.currentGridInstance = this;
        }

        private void ConstructGridArray()
        {
            gridArray = new Cell[this._cols, this._rows];
            // create a <Cell> and <Tile> pointing to null to change its value for constructing the array 
            Cell cell;
            Tile tile;
            int2 coordinates;

            
            for (int row = 0; row < gridArray.GetLength(1); row++)
            {
                for (int col = 0; col < gridArray.GetLength(0); col++)
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

        public void addRandomCreatures()
        {
            Debug.Log("Spawning creatures...");
            Creature newCreature;
            for (int row = 0; row < gridArray.GetLength(1); row++)
            {
                for (int col = 0; col < gridArray.GetLength(0); col++)
                {
                    int doSpawn = UnityEngine.Random.Range(1, GameConfig.instance.SpawnEntityProbability);
                    if (doSpawn == 1)
                    {
                        newCreature = new Creature(new int2(col, row));

                        Transform newCreatureTransform = newCreature.getObject().transform;
                        newCreatureTransform.SetParent(this._gridObject.transform);
                        
                        // set creature in the corresponding grid position
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

        public void spawnRandomPlants()
        {
            Debug.Log("Spawning plants...");
            Plant newPlant;
            Cell randomCell;
            int numberOfTiles = this._rows * this._cols;

            int plantsToSpawn = UnityEngine.Random.Range(0, numberOfTiles / GameConfig.instance.SpawnPlantProbability + 1);
            for (int plantNumber = 1; plantNumber <= plantsToSpawn; plantNumber++)
            {
                randomCell = findRandomCell();

                if (randomCell == null)
                {
                    continue;
                }
                newPlant = new Plant(new int2(randomCell.getCoordinates().x, randomCell.getCoordinates().y));

                Transform newPlantTransform = newPlant.getObject().transform;
                newPlantTransform.SetParent(this._gridObject.transform);
                
                // set plant in the corresponding grid position
                newPlantTransform.localPosition = new Vector3(randomCell.getCoordinates().x * this._cellSize, randomCell.getCoordinates().y * this._cellSize, newPlantTransform.position.z - 5);
                randomCell.setEntity(newPlant);
            }
        }
        private Cell findRandomCell()
        {
            int tries = 10;
            int row = UnityEngine.Random.Range(0, this._rows);
            int col = UnityEngine.Random.Range(0, this._cols);
            while (!isCellFree(gridArray[col, row]) && tries-- > 0)
            {
                row = UnityEngine.Random.Range(0, this._rows);
                col = UnityEngine.Random.Range(0, this._cols);
            }
            if (tries < 0)
            {
                return null;
            }
            return gridArray[col, row];
        }

        private void setPathFinder()
        {
            this.pathFinder = new PathFinding.A_Star();
        }
        public void setPathFinder(PathFinder newPathFinder)
        {
            this.pathFinder = newPathFinder;
        }
        // #### #### [--] Constructor [--] #### ####


        // #### #### [++] Getters & Setters [++] #### #### 
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
        // #### #### [--] Getters & Setters [--] #### ####

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


        // #### #### [++] Methods [++] #### ####
        public void updateCreatures()
        {
            if (this.gridArray != null)
            {
                foreach(Cell cell in this.gridArray)
                {
                    if(cell.getEntity().GetType().IsSubclassOf(typeof(LivingEntity)))
                    {   // if cell contains a living entity, consume its energy by the configured amount in GameConfig
                        if (cell.getEntity().GetType() == typeof(Creature))
                        {
                            Creature foundLivingEntity = (Creature)cell.getEntity();
                            foundLivingEntity.updateBrain();
                        }
                    }
                }
            }
        }
        public void consumeAllCreaturesEnergy(float amount)
        {
            if (this.gridArray != null)
            {
                foreach(Cell cell in this.gridArray)
                {
                    if(cell.getEntity().GetType().IsSubclassOf(typeof(LivingEntity)))
                    {   // if cell contains a living entity, consume its energy by the configured amount in GameConfig
                        LivingEntity foundLivingEntity = (LivingEntity)cell.getEntity();
                        foundLivingEntity.modifyEnergyBy(amount);
                    }
                }
            }
        }
        public void destroyDeadEntities()
        {
            if (this.gridArray != null)
            {
                Debug.Log("dead body collection started...");
                foreach(Cell cell in this.gridArray)
                {
                    if(cell.getEntity().GetType().IsSubclassOf(typeof(LivingEntity)))
                    {   // if cell contains a living entity, consume its energy by the configured amount in GameConfig
                        LivingEntity foundLivingEntity = (LivingEntity)cell.getEntity();
                        if (!foundLivingEntity.isAlive())
                        {
                            Debug.Log("dead body collected.");
                            cell.setEntity(new NullEntity());
                        }
                    }
                }
            }
            Debug.Log("dead body collection ended..");
        }

        public List<Entity> getVisibleEntities(Creature askingEntity)
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
                col_max = this._cols - 1;
            }
            if (row_min < 0)
            {
                row_min = 0;
            }
            if (row_max > this._rows - 1)
            {
                row_max = this._rows - 1;
            }


            // going through the gridArray and return found LivingEntities
            this.foundLivingEntities = new List<Entity>();
            for (int col = col_min; col <= col_max; col++)
            {
                for (int row = row_min; row <= row_max; row++)
                {
                    if (gridArray[col, row].getEntity().GetType().BaseType == typeof(LivingEntity))
                    {
                        if (gridArray[col, row].getEntity() != askingEntity)
                        {
                            this.foundLivingEntities.Add(gridArray[col, row].getEntity());
                        }
                    }
                }
            }

            return this.foundLivingEntities;
        }

        public void moveCreatureRandomly(int2 creatureCoordinates, Vector2 moveDirection)
        {
            int2 cellCoordinates = getNextCellRaw(creatureCoordinates);
            
            if (cellCoordinates.x != -1 && cellCoordinates.y != -1)
            {
                //  if cell is free, move the creature
            gridArray[cellCoordinates.x, cellCoordinates.y].setEntity((Creature)gridArray[creatureCoordinates.x, creatureCoordinates.y].getEntity());   // move creature to next cell
            Creature movedEntity = (Creature)gridArray[cellCoordinates.x, cellCoordinates.y].getEntity();
            movedEntity.setCoordinates(new int2(cellCoordinates.x, cellCoordinates.y));              // update creature coordinates
            // movedEntity.setMoveDirection(moveDirection);
            gridArray[creatureCoordinates.x, creatureCoordinates.y].setEntity(new NullEntity());     // remove creature from previous cell                                                        

            //Transform movedEntityTransform = movedEntity.getObject().transform;  
            //Vector3 currentPos = movedEntityTransform.localPosition;
            //Vector3 nextPos = new Vector3(cellCoordinates.x * this._cellSize, cellCoordinates.y * this._cellSize, movedEntityTransform.position.z);
            
            //float moveSpeed = movedEntity.getMoveSpeed();

            //movedEntityTransform.localPosition = Vector3.Lerp(currentPos, nextPos, 1f);
            }
            
        }
        public void moveCreatureTo(int2 creatureCoordinates, int2 targetCoordinates)
        {
            if (targetCoordinates.x != -1 && targetCoordinates.y != -1)
            {
                //  if cell is free, move the creature
                gridArray[targetCoordinates.x, targetCoordinates.y].setEntity((Creature)gridArray[creatureCoordinates.x, creatureCoordinates.y].getEntity());   // move creature to next cell
                Creature movedEntity = (Creature)gridArray[targetCoordinates.x, targetCoordinates.y].getEntity();
                movedEntity.setCoordinates(new int2(targetCoordinates.x, targetCoordinates.y));          // update creature coordinates
                gridArray[creatureCoordinates.x, creatureCoordinates.y].setEntity(new NullEntity());     // remove creature from previous cell             
            }
            
        }

        public Queue<int2> pathTo(int2 startCoordinates, int2 targetCoordinates)
        {
            return pathFinder.findPathTo(startCoordinates, targetCoordinates, this.gridArray);
        }

        /*public static Vector2 rotate(Vector2 vector, float alpha)
        {
            return new Vector2(
                vector.x * Mathf.Cos(alpha) - vector.y * Mathf.Sin(alpha),
                vector.x * Mathf.Sin(alpha) + vector.y * Mathf.Cos(alpha)
            );
        }*/

        private Vector3 rotate(Vector3 vector, float degrees)
        {   // dont think it works properly
            float vectorDegrees = CalculateAngle(vector, Vector3.right); // calculate the angle between this vector and world 0 rotation
            float newVectorDegrees = vectorDegrees;
            
            if (GameConfig.instance.Debugging)
            {
                Debug.Log("direction degrees: " + vectorDegrees);
            }
            
            if (vectorDegrees + degrees >= 360)
            {
                newVectorDegrees = vectorDegrees + degrees - 360;
            }
            if (vectorDegrees + degrees <= 0)
            {
                newVectorDegrees = Mathf.Abs(vectorDegrees + degrees);
            }
            return Quaternion.Euler(0, 0, newVectorDegrees) * vector;
        }
        private float CalculateAngle(Vector3 from, Vector3 to)
        {
            //return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
            float angle = Vector3.Angle(from, to);
            return (Vector3.Angle(Vector3.left, to) > 90f) ? 360f - angle : angle;   
        }

        private int2 getNextCellVector(int2 creatureCoordinates, Vector3 moveDirection)
        {
            // TO DO : NOT CHANGING DIRECTION ENOUGH
            float alphaDegrees = UnityEngine.Random.Range(-90, 90);
            moveDirection = rotate(moveDirection, alphaDegrees);

            if (GameConfig.instance.Debugging)
            {
                Debug.Log("move Direction Vector: " + moveDirection);
            }

            //Redo:
            int2 adiacentDirection = directionVector2ToInt(moveDirection);
            int2 nextCellCoordinates = creatureCoordinates + adiacentDirection;

            //Redo:
            if (!isInBounds(nextCellCoordinates))
            {   // not in bounds
                nextCellCoordinates = getNextCellRaw(creatureCoordinates);
                moveDirection = rotate(moveDirection, 180);

                //goto Redo;
            }
            if (!isCellFree(gridArray[nextCellCoordinates.x, nextCellCoordinates.y]))
            {   // not free
                nextCellCoordinates = getNextCellRaw(creatureCoordinates);
                moveDirection = rotate(moveDirection, 180);
                //goto Redo;
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
        public bool isCellFree(int2 cellCoords)
        {
            if (cellCoords.x < 0 || cellCoords.y < 0)
            {
                return false;
            }
            if (gridArray[cellCoords.x, cellCoords.y].getEntity().GetType() == typeof(NullEntity))
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
        // #### #### [--] Methods [--] #### ####
    }
}
