using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EntityDomain
{
    public class Entity_AssetsService : MonoBehaviour
    {
        // Used to store all sprites related to GridTileSet and make them easy and fast to access

        public static Entity_AssetsService instance; // make the instance visble and usable

        private void Awake() {
            instance = this;
        }

        // ### [++] Sprite List [++] ###
        public Sprite creature_default;
        public Sprite creature_female;
        public Sprite creature_male;
        // ### [--] Sprite List [--] ###
    }
}