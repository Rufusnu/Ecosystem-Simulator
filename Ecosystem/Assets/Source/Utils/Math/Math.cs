using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityDomain;
using Unity.Mathematics;
using GridDomain;

namespace EcoMath
{
    public static class Math
    {
        // #### #### [++] Distance [++] #### ####
        public static float distanceBetween(Entity entity1, Entity entity2)
        {
            int2 coordinates_e1 = entity1.getCoordinates();
            int2 coordinates_e2 = entity2.getCoordinates();

            return Mathf.Sqrt(Mathf.Pow(coordinates_e2.x - coordinates_e1.x, 2) + Mathf.Pow(coordinates_e2.y - coordinates_e1.y, 2));
        }
        public static float distanceBetween(int2 coords1, int2 coords2)
        {
            return Mathf.Sqrt(Mathf.Pow(coords2.x - coords1.x, 2) + Mathf.Pow(coords2.y - coords1.y, 2));
        }
        public static float distanceBetween(Cell cell1, Cell cell2)
        {
            int2 coordinates_c1 = cell1.getCoordinates();
            int2 coordinates_c2 = cell2.getCoordinates();
            return Mathf.Sqrt(Mathf.Pow(coordinates_c2.x - coordinates_c1.x, 2) + Mathf.Pow(coordinates_c2.y - coordinates_c1.y, 2));
        }
        // #### #### [--] Distance [--] #### ####
    }
}
