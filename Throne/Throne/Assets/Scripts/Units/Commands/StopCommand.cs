using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCommand : Command, Icommand
{

    public StopCommand()
    {
        unitCommand = UnitCommands.Hold;
        SetKeyBind(KeyCode.H);
    }

    public GameObject GetUnit()
    {
        throw new NotImplementedException();
    }

    public UnitCommands GetUnitCommand()
    {
        return unitCommand;
    }

    public void InitializeCommand()
    {
        foreach(SelectableUnit unit in SelectionManager.main.GetSelectedUnits())
        {
            unit.gameObject.GetComponent<Unit>().Command(this);
        }
    }

    public void KeyBindCommand()
    {
        InitializeCommand();
    }
}
