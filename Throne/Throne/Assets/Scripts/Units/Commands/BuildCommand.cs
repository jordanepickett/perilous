using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCommand : Command, Icommand
{

    private bool isBuildingPanelOpen;
    private bool isReadyForPlacement = false;

    public UnitCommands GetUnitCommand()
    {
        return unitCommand;
    }

    public BuildCommand(KeyCode keyBind)
    {
        EventsManager.main.LeftMouseClick += TryToBuild;
        EventsManager.main.RightMouseClick += CancelTryToBuild;
        unitCommand = UnitCommands.Build;
        SetKeyBind(keyBind);
    }

    public void InitializeCommand()
    {
        ReadyUpCommand();
    }

    void ReadyUpCommand()
    {
        if (isReadyForPlacement == true)
        {
            isReadyForPlacement = false;
        }
        else
        {
            isReadyForPlacement = true;
        }
    }

    public void KeyBindCommand()
    {
        InitializeCommand();
    }

    private void TryToBuild(Vector3 potentialPlacement)
    {
        if(isReadyForPlacement == true)
        {
            this.point = potentialPlacement;
            SelectionManager.main.FirstUnit().gameObject.GetComponent<Unit>().Command(this);
            UiManager.main.CheckFirstUnit();
            isReadyForPlacement = false;
        }
    }

    private void CancelTryToBuild(Vector3 point)
    {
        isReadyForPlacement = false;
    }

    public GameObject GetUnit()
    {
        return Unit;
    }
}
