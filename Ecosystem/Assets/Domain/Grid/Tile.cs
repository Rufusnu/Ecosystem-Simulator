using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private GameObject _tileObject;
    public Tile(Sprite newSprite)
    {
        this._tileObject = (GameObject) new GameObject();
        this._tileObject.AddComponent<SpriteRenderer>();
    }

    public void setSprite(Sprite newSprite)
    {
        this._tileObject.GetComponent<SpriteRenderer>().sprite = newSprite;
    }
}
