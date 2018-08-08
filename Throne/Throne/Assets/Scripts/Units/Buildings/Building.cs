using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Unit {

    public BuildingType buildingType;

    public override void DeployPlacement()
    {
        SetState(state.DEPLOYING);
        startPosition = transform.position;
        target = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
        timeToReachTarget = GetItem().BuildTime;
        isMovable = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case state.IDLE:
                isInteractable = true;
                break;
            case state.MOVING:
                Debug.Log("Moving");
                break;
            case state.GATHERING:
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
            case state.GATHERING:
                break;
            case state.BUILDING:
                isMovable = false;
                isInteractable = true;
                break;
            case state.DEPLOYING:
                isMovable = false;
                isInteractable = false;
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
            GetComponent<RtsObjectController>().GetPlayer().GetComponent<PlayerUnitController>().AddUnit(gameObject);
        }
    }

    public override void DisplayPanel()
    {
        UiManager.main.BuildingCommands();
    }
}
