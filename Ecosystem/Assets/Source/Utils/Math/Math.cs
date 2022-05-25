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

        public static Vector3 gridToWorldCoordinates(int2 coordinates, GameObject gameObject)
        {
            return new Vector3(coordinates.x * Configs.GridCellSize(), coordinates.y * Configs.GridCellSize(), gameObject.transform.localPosition.z);
        }

        
        // #### #### [++] Creature Direction [++] #### ####
        public static Vector3 rotate(Vector3 vector, float degrees)
        {   
            // dont think it works properly
            float vectorDegrees = CalculateAngle(vector, Vector3.right); // calculate the angle between this vector and world 0 rotation
            float newVectorDegrees = vectorDegrees;
            
            if (Configs.Debugging())
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
        public static float CalculateAngle(Vector3 from, Vector3 to)
        {
            //return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
            float angle = Vector3.Angle(from, to);
            return (Vector3.Angle(Vector3.left, to) > 90f) ? 360f - angle : angle;   
        }
        // #### #### [--] Creature Direction [--] #### ####
    }
}
