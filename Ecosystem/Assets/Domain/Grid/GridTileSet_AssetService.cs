using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileSet_AssetService : MonoBehaviour
{
    public static GridTileSet_AssetService instance;
    public Sprite tile_default;
    private void Awake() {
        instance = this;
    }
}
