using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry : Unit {


    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        switch (State)
        {
            case state.IDLE:
                //Debug.Log("Idle");
                break;
            case state.MOVING:
                //Debug.Log("Moving");
                break;
            case state.GATHERING:
                break;
            case state.ATTACKING:
                GetComponent<UnitAttack>().Attack();
                //Building();
                break;
            case state.BUILDING:
                Debug.Log("BUILDING");
                //Building();
                break;
            case state.DEPLOYING:
                Debug.Log("DEPLOYING");
                break;
        }
	}

    public void Building()
    {
        
    }

    protected override void OnChangedStates(state newState)
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
                break;
            case state.BUILDING:
                HideCommands();
                break;
            case state.DEPLOYING:
                HideCommands();
                isMovable = false;
                isInteractable = false;
                Deploying();
                break;
        }
    }

    public override void DisplayPanel()
    {
        Debug.Log("UNIT DISPLAY");
        UiManager.main.UnitCommands();
    }
}
