using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public delegate void PositionReached();

public class Movement : NetworkBehaviour {

    private Unit unit;
    private NavMeshAgent agent;
    public float speed;
    public float turnSpeed;
    public float acceleration;

    public event PositionReached PositionReached;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        unit = GetComponent<Unit>();
        agent.speed = speed;
        agent.angularSpeed = turnSpeed;
        agent.acceleration = acceleration;
	}
	
	// Update is called once per frame
	void Update () {
        if (!this.hasAuthority)
        {
            return;
        }
        if (unit.GetState() == state.MOVING)
        {
            float dist = agent.remainingDistance;
            if (agent.remainingDistance == 0)
            {
                unit.SetState(state.IDLE);
            }
        }
	}

    public void MoveUnit(Vector3 point)
    {
        CmdSetTarget(point);
    }

    public void UnitMove(Vector3 point)
    {
        if(GetComponent<RtsObject>().isMovable)
        {
            transform.LookAt(point);
            agent.SetDestination(point);
            unit.SetState(state.MOVING);
        }
    }

    [Command]
    public void CmdSetTarget(Vector3 point)
    {
        UnitMove(point);
        RpcMoveUnit(point);
    }

    [ClientRpc]
    public void RpcMoveUnit(Vector3 point)
    {
        UnitMove(point);
    }

    public void Hold()
    {
        CmdHold();
    }

    [Command]
    public void CmdHold()
    {
        HoldUnit();
        RpcHold();
    }

    [ClientRpc]
    public void RpcHold()
    {
        HoldUnit();
    }

    public void HoldUnit()
    {
        agent.SetDestination(this.transform.position);
        unit.SetState(state.IDLE);
    }
}
