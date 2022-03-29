using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridDomain {
    public class GridTileSet_AssetService : MonoBehaviour
    {
        // Used to store all sprites related to GridTileSet and make them easy and fast to access

        public static GridTileSet_AssetService instance; // make the instance visble and usable

        private void Awake() {
            instance = this;
        }

        // ### [++] Sprite List [++] ###
        public Sprite tile_default;
        // ### [--] Sprite List [--] ###
    }
}
