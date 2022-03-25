using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridDomain
{
    public class GridMap
    {
        // #### [++] Attributes [++] ####
        private GameObject _gridObject = null; 
        private float _cellSize;
        private int _cols;
        private int _rows;
        private Cell[,] gridArray;
        // #### [--] Attributes [--] #### 


        // #### [++] Constructor [++] ####
        public GridMap(int rows, int cols, float cellSize)
        {
            this._gridObject = new GameObject("Grid");
            setRowsNumber(rows);
            setColsNumber(cols);
            setCellSize(cellSize);
            gridArray = new Cell[cols, rows];
            
            // create a <Cell> and <Tile> pointing to null to change its value for constructing the array 
            Cell cell = null;
            Tile tile = null;

            for (int col = 0; col < gridArray.GetLength(0); col++)
            {
                for (int row = 0; row < gridArray.GetLength(1); row++)
                {
                    try {
                        // creating coordinates for the <Cell> and assigning it to the corresponding array position
                        Vector2Int coordinates = new Vector2Int(col, row);

                        // creating tile to insert into cell; setting its sprite from the Asset Service Class
                        tile = new Tile(GridTileSet_AssetService.instance.tile_default);
                        tile.setObjectName("Tile " + coordinates + "");
                        
                        cell = (Cell) new Cell(coordinates, this._cellSize);
                        cell.setTile(tile);
                        cell.getObject().transform.SetParent(this._gridObject.transform); // needs to be replaced by a set parent function between objects to be simpler
                        gridArray[col, row] = cell;
                    } catch (System.Exception exception) {
                        throw new System.Exception("<GridMap> ->" + exception);
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
        // ---- [--] Cell Size [--] ----


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
        // #### [--] Getters & Setters [--] ####

        // ---- [++] Change Grid Position [++] ----
        public void positionTo(Vector3 newPosition)
        {
            if (newPosition == null)
            {
                throw new System.Exception("<GridMap> Cannot set null position.");
            }
            this._gridObject.transform.position = newPosition;
        }
        // ---- [++] Change Grid Position [++] ----
    }
}
