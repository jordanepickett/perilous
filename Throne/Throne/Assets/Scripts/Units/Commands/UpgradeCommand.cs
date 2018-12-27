using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCommand : Command, Icommand
{

    private bool isBuildingPanelOpen;
    private bool isReadyForPlacement = false;

    public UnitCommands GetUnitCommand()
    {
        return unitCommand;
    }

    public UpgradeCommand(KeyCode keyBind)
    {
        unitCommand = UnitCommands.Upgrade;
        SetKeyBind(keyBind);
    }

    public void InitializeCommand()
    {
        isReadyForPlacement = true;
        TryToUpgrade();
    }

    public void KeyBindCommand()
    {
        InitializeCommand();
    }

    private void TryToUpgrade()
    {
        if (!CanBuildUnit())
        {
            return;
        }
        if (SelectionManager.main.FirstUnit().GetComponent<RtsObject>().unitType == UnitType.Building)
        {
            if (isReadyForPlacement == true)
            {
                SelectionManager.main.FirstUnit().gameObject.GetComponent<Unit>().Command(this);
                UiManager.main.CheckFirstUnit();
                isReadyForPlacement = false;
                //changed this
                MouseManager.main.SetState(MouseState.DEFAULT);
            }
        }
        else
        {
            return;
        }
    }

    public GameObject GetUnit()
    {
        return Unit;
    }

    private bool CanBuildUnit()
    {
        if (!SelectionManager.main.FirstUnit())
        {
            return false;
        }
        if (!GetUnit().GetComponent<RtsObject>().GetItem().isAvailable)
        {
            return false;
        }
        PlayerUnitController player = SelectionManager.main.FirstUnit().GetComponent<RtsObjectController>().GetPlayer().GetComponent<PlayerUnitController>();
        int totalGold = player.GetComponent<PlayerController>().gold;
        int totalLumber = player.GetComponent<PlayerController>().lumber;

        if (GetUnit().GetComponent<Unit>().unitType != UnitType.Building && (GetUnit().GetComponent<Unit>().GetItem().food + player.unitsExisting) > player.allowedUnits)
        {
            return false;
        }

        if (GetUnit().GetComponent<Upgrade>().Costruct.Gold > totalGold || GetUnit().GetComponent<Upgrade>().Costruct.Lumber > totalLumber)
        {
            return false;
        }

        return true;
    }

    public Upgrade GetUpgrade()
    {
        return GetUnit().GetComponent<Upgrade>();
    }
}
