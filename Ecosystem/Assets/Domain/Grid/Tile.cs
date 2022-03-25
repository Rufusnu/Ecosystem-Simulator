using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridDomain 
{
    public class Tile
    {
        // #### [++] Attributes [++] ####
        private GameObject _tileObject; // Used to set Tile Sprite
        private SpriteRenderer _tileObjectSpriteRederer; // Tile Sprite
        // #### [--] Attributes [--] ####


        // #### [++] Constructor [++] ####
        public Tile(Sprite newSprite)
        {
            this._tileObject = (GameObject) new GameObject();
            this._tileObjectSpriteRederer = this._tileObject.AddComponent<SpriteRenderer>();
            setSprite(newSprite);
        }
        // #### [--]] Constructor [--] ####


        // #### [++] Getters & Setters [++] ####
        // ---- [++] Sprite [++] ---- 
        public void setSprite(Sprite newSprite)
        {
            if (newSprite == null)
            {
                throw new System.Exception("<Tile>: Cannot set null sprite");
            }
            this._tileObjectSpriteRederer.sprite = newSprite;
        }
        public Sprite getSprite()
        {
            return this._tileObjectSpriteRederer.sprite;
        }
        // ---- [--] Sprite [--] ---- 

        // ---- [++] Tile Object[++] ---- 
        public void setObject(GameObject newTileObject)
        {
            if (newTileObject == null)
            {
                throw new System.Exception("<Tile>: Cannot set null GameObject");
            }
            this._tileObject = newTileObject;
        }
        public GameObject getObject()
        {
            return this._tileObject;
        }
        // ---- [--] Tile Object [--] ---- 

        // ---- [++] Tile Object Name [++] ---- 
        public void setObjectName(string newName)
        {
            if (newName == null)
            {
                throw new System.Exception("<Tile>: Cannot set null name");
            }
            this._tileObject.name = newName;
        }
        public string getObjectName()
        {
            return this._tileObject.name;
        }
        // ---- [--] Tile Object Name [--] ---- 
        // #### [++] Getters & Setters [++] ####
    }
}
