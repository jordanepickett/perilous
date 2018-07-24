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
            case state.WORKING:
                break;
            case state.BUILDING:
                Debug.Log("BUILDING");
                //Building();
                break;
            case state.DEPLOYING:
                Debug.Log("DEPLOYING");
                Deploying();
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

    public override void DisplayPanel()
    {
        UiManager.main.UnitCommands();
    }
}
