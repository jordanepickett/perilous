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
            TryToBuild(new Vector3(0, 0, 0));
        }
        else
        {
            ReadyUpCommand();
        }
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
        if(isReadyForPlacement == true)
        {
            MouseManager.main.SetBuildingTobePlaced(GetUnit());
            MouseManager.main.SetState(MouseState.BUILDING_PLACEMENT);
        }
    }

    public void KeyBindCommand()
    {
        InitializeCommand();
    }

    private void TryToBuild(Vector3 potentialPlacement)
    {
        if(SelectionManager.main.FirstUnit().GetComponent<RtsObject>().unitType == UnitType.Worker)
        {
            if (isReadyForPlacement == true)
            {
                this.point = potentialPlacement;
                SelectionManager.main.FirstUnit().gameObject.GetComponent<Unit>().Command(this);
                UiManager.main.CheckFirstUnit();
                isReadyForPlacement = false;
                //changed this
                MouseManager.main.SetState(MouseState.DEFAULT);
            }
        }
        else
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
}
