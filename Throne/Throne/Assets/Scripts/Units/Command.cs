using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command {
    
    public UnitCommands UnitCommand
    {
        get;
        private set;
    }

    public Vector3 Location
    {
        get;
        private set;
    }

    public RtsObject RtsObject
    {
        get;
        private set;
    }

    public Command(UnitCommands command)
    {
        UnitCommand = command;
    }

    public Command(UnitCommands command, Vector3 location)
    {
        UnitCommand = command;
        Location = location;
    }

    public Command(UnitCommands command, RtsObject obj)
    {
        UnitCommand = command;
        RtsObject = obj;
    }
}
