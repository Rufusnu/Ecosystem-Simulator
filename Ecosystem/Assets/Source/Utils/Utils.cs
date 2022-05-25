using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Utils_Objects
    {
        // class used to provide utility function

        public static void makeChildOfParent(GameObject toBeChild, GameObject toBeParent)
        {
            toBeChild.transform.SetParent(toBeParent.transform);
        }
    }
}