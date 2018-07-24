using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum state
{
    IDLE,
    MOVING,
    ATTACKING,
    STUNNED,
    SLOWED,
    WORKING,
    BUILDING,
    DEPLOYING,
    DEAD
}

public delegate void CheckQueue();
public delegate void StateChanged(state newState);

public class Unit : RtsObject {

    protected Icommand command;

    [SerializeField]
    protected state State;

    public event StateChanged StateChanged;

    private int id;

    protected float t;
    protected Vector3 startPosition;
    protected Vector3 target;
    protected float timeToReachTarget;
    protected List<Icommand> queueCommands = new List<Icommand>();
    protected event CheckQueue CheckQueue;

    public state GetState()
    {
        return State;
    }

    public int GetId()
    {
        return id;
    }

    public void SetState(state NewState)
    {
        State = NewState;
        if(StateChanged != null)
        {
            StateChanged(NewState);
        }
    }

    // Use this for initialization
    void Awake () {
        Initialized();
    }

    protected virtual void Initialized()
    {
        CheckQueue += CheckCommandQueue;
        StateChanged += OnChangedStates;
        SetState(state.IDLE);
    }

    protected virtual void OnChangedStates(state newState)
    {
        switch (State)
        {
            case state.IDLE:
                isMovable = true;
                isInteractable = true;
                break;
            case state.MOVING:
                isMovable = true;
                isInteractable = true;
                break;
            case state.WORKING:
                break;
            case state.BUILDING:
                isMovable = false;
                isInteractable = false;
                gameObject.GetComponent<Renderer>().enabled = false;
                StartCoroutine(BuildingLength(5f));
                break;
        }
    }

    public virtual void DeployPlacement()
    {
        SetState(state.DEPLOYING);
        startPosition = transform.position;
        target = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        timeToReachTarget = 5f;
        isMovable = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Command(Icommand command, bool addToQueue = true)
    {
        if (isInteractable)
        {
            switch (command.GetUnitCommand())
            {
                case (UnitCommands.Move):
                    if (IsMoveable())
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
                    if (this.unitType == UnitType.Worker || this.unitType == UnitType.Building)
                    {
                        BuildCommand(command);
                        if(addToQueue)
                        {
                            queueCommands.Add(command);
                            Debug.Log(queueCommands.Count + " COMMANDS");
                        }
                    }
                    break;
            }
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
        if (GetState() == state.BUILDING)
        {
            return;
        }
        bool atDestination = true;
        this.command = command;
        if(this.unitType == UnitType.Worker)
        {
            if (transform.position != command.GetPoint())
            {
                atDestination = false;
                GetComponent<Movement>().MoveUnit(command.GetPoint());
                StartCoroutine(AtBuildingPlacement(command.GetPoint(), atDestination));
            }
            else
            {
                CmdBuildBuilding(command.GetUnit().GetComponent<RtsObject>().GetItem().ID, command.GetPoint());
            }
        }
        else
        {
            CmdBuildBuilding(command.GetUnit().GetComponent<RtsObject>().GetItem().ID, command.GetPoint());
        }
    
    }

    [Command]
    public void CmdBuildBuilding(int id, Vector3 point)
    {
        if (this.isServer)
        {
            GameObject prefab = NetworkManager.singleton.spawnPrefabs[id];
            GameObject obj = Instantiate(prefab, new Vector3(point.x, point.y - 3, point.z), Quaternion.Euler(0, -210, 0)) as GameObject;
            obj.GetComponent<RtsObject>().Team = Team.One;
            NetworkServer.SpawnWithClientAuthority(obj, GameManager.main.connectionToClient);
            obj.GetComponent<Unit>().DeployPlacement();

            foreach(var item in GameManager.main.gameObject.GetComponent<Faction>().GetBuildableBuildings())
            {
                if(id == item.ID)
                {
                    obj.GetComponent<RtsObject>().SetItem(item);
                }
            }
        }
        RpcBuildBuilding();
    }

    [ClientRpc]
    public void RpcBuildBuilding()
    {
        SetState(state.BUILDING);
    }

    public IEnumerator AtBuildingPlacement(Vector3 buildingPlacement, bool atDestination)
    {
        while (Vector3.Distance(transform.position, buildingPlacement) > 1f)
        {
            yield return null;
        }
        CmdBuildBuilding(command.GetUnit().GetComponent<RtsObject>().GetItem().ID, command.GetPoint());
        GetComponent<Movement>().Hold();
    }

    public virtual void Deploying()
    {
        isMovable = false;
        isInteractable = false;
        gameObject.GetComponent<Renderer>().enabled = false;
        StartCoroutine(DeployLength(GetItem().BuildTime));
    }

    public IEnumerator DeployLength(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.GetComponent<Renderer>().enabled = true;
        SetState(state.IDLE);
    }

    protected IEnumerator BuildingLength(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.GetComponent<Renderer>().enabled = true;
        SetState(state.IDLE);
        CheckQueue();
    }

    protected void CheckCommandQueue()
    {
        queueCommands.RemoveAt(0);
        if(queueCommands.Count > 0)
        {
            Command(queueCommands[0], false);
        }

        foreach(var q in queueCommands)
        {
            Debug.Log(q.ToString());
        }
    }

}
