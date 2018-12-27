using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherCommand : Command, Icommand
{

    private bool isReadyForAttack = false;

    public void SetXPoint(float newPoint)
    {
        point.x = newPoint;
    }

    public void SetPoint(Vector3 point)
    {
        this.point = point;
    }

    public UnitCommands GetUnitCommand()
    {
        return unitCommand;
    }

    public void SetUnitCommand(UnitCommands newCommand)
    {
        unitCommand = newCommand;
    }

    public GatherCommand()
    {
        EventsManager.main.RightMouseClickNode += GiveGatherCommand;
        unitCommand = UnitCommands.Gather;
        SetKeyBind(KeyCode.G);
    }

    public void InitializeCommand()
    {
        ReadyUpCommand();
    }

    void ReadyUpCommand()
    {
        if (isReadyForAttack == true)
        {
            isReadyForAttack = false;
        }
        else
        {
            isReadyForAttack = true;
        }
    }

    void SendCommand(RaycastHit point)
    {
        if (isReadyForAttack)
        {
            CommandInfo(point);
        }
    }

    void GiveGatherCommand(RaycastHit point)
    {
        if(point.collider.gameObject.GetComponent<GatherNode>())
        {
            Debug.Log("COMMAND GIVEN");
            CommandInfo(point);
        }
    }

    void CommandInfo(RaycastHit point)
    {
        if (point.collider.gameObject.GetComponent<GatherNode>())
        {
            SetUnit(point.collider.gameObject);
            this.point = point.collider.gameObject.transform.position;
            SelectionManager.main.GiveAllUnitsCommand(this);
        }
    }

    public void KeyBindCommand()
    {
        InitializeCommand();
    }

    public GameObject GetUnit()
    {
        return Unit;
    }
}

