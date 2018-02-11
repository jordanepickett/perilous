using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBuildCommand : Command, Icommand
{
    private bool isBuildingPanelOpen = false;

    public OpenBuildCommand()
    {
        unitCommand = UnitCommands.Hold;
        SetKeyBind(KeyCode.B);
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
        UiManager.main.CreateBuildingsPanel();
    }

    public void KeyBindCommand()
    {
        UiManager.main.CreateBuildingsPanel();
    }
}
