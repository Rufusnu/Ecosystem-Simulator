using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace EntityDomain
{
    public sealed class NullEntity : Entity
    {
        // #### [++] Initialization [++] ####
        public void Initialize()
        {
            base.Initialize(new int2(0, 0));
        }
        // #### [--] Initialization [--] ####


        // #### [++] Getters & Setters [++] ###
        // ---- [++] Coordinates [++] ---- 
        public override int2 getCoordinates()
        {
            return base.getCoordinates();
        }
        public override void setCoordinates(int2 newCoordinates) {}
        // ---- [--] Coordinates [--] ---- 
        // #### [--] Getters & Setters [--] ####
    }
}
