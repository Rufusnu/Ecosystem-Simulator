using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using EntityDomain;
using PathFinding;
using SmellDomain;

namespace GridDomain
{
    public class GridMap
    {
        public static GridMap currentGridInstance = null;
        private PathFinder pathFinder;

        // #### #### [++] Attributes [++] #### ####
        private GameObject _gridObject = null;
        private float _cellSize;
        private int _cols;
        private int _rows;
        private Cell[,] gridArray;
        private List<LivingEntity> _livingEntities;
        private GameObject _cellsContainer;
        private GameObject _creatureContainer;
        private GameObject _plantContainer;
        ArrayList grid;
        List<Entity> foundLivingEntities;

        int2[] _moveDirections = {new int2(0,-1), new int2(-1,0), new int2(0,1), new int2(1,0)}; // these array will help you travel in the 4 directions more easily
        //private List<LivingEntity> _creatures = new List<LivingEntity>();
        // #### #### [--] Attributes [--] #### #### 


        // #### #### [++] Constructor [++] #### ####
        public static GridMap createGridMap(int rows, int cols, float cellSize)
        {
            if (currentGridInstance == null)
            {
                GridMap.currentGridInstance = new GridMap(rows, cols, cellSize);
            }
            return GridMap.currentGridInstance;
        }
        private GridMap(int rows, int cols, float cellSize)
        {
            // create grid object and put it inside game container
            this._gridObject = new GameObject("Grid");
            this._gridObject.transform.SetParent(GameObject.Find("GameObjects").transform);

            setRowsNumber(rows);
            setColsNumber(cols);
            setCellSize(cellSize);
            positionTo(getCenter());
            initializeContainers();
            initializeLivingEntitiesList();
            ConstructGridArray();
            addRandomCreatures();
            spawnRandomPlants(25);
            setPathFinder();
        }

        private void initializeContainers()
        {
            this._cellsContainer = new GameObject("CellsContainer");
            this._cellsContainer.transform.SetParent(this._gridObject.transform);
            this._cellsContainer.transform.localPosition = new Vector3(0,0,0);

            this._creatureContainer = new GameObject("CreatureContainer");
            this._creatureContainer.transform.SetParent(this._gridObject.transform);
            this._creatureContainer.transform.localPosition = new Vector3(0,0,0);

            this._plantContainer = new GameObject("PlantContainer");
            this._plantContainer.transform.SetParent(this._gridObject.transform);
            this._plantContainer.transform.localPosition = new Vector3(0,0,0);
        }

