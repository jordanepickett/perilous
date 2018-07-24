using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Unit {



    public override void DeployPlacement()
    {
        SetState(state.DEPLOYING);
        startPosition = transform.position;
        target = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
        timeToReachTarget = 5f;
        isMovable = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case state.IDLE:
                //Debug.Log("Idle");
                isInteractable = true;
                break;
            case state.MOVING:
                Debug.Log("Moving");
                break;
            case state.WORKING:
                break;
            case state.BUILDING:
                break;
            case state.DEPLOYING:
                Debug.Log("DEPLOYING");
                Deploying();
                break;
        }
    }

    protected override void OnChangedStates(state newState)
    {
        switch (State)
        {
            case state.IDLE:
                isInteractable = true;
                break;
            case state.MOVING:
                isInteractable = true;
                break;
            case state.WORKING:
                break;
            case state.BUILDING:
                isMovable = false;
                isInteractable = true;
                StartCoroutine(BuildingLength(5f));
                break;
        }
    }

    public override void Deploying()
    {
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, target, t);
        if(transform.position == target)
        {
            SetState(state.IDLE);
        }
    }

    public override void DisplayPanel()
    {
        UiManager.main.BuildingCommands();
    }
}
