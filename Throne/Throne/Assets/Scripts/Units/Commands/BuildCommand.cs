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
        if(SelectionManager.main.FirstUnit().GetComponent<RtsObject>().unitType == UnitType.Worker)
        {
            EventsManager.main.LeftMouseClick += TryToBuild;
            EventsManager.main.RightMouseClick += CancelTryToBuild;
        }
        unitCommand = UnitCommands.Build;
        SetKeyBind(keyBind);
    }

    public void InitializeCommand()
    {
        if (SelectionManager.main.FirstUnit().GetComponent<RtsObject>().unitType == UnitType.Building)
        {
            isReadyForPlacement = true;
            TryToBuild(new RaycastHit());
        }
        else
        {
            ReadyUpCommand();
        }
    }

    void ReadyUpCommand()
    {
        if (MouseManager.main.GetState() != MouseState.BUILDING_PLACEMENT)
        {
            if (isReadyForPlacement == true)
            {
                isReadyForPlacement = false;
            }
            else
            {
                isReadyForPlacement = true;
            }
            if (isReadyForPlacement == true)
            {
                MouseManager.main.SetBuildingTobePlaced(GetUnit().GetComponent<RtsObject>().GetItem().PrefabPlacement);
                if(GetUnit().GetComponent<Building>().buildingType == BuildingType.MAIN)
                {
                    GetUnit().GetComponent<RtsObject>().GetItem().PrefabPlacement.GetComponentInChildren<PlacementTrigger>().isMainBuilding = true;
                }
                MouseManager.main.SetState(MouseState.BUILDING_PLACEMENT);
            }
        }
    }

    public void KeyBindCommand()
    {
        InitializeCommand();
    }

    private void TryToBuild(RaycastHit potentialPlacement)
    {
        Vector3 position = potentialPlacement.point;
        if(!CanBuildUnit())
        {
            return;
        }
        if(SelectionManager.main.FirstUnit().GetComponent<RtsObject>().unitType == UnitType.Worker && MouseManager.main.IsLegalPosition())
        {
            if (isReadyForPlacement == true)
            {
                this.point = position;
                SelectionManager.main.FirstUnit().gameObject.GetComponent<Unit>().Command(this);
                UiManager.main.CheckFirstUnit();
                isReadyForPlacement = false;
                //changed this
                MouseManager.main.SetState(MouseState.DEFAULT);
            }
        }
        else if(SelectionManager.main.FirstUnit().GetComponent<RtsObject>().unitType == UnitType.Building)
        {
            if (isReadyForPlacement == true)
            {
                Vector3 playerPos = new Vector3(SelectionManager.main.FirstUnit().transform.position.x,
                    SelectionManager.main.FirstUnit().transform.position.y + 3,
                    SelectionManager.main.FirstUnit().transform.position.z);
                Vector3 playerDirection = SelectionManager.main.FirstUnit().transform.forward;
                Quaternion playerRotation = SelectionManager.main.FirstUnit().transform.rotation;
                float spawnDistance = 1;

                Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
                this.point = spawnPos;
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

    private void CancelTryToBuild(Vector3 point)
    {
        isReadyForPlacement = false;
        MouseManager.main.SetState(MouseState.DEFAULT);
    }

    public GameObject GetUnit()
    {
        return Unit;
    }

    private bool CanBuildUnit()
    {
        if(!SelectionManager.main.FirstUnit())
        {
            return false;
        }
        if(!GetUnit().GetComponent<RtsObject>().GetItem().isAvailable)
        {
            return false;
        }
        PlayerUnitController player = SelectionManager.main.FirstUnit().GetComponent<RtsObjectController>().GetPlayer().GetComponent<PlayerUnitController>();
        int totalGold = player.GetComponent<PlayerController>().gold;
        int totalLumber = player.GetComponent<PlayerController>().lumber;

        if(GetUnit().GetComponent<Unit>().unitType != UnitType.Building && (GetUnit().GetComponent<Unit>().GetItem().food + player.unitsExisting) > player.allowedUnits)
        {
            return false;
        }

        if(GetUnit().GetComponent<Unit>().GetItem().Cost > totalGold || GetUnit().GetComponent<Unit>().GetItem().Lumber > totalLumber)
        {
            return false;
        }

        return true;
    }

    public int GetBuildTime()
    {
        return (int)GetUnit().GetComponent<Unit>().GetItem().BuildTime;
    }
}
