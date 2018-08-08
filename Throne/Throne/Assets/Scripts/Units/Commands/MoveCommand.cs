using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Command, Icommand {

    private bool isReadyForLocation = false;

    public void SetXPoint(float newPoint)
    {
        point.x = newPoint;
    }

    public UnitCommands GetUnitCommand()
    {
        return unitCommand;
    }

    public void SetUnitCommand(UnitCommands newCommand)
    {
        unitCommand = newCommand;
    }

    public MoveCommand()
    {
        EventsManager.main.LeftMouseClick += SendCommand;
        EventsManager.main.RightMouseClick += GiveMovementCommand;
        unitCommand = UnitCommands.Move;
        SetKeyBind(KeyCode.M);
    }

    public void InitializeCommand()
    {
        ReadyUpCommand();
    }

    void ReadyUpCommand()
    {
        if (isReadyForLocation == true)
        {
            isReadyForLocation = false;
        }
        else
        {
            isReadyForLocation = true;
        }
    }

    void SendCommand(Vector3 point)
    {
        if (isReadyForLocation)
        {
            this.point = point;
            MovementManager.main.GiveMovementCommand(this);
            isReadyForLocation = false;
        }
    }

    void GiveMovementCommand(Vector3 point)
    {
        Debug.Log("MOVE COMMAND");
        this.point = point;
        MovementManager.main.GiveMovementCommand(this);
    }

    public void KeyBindCommand()
    {
        InitializeCommand();
    }

    public GameObject GetUnit()
    {
        throw new NotImplementedException();
    }
}
