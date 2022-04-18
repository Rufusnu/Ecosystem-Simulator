using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace GridDomain
{
    public class NullTile : Tile
    {   // TO DO : ERROR : USING IT CREATES LOTS OF EMPTY OBJECTS
        // #### [++] Constructor [++] ####
        public NullTile() : base(GridTileSet_AssetService.instance.tile_default, new int2(0,0)) {}
        // #### [--]] Constructor [--] ####


        // #### [++] Getters & Setters [++] ####
        // ---- [++] Sprite [++] ---- 
        public override void setSprite(Sprite newSprite) {}
        public override Sprite getSprite()
        {
            return base.getSprite();
        }
        // ---- [--] Sprite [--] ---- 

        // ---- [++] Tile Object[++] ---- 
        public override void setObject(GameObject newTileObject) {}
        public override GameObject getObject()
        {
            return base.getObject();
        }
        // ---- [--] Tile Object [--] ---- 

        // ---- [++] Tile Object Name [++] ---- 
        public override void setObjectName(string newName) {}
        public override string getObjectName()
        {
            return base.getObjectName();
        }
        // ---- [--] Tile Object Name [--] ---- 
        // #### [++] Getters & Setters [++] ####
    }
}
