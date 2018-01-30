using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class Unit : MonoBehaviour {

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
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