        private void initializeLivingEntitiesList()
        {
            this._livingEntities = new List<LivingEntity>();
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
                        cell = new Cell(this._cellsContainer, coordinates, this._cellSize, tile);

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
                    int doSpawn = UnityEngine.Random.Range(1, Configs.SpawnProbability_Creature());
                    if (doSpawn == 1)
                    {
                        newCreature = new Creature(new int2(col, row));

                        Transform newCreatureTransform = newCreature.getObject().transform;
                        newCreatureTransform.SetParent(this._creatureContainer.transform);
                        
                        // set creature in the corresponding grid position
                        newCreatureTransform.localPosition = new Vector3(col * this._cellSize, row * this._cellSize, -15);
                        this.gridArray[col, row].setEntity(newCreature);

                        // add it to the alive creatures list
                        this._livingEntities.Add(newCreature);
                    }
                }
            }
        }

        public void addBornCreatures(List<Creature> children)
        {
            foreach(Creature child in children)
            {
                Transform newCreatureTransform = child.getObject().transform;
                newCreatureTransform.SetParent(this._creatureContainer.transform);
                child.getObject().transform.localPosition = new Vector3(child.getCoordinates().x * this._cellSize, child.getCoordinates().y * this._cellSize, -15);
                this._livingEntities.Add(child);
            }
        }

        /*private bool tryToGiveBirthTo(Creature child)
        {
            // try to put the creature on one of the neighbouring cells if cell is free
            int tries = 4;
            int positionIndex = UnityEngine.Random.Range(0,3);

            int2 chosenCoordinates = motherCoordinates + this._moveDirections[positionIndex];
            while(!isCellFree(gridArray[chosenCoordinates.x, chosenCoordinates.y]) && tries > 0)
            {
                positionIndex = UnityEngine.Random.Range(0,3);
                chosenCoordinates = motherCoordinates + this._moveDirections[positionIndex];
                tries--;
            }
            if (isCellFree(gridArray[chosenCoordinates.x, chosenCoordinates.y]))
            {
                child.setCoordinates(chosenCoordinates);
                child.getObject().transform.localPosition = new Vector3(chosenCoordinates.x * this._cellSize, chosenCoordinates.y * this._cellSize, child.getObject().transform.position.z - 5);
                return true;
            }
            else
            {
                child.kill();
                return false;
            }
        }*/

        public void spawnRandomPlants(float probabilityFactor = 1)
        {
            Debug.Log("Spawning plants...");
            Plant newPlant;
            Cell randomCell;
            int numberOfTiles = this._rows * this._cols;

            int plantsToSpawn = UnityEngine.Random.Range((int)(1 * probabilityFactor), (int)(numberOfTiles / Configs.SpawnProbability_Plant() * probabilityFactor) + 1);
            for (int plantNumber = 1; plantNumber <= plantsToSpawn; plantNumber++)
            {
                randomCell = findRandomCell();

                if (randomCell == null)
                {
                    continue;
                }
                newPlant = new Plant(new int2(randomCell.getCoordinates().x, randomCell.getCoordinates().y));

                Transform newPlantTransform = newPlant.getObject().transform;
                newPlantTransform.SetParent(this._plantContainer.transform);
                
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

        public GameObject getObject()
        {
            return this._gridObject;
        }
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
        public void updateLivingEntitiesList()
        {
            if (this._livingEntities != null)
            {
                foreach(LivingEntity entity in this._livingEntities.ToArray())  // using ToArray() because list is constantly changing so we make a copy
                {
                    if(entity != null)
                    {   
                        entity.updateBrain();
                    }
                }
            }
        }
        public void updateLivingEntities()
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
                        if (cell.getEntity().GetType() == typeof(Plant))
                        {
                            Plant foundLivingEntity = (Plant)cell.getEntity();
                            foundLivingEntity.updateBrain();
                        }
                    }
                }
            }
        }

        public void updateLivingEntitiesEnergyClock(float energyConsumedOnUpdate)
        {
            if (this._livingEntities != null)
            {
                foreach(LivingEntity entity in this._livingEntities.ToArray())  // using ToArray() because list is constantly changing so we make a copy
                {
                    if(entity != null)
                    {   
                        entity.modifyEnergyBy(energyConsumedOnUpdate);
                        entity.updateStats();
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
            // DEPRECATED
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

        public void killLivingEntity(LivingEntity entity)
        {
            // remove all references so the garbage collector will trash this entity
            gridArray[entity.getCoordinates().x, entity.getCoordinates().y].setEntity(new NullEntity());
            this._livingEntities.Remove(entity);
        }
        public void destroySmell(SmellNode smellNode)
        {
            gridArray[smellNode.getCoordinates().x, smellNode.getCoordinates().y].destroySmell(smellNode);
        }
        public void addSmell(SmellNode smellNode)
        {
            gridArray[smellNode.getCoordinates().x, smellNode.getCoordinates().y].addSmell(smellNode);
        }

        public List<Entity> getVisibleEntities(Creature askingEntity)
        {
            int2 entityCoordinates = askingEntity.getCoordinates();
            int sightDistance = askingEntity.getGene_SightDistance();

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

        public void moveCreatureRandomly(int2 initialCreatureCoordinates, int2 creatureMoveDirection, Creature creature)
        {
            int2[] nextCellResult = getNextCellRaw(initialCreatureCoordinates, creatureMoveDirection);
            int2 cellCoordinates = nextCellResult[0];
            int2 newDirection = new int2(1, 0); // default direction
            
            if (cellCoordinates.x != -1 && cellCoordinates.y != -1 && nextCellResult.Length > 1)
            {
                //  if cell is free, move the creature
                // move creature to next cell
                gridArray[cellCoordinates.x, cellCoordinates.y].setEntity(creature);

                // update creature coordinates
                creature.setCoordinates(new int2(cellCoordinates.x, cellCoordinates.y));

                // remove creature from previous cell                                                        
                if (gridArray[initialCreatureCoordinates.x, initialCreatureCoordinates.y].getEntity() == creature)
                {
                    // this check is made for children born form the mother which will spawn in the same cell as the mother
                    // but will not be put in the cell's Entity attribute
                    gridArray[initialCreatureCoordinates.x, initialCreatureCoordinates.y].setEntity(new NullEntity());
                    newDirection = nextCellResult[1];
                    creature.setMoveDirection(newDirection);
                }
            }
            
        }
        public void moveCreatureTo(int2 initialCreatureCoordinates, int2 targetCoordinates, Creature creature)
        {
            if (targetCoordinates.x != -1 && targetCoordinates.y != -1)
            {
                //  if cell is free, move the creature
                // move creature to next cell
                gridArray[targetCoordinates.x, targetCoordinates.y].setEntity(creature);

                // update creature coordinates                             
                creature.setCoordinates(new int2(targetCoordinates.x, targetCoordinates.y));                               
                
                // remove creature from previous cell   
                if (gridArray[initialCreatureCoordinates.x, initialCreatureCoordinates.y].getEntity() == creature)
                {
                    // this check is made for children born form the mother which will spawn in the same cell as the mother
                    // but will not be put in the cell's Entity attribute
                    gridArray[initialCreatureCoordinates.x, initialCreatureCoordinates.y].setEntity(new NullEntity());     
                }          
            }
            
        }

        public Queue<int2> pathTo(int2 startCoordinates, int2 targetCoordinates)
        {
            return pathFinder.findPathTo(startCoordinates, targetCoordinates, this.gridArray);
        }

        /*private int2 getNextCellVector(int2 creatureCoordinates, Vector3 moveDirection)
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
        }*/

        private int2[] getNextCellRaw(int2 creatureCoordinates, int2 creatureMoveDirection)
        {
            

            const int max_tries = 8;
            int tries = 0;

            int2 newCellCoordinates;
            //int xOffset,yOffset;

            Redo:
            /*xOffset = UnityEngine.Random.Range(-1, 2);

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
            }*/
            // find index of direction
            int currentDirection = 0;
            for(int direction = 0; direction < this._moveDirections.Length; direction++)
            {
                if (creatureMoveDirection.x == this._moveDirections[direction].x && creatureMoveDirection.y == this._moveDirections[direction].y)
                {
                    currentDirection = direction;
                }
            }

            int offset = UnityEngine.Random.Range(-1, 2);
            int newDirectionIndex = currentDirection + offset;
            
            if (newDirectionIndex > 3)
            {
                newDirectionIndex = 0;
            }
            if (newDirectionIndex < 0)
            {
                newDirectionIndex = 3;
            }

            int2 newDirection = this._moveDirections[newDirectionIndex];

            tries++;
            newCellCoordinates = new int2(creatureCoordinates.x + newDirection.x, creatureCoordinates.y + newDirection.y);
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
                int2[] result = {newCellCoordinates, newDirection};
                return result;
            }
            else
            {
                int2[] result = {new int2(-1, -1)};
                return result;
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
