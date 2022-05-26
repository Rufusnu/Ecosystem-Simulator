using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EntityDomain
{
    public class Entity_AssetsService : MonoBehaviour
    {
        // Used to store all sprites related to GridTileSet and make them easy and fast to access

        public static Entity_AssetsService instance; // make the instance visble and usable
        private static bool _alive;

        private void Awake() {
            if (_alive == false)
            {
                instance = this;
                _alive = true;
            }
        }

        // ### [++] Sprite List [++] ###
        public Sprite creature_default;
        public Sprite creature_female;
        public Sprite creature_male;
        public Sprite plant_default;
        public Sprite smell_default;
        // ### [--] Sprite List [--] ###
    }
}