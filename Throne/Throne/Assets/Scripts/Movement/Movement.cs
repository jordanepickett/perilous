using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour {

    private Unit unit;
    private NavMeshAgent agent;
    public float speed;
    public float turnSpeed;
    public float acceleration;

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
		if(unit.GetState() == state.MOVING)
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
        agent.SetDestination(point);
        unit.SetState(state.MOVING);
        Debug.Log(unit.GetState());
    }
}
