using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Energy;

namespace EntityDomain
{
    public enum Actions
    {
        // Creature
        Think_Creature,
        Move_Creature,
        Eat_Creature,
        Drink_Creature,
        Hunt_Creature,
        Flee_Creature,
        Mate_Creature

    }
    public class ActionType
    {
        public Actions actionType;
        public float energyNeeded; 

        public ActionType(Actions newActionType, float newEnergyNeeded)
        {
            this.actionType = newActionType;
            this.energyNeeded = newEnergyNeeded;
        }
    }

    public class CreatureActions
    {
        public static ActionType Think = new ActionType(Actions.Think_Creature, EnergySystem.ThinkConsumption);
        public static ActionType Move = new ActionType(Actions.Move_Creature, EnergySystem.MoveConsumption);
        public static ActionType Eat = new ActionType(Actions.Eat_Creature, 0);
        public static ActionType Drink = new ActionType(Actions.Drink_Creature, 0);
        public static ActionType Hunt = new ActionType(Actions.Hunt_Creature, 0);
        public static ActionType Flee = new ActionType(Actions.Flee_Creature, 0);
        public static ActionType Mate = new ActionType(Actions.Mate_Creature, EnergySystem.MateConsumption);
    }
}