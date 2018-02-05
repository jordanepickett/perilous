using UnityEngine;
using System.Collections;

public static class Commands
{

    public static Command CreateHoldOrder()
    {
        return new Command(UnitCommands.Hold);
    }

    public static Command CreateMoveOrder(Vector3 location)
    {
        return new Command(UnitCommands.Move, location);
    }

    public static Command CreateAttackOrder(RtsObject obj)
    {
        return new Command(UnitCommands.Attack, obj);
    }

    //public static Command CreateDeployOrder()
    //{
    //    return new Order("Deploy", 3);
    //}
}
