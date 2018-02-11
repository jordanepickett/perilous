using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum state
{
    IDLE,
    MOVING,
    STUNNED,
    SLOWED,
    WORKING,
    BUILDING,
    DEAD
}

public class Unit : RtsObject {

    private Icommand command;

    protected state State;

    public state GetState()
    {
        return State;
    }

    public void SetState(state NewState)
    {
        State = NewState;
    }

    // Use this for initialization
    void Start () {
		//GetComponent<Movement>().PositionReached += 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Command(Icommand command)
    {
        switch(command.GetUnitCommand())
        {
            case (UnitCommands.Move):
                if(IsMoveable())
                {
                    MoveCommand(command);
                }
                break;
            case (UnitCommands.Hold):
                if (IsMoveable())
                {
                    HoldCommand();
                }
                break;
            case (UnitCommands.Build):
                if (this.unitType == UnitType.Worker)
                {
                    BuildCommand(command);
                }
                break;
        }

    }

    void MoveCommand(Icommand command)
    {
        GetComponent<Movement>().MoveUnit(command.GetPoint());
    }

    void HoldCommand()
    {
        GetComponent<Movement>().Hold();
    }

    void BuildCommand(Icommand command)
    {
        bool atDestination = true;
        this.command = command;
        if (transform.position != command.GetPoint())
        {
            atDestination = false;
            GetComponent<Movement>().MoveUnit(command.GetPoint());
            StartCoroutine(AtBuildingPlacement(command.GetPoint(), atDestination));
        }
        else
        {
            CmdBuildBuilding();
        }
    }

    [Command]
    public void CmdBuildBuilding()
    {
        RpcBuildBuilding();
    }

    [ClientRpc]
    public void RpcBuildBuilding()
    {
        if (this.isServer)
        {
            //GameObject obj = Instantiate(Resources.Load("Models/FactionA/Units/Buildings/Town Hall/TownHall", typeof(GameObject)) as GameObject);
            //NetworkServer.SpawnWithClientAuthority(obj, this.connectionToClient);
            GameObject obj = Instantiate(command.GetUnit(), command.GetPoint(), Quaternion.Euler(0, -30, 0));
        }
        SetState(state.BUILDING);
    }

    public IEnumerator AtBuildingPlacement(Vector3 buildingPlacement, bool atDestination)
    {
        while (Vector3.Distance(transform.position, buildingPlacement) > 1f)
        {
            Debug.Log(transform.position);
            yield return null;
        }
        CmdBuildBuilding();
        GetComponent<Movement>().Hold();
    }

}
