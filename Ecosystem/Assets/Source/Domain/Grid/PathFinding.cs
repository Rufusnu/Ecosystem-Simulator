using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using GridDomain;

namespace PathFinding
{
    // #### #### Strategy Pattern #### ####
    public abstract class PathFinder
    {
        public abstract Queue<int2> findPathTo(int2 startCoordinate, int2 targetCoordinate, Cell[,] gridArray);
    }

    // #### #### [++] Pathfinding Algorithms [++] #### ####
    public class Lee : PathFinder
    {
        // Pathfinding using Lee's Algorithm

        int[] dl = {-1, 0, 1, 0}; // these arrays will help you travel in the 4 directions more easily
        int[] dc = {0, 1, 0, -1};

        public override Queue<int2> findPathTo(int2 startCoordinate, int2 targetCoordinate, Cell[,] gridArray)
        {
            Queue<int2> path = new Queue<int2>();
            int[,] mat = new int[gridArray.GetLength(0), gridArray.GetLength(1)];

            path.Enqueue(startCoordinate); // initialize the queue with the start position
            initMat(mat, gridArray);

            lee(path, targetCoordinate, mat);
            return path;
        }

        private void initMat(int[,] mat, Cell[,] gridArray)
        {
            for(int i = 0; i < mat.GetLength(0); i++)
            {
                for(int j = 0; j < mat.GetLength(1); j++)
                {
                    mat[i,j] = 0;
                }
            }
        }
    
        private void lee(Queue<int2> path, int2 targetCoordinate, int[,] mat)
        {
            int x, y, xx, yy;
            while(path.Count != 0) // while there are still positions in the queue
            {
                x = path.Peek().x; // set the current position
                y = path.Peek().y;
                for(int i = 0; i < 4; i++)
                {
                    xx = x + dl[i]; // travel in an adiacent cell from the current position
                    yy = y + dc[i];
                    if(GridMap.currentGridInstance.isCellFree(new int2(xx, yy)))
                    {
                        path.Enqueue(new int2(xx, yy));
                        mat[xx,yy] = -1; // mark that you have been to this position in the matrix
                    }
                }
                path.Dequeue();   
            }
        }
    }

    public class A_Star : PathFinder
    {
        // Pathfinding using A* Algorithm
        // without diagonals
        public override Queue<int2> findPathTo(int2 startCoordinate, int2 targetCoordinate, Cell[,] gridArray)
        {
            Queue<int2> path = new Queue<int2>();
            Grid grid = new Grid(gridArray, targetCoordinate);
            FindPath(startCoordinate, targetCoordinate, grid);
            path = grid.GetPathAsInt2();
            return path;
        }

        void FindPath(int2 startPos, int2 targetPos, Grid grid)
        {
            Node startNode = grid.grid[startPos.x, startPos.y];
            Node targetNode = grid.grid[targetPos.x, targetPos.y];

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node node = openSet[0];
                for (int i = 1; i < openSet.Count; i ++)
                {
                    if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                    {
                        if (openSet[i].hCost < node.hCost)
                            node = openSet[i];
                    }
                }

                openSet.Remove(node);
                closedSet.Add(node);

                if (node == targetNode)
                {
                    RetracePath(startNode,targetNode, grid);
                    return;
                }

                foreach (Node neighbour in grid.GetNeighbours(node))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }

        void RetracePath(Node startNode, Node endNode, Grid grid)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();

            grid.path = path;

        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            //if (dstX > dstY)
                //return 14*dstY + 10* (dstX-dstY);
            return dstX + dstY; //14*dstX + 10 * (dstY-dstX);
        }
    }


    public class BresenhamLine : PathFinder
    {
        // Pathfinding using BresenhamLine Algorithm
        public override Queue<int2> findPathTo(int2 startCoordinate, int2 targetCoordinate, Cell[,] gridArray)
        {
            Queue<int2> path = new Queue<int2>(GetPath(startCoordinate, targetCoordinate));
            return path;
        }

        public static List<int2> GetPath (int2 start, int2 end)
        {
            // bresenham line algorithm
            int w = end.x - start.x;
            int h = end.y - start.y;
            int absW = System.Math.Abs (w);
            int absH = System.Math.Abs (h);

            // Is neighbouring tile
            if (absW == 1 && absH == 0 || absW == 0 && absH == 1)
            {
                return new List<int2>();
            }

            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0)
            {
                dx1 = -1;
                dx2 = -1;
            }
            else if (w > 0)
            {
                dx1 = 1;
                dx2 = 1;
            }

            if (h < 0)
            {
                dy1 = -1;
            }
            else if (h > 0)
            {
                dy1 = 1;
            }

            int longest = absW;
            int shortest = absH;
            if (longest <= shortest)
            {
                longest = absH;
                shortest = absW;
                if (h < 0)
                {
                    dy2 = -1;
                }
                else if (h > 0)
                {
                    dy2 = 1;
                }
                dx2 = 0;
            }

            int numerator = longest >> 1;
            List<int2> path = new List<int2>();
            for (int i = 1; i <= longest; i++)
            {
                numerator += shortest;
                if (numerator >= longest)
                {
                    numerator -= longest;
                    start.x += dx1;
                    start.y += dy1;
                }
                else
                {
                    start.x += dx2;
                    start.y += dy2;
                }

                // If not walkable, path is invalid so return null
                // (unless is target tile, which may be unwalkable e.g water)
                if (i != longest && !GridMap.currentGridInstance.isCellFree(new int2(start.x,start.y)))
                {
                    return new List<int2>();
                }
                path.Add(new int2(start.x, start.y));
            }
            foreach(int2 coord in path)
            {
                Debug.Log(coord);
            }
            return path;
        }
    }
    // #### #### [--] Pathfinding Algorithms [--] #### ####



    // #### #### [++] A_STAR classes [++] #### ####
    public class Node 
    {
        public bool walkable;
        public int gridX;
        public int gridY;

        public int gCost;
        public int hCost;
        public Node parent;
        
        public Node(bool _walkable, int _gridX, int _gridY)
        {
            walkable = _walkable;
            gridX = _gridX;
            gridY = _gridY;
        }

        public int fCost
        {
            get{
                return gCost + hCost;
            }
        }
    }

    public class Grid
    {
        public List<Node> path;
        public Node[,] grid;
        int gridSizeX, gridSizeY;
        int2 target;

        public Grid(Cell [,] gridArray, int2 target)
        {
            this.target = target;
            path = new List<Node>();
            gridSizeX = gridArray.GetLength(0);
            gridSizeY = gridArray.GetLength(1);
            grid = new Node[gridSizeX, gridSizeY];

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    bool walkable;
                    if (x != target.x || y != target.y)
                        walkable = GridMap.currentGridInstance.isCellFree(new int2(x, y));
                    else
                        walkable = true; // is the target
                    grid[x,y] = new Node(walkable, x, y);
                }
            }
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (Mathf.Abs(x) == Mathf.Abs(y))
                    {
                        continue;
                    }
                        
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX,checkY]);
                    }
                }
            }
            return neighbours;
        }

        public Queue<int2> GetPathAsInt2()
        {
            Queue<int2> localPath = new Queue<int2>();
            if (path.Count > 0)
            {
                foreach(Node node in path)
                {
                    localPath.Enqueue(new int2(node.gridX, node.gridY));
                }
            }
            
            return localPath; 
        }
    }
    // #### #### [--] A_STAR classes [--] #### ####
}
