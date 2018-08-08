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
    GATHERING,
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
    public GameManager manager;

    private int id;

    protected float t;
    protected Vector3 startPosition;
    protected Vector3 target;
    protected float timeToReachTarget;
    protected List<Icommand> queueCommands = new List<Icommand>();
    protected event CheckQueue CheckQueue;

    protected int buildSeconds;

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
        gameObject.AddComponent<FactionA>();
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
                break;
            case state.GATHERING:
                break;
            case state.ATTACKING:
                isMovable = true;
                isInteractable = true;
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
        Debug.Log(command.GetUnitCommand());
        if (isInteractable)
        {
            switch (command.GetUnitCommand())
            {
                case (UnitCommands.Move):
                   
                    if (isMovable)
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
                case (UnitCommands.Attack):
                    GetComponent<UnitAttack>().SetTarget(command.GetUnit());
                    break;
                case (UnitCommands.Build):
                    if (this.unitType == UnitType.Worker || this.unitType == UnitType.Building)
                    {
                        BuildCommand(command);
                        if(addToQueue)
                        {
                            queueCommands.Add(command);
                        }
                    }
                    break;
                case (UnitCommands.Gather):
                    if(unitType == UnitType.Worker)
                    {
                        GatherCommand(command);
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
        SetState(state.BUILDING);
        Debug.Log("BUILDING STATE SET");
        bool atDestination = true;
        this.command = command;
        CmdBuildSeconds((int)command.GetUnit().GetComponent<Unit>().GetItem().BuildTime);
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
    void CmdBuildSeconds(int seconds)
    {
        buildSeconds = seconds;
    }

    [Command]
    public void CmdBuildBuilding(int id, Vector3 point)
    {
        if (this.isServer)
        {
            GameObject prefab = CustomNetworkManager.singleton.spawnPrefabs[id];
            if(unitType == UnitType.Building)
            {
                Vector3 newPoint = CheckSpawnPoint(point);
                point = newPoint;
            }
            GameObject obj = Instantiate(prefab, new Vector3(point.x, point.y - 3, point.z), Quaternion.Euler(0, -210, 0)) as GameObject;
            obj.GetComponent<RtsObjectController>().teamId = GetComponent<RtsObjectController>().teamId;
            obj.GetComponent<RtsObjectController>().itemId = id;
            obj.GetComponent<RtsObjectController>().color = GetComponent<RtsObjectController>().color;
            obj.GetComponent<RtsObjectController>().ParentObject = gameObject;
            obj.GetComponent<RtsObject>().conn = this.conn;
            obj.GetComponent<RtsObjectController>().SetPlayer(GetComponent<RtsObjectController>().GetPlayer());
            NetworkServer.SpawnWithClientAuthority(obj, conn);
            RpcBuildBuilding(obj);

            //SetState(state.BUILDING);
            //obj.GetComponent<Unit>().DeployPlacement();
        }
    }

    [ClientRpc]
    public void RpcBuildBuilding(GameObject obj)
    {
        //obj.GetComponent<Unit>().DeployPlacement();
    }

    public IEnumerator AtBuildingPlacement(Vector3 buildingPlacement, bool atDestination)
    {
        while (Vector3.Distance(transform.position, buildingPlacement) > 1f)
        {
            yield return null;
        }
        CmdBuildBuilding(command.GetUnit().GetComponent<RtsObject>().GetItem().ID, command.GetPoint());
        GetComponent<Movement>().Hold(state.BUILDING);
        Debug.Log("HOLD");
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
        GetComponent<RtsObjectController>().GetPlayer().GetComponent<PlayerUnitController>().AddUnit(gameObject);
    }

    public IEnumerator BuildingLength(float seconds)
    {
        isMovable = false;
        isInteractable = false;
        gameObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(seconds);
        gameObject.GetComponent<Renderer>().enabled = true;
        Debug.Log("SECONDS" + seconds);
        SetState(state.IDLE);
        CheckQueue();
    }

    protected void CheckCommandQueue()
    {
        if(queueCommands.Count > 0)
        {
            queueCommands.RemoveAt(0);
            if (queueCommands.Count > 0)
            {
                Command(queueCommands[0], false);
            }

            foreach (var q in queueCommands)
            {
                Debug.Log(q.ToString());
            }
        }
    }

    void GatherCommand(Icommand command)
    {
        if(GetComponent<UnitGather>())
        {
            GetComponent<UnitGather>().Gather(command);
        }
    }

    private Vector3 CheckSpawnPoint(Vector3 point)
    {
        float radius = 3.0f;
        var hitColliders = Physics.OverlapSphere(point, 1);
        if (hitColliders.Length > 0)
        {
            foreach(var collider in hitColliders)
            {
                if(collider.gameObject.name == "Terrain")
                {
                    continue;
                }
                Debug.Log(collider);
                return CheckSpawnPoint(new Vector3(point.x + 0.1f, point.y, point.z));
            }
        }
        return point;
    }

    public void TakeDamage(int[] damageInfo)
    {
        health -= damageInfo[0];
        if(health <= 0)
        {
            health = 0;
            NetworkServer.Destroy(gameObject);
        }
    }

}
