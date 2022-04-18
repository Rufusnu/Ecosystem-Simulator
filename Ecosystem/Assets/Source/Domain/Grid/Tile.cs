using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace GridDomain 
{
    public class Tile
    {
        // #### [++] Attributes [++] ####
        private GameObject _tileObject = null; // Used to set Tile Sprite
        private SpriteRenderer _tileObjectSpriteRederer; // Tile Sprite
        // #### [--] Attributes [--] ####


        // #### [++] Constructor [++] ####
        public Tile(Sprite newSprite, int2 coordinates)
        {
            if (newSprite == null)
            { // if the sprite is invalid -> select the default sprite
                newSprite = GridTileSet_AssetService.instance.tile_default;
                Debug.Log("<Tile> Sprite was not found. Falling back to default sprite.");
            }

            this._tileObject = new GameObject();
            setObjectName("Tile " + coordinates);
            this._tileObjectSpriteRederer = this._tileObject.AddComponent<SpriteRenderer>();
            setSprite(newSprite);
        }
        // #### [--]] Constructor [--] ####


        // #### [++] Getters & Setters [++] ####
        // ---- [++] Sprite [++] ---- 
        public virtual void setSprite(Sprite newSprite)
        {
            if (newSprite == null)
            {
                throw new System.Exception("<Tile>: Cannot set null sprite");
            }
            this._tileObjectSpriteRederer.sprite = newSprite;
        }
        public virtual Sprite getSprite()
        {
            return this._tileObjectSpriteRederer.sprite;
        }
        // ---- [--] Sprite [--] ---- 

        // ---- [++] Tile Object[++] ---- 
        public virtual void setObject(GameObject newTileObject)
        {
            if (newTileObject == null)
            {
                throw new System.Exception("<Tile>: Cannot set null GameObject");
            }
            this._tileObject = newTileObject;
        }
        public virtual GameObject getObject()
        {
            return this._tileObject;
        }
        // ---- [--] Tile Object [--] ---- 

        // ---- [++] Tile Object Name [++] ---- 
        public virtual void setObjectName(string newName)
        {
            if (newName == null)
            {
                throw new System.Exception("<Tile>: Cannot set null name");
            }
            this._tileObject.name = newName;
        }
        public virtual string getObjectName()
        {
            return this._tileObject.name;
        }
        // ---- [--] Tile Object Name [--] ---- 
        // #### [++] Getters & Setters [++] ####
    }
}
