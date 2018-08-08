using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : Command, Icommand
{

    private bool isReadyForAttack = false;

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

    public AttackCommand()
    {
        EventsManager.main.RightMouseClickObject += GiveAttackCommand;
        unitCommand = UnitCommands.Attack;
        SetKeyBind(KeyCode.A);
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
            AttackInfo(point);
        }
    }

    void GiveAttackCommand(RaycastHit point)
    {
        AttackInfo(point);
    }

    void AttackInfo(RaycastHit point)
    {
        if (point.collider.gameObject.GetComponent<RtsObjectController>())
        {
            Debug.Log("HERE");
            if (point.collider.gameObject.GetComponent<RtsObjectController>().Team != SelectionManager.main.FirstUnit().GetComponent<RtsObjectController>().Team)
            {
                SetUnit(point.collider.gameObject);
                SelectionManager.main.GiveAllUnitsCommand(this);
            }
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

