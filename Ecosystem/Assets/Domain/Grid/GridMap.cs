using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridDomain
{
    public class GridMap
    {
        // #### [++] Attributes [++] #### 
        private int _cellSize;
        private int _cols;
        private int _rows;
        private Cell[,] gridArray;
        // #### [--] Attributes [--] #### 


        // #### [++] Constructor [++] ####
        public GridMap(int rows, int cols, int cellSize)
        {
            setRowsNumber(rows);
            setColsNumber(cols);
            setCellSize(cellSize);
            gridArray = new Cell[rows, cols];
            
            // create a <Cell> and <Tile> pointing to null to change its value for constructing the array 
            Cell cell = null;
            Tile tile = null;

            for (int row = 0; row < gridArray.GetLength(0); row++)
            {
                for (int col = 0; col < gridArray.GetLength(1); col++)
                {
                    try {
                        // creating coordinates for the <Cell> and assigning it to the corresponding array position
                        Vector2Int coordinates = new Vector2Int(row, col);

                        // creating tile to insert into cell; setting its sprite from the Asset Service Class
                        tile = new Tile(GridTileSet_AssetService.instance.tile_default);
                        tile.setObjectName("Tile " + coordinates + "");
                        
                        cell = (Cell) new Cell(coordinates, this._cellSize);
                        cell.setTile(tile);
                        gridArray[row, col] = cell;
                    } catch (System.Exception exception) {
                        throw new System.Exception("<GridMap> ->" + exception);
                    }
                }
            }
        }
        // #### [--] Constructor [--] ####


        // #### [++] Getters & Setters [++] #### 
        // ---- [++] Cell Size [++] ----
        public int getCellSize()
        {
            return this._cellSize;
        }
        private void setCellSize(int newCellSize)
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
                throw new System.Exception("<GridMap> Cannot set height/width with a value less than 8");
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
                throw new System.Exception("<GridMap> Cannot set height/width with a value less than 8");
            }
            this._cols = newCols;
        }
        // ---- [--] Cols [--] ----
        // #### [--] Getters & Setters [--] #### 
    }
}
