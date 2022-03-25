using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class Utils_Objects
    {
        // class used to provide utility functions
        public static Utils_Objects instance; // make the instance visble and usable

        private void Awake() {
            instance = this;
        }

        public void makeChildOfParent(GameObject toBeChild, GameObject toBeParent)
        {
            toBeChild.transform.SetParent(toBeParent.transform);
        }
    }
}