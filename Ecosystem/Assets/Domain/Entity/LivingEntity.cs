using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public abstract class LivingEntity : Entity
    {
        // can send signals to energy system and energy system decides what to do
        private float energy;

        public LivingEntity(Vector2Int newCoordinates) : base (newCoordinates) // used to call base class <Entity> constructor for inherited attributes
        {

        }

    }

}