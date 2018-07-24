using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseState
{
    DEFAULT,
    ATTACK,
    MOVE,
    BUILDING_PLACEMENT
}

public delegate void ChangedMouseState(MouseState newState);

public class MouseManager : MonoBehaviour {

    private MouseState state;
    public static MouseManager main;
    public event ChangedMouseState ChangedMouseState;

    [SerializeField]
    private GameObject buildingToBePlaced;

    public void SetBuildingTobePlaced(GameObject building)
    {
        buildingToBePlaced = building;
    }

    public MouseState GetState()
    {
        return state;
    }

    public void SetState(MouseState newState)
    {
        state = newState;
        ChangedMouseState(newState);
    }

	// Use this for initialization
	void Awake () {
        main = this;
        ChangedMouseState += OnChangedMouseState;
    }
	
	// Update is called once per frame
	void Update () {
        switch (GetState())
        {
            case MouseState.DEFAULT:
                //Debug.Log("Idle");
                break;
            case MouseState.ATTACK:
                //Debug.Log("Moving");
                break;
            case MouseState.MOVE:
                break;
            case MouseState.BUILDING_PLACEMENT:
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    buildingToBePlaced.transform.position = hit.point;
                    //Debug.Log(buildingToBePlaced.transform.position);
                }
                break;
        }
    }

    private void OnChangedMouseState(MouseState newState)
    {
        switch (newState)
        {
            case MouseState.DEFAULT:
                //Destroy(buildingToBePlaced);
                break;
            case MouseState.ATTACK:
                break;
            case MouseState.MOVE:
                break;
            case MouseState.BUILDING_PLACEMENT:
                //Instantiate(buildingToBePlaced);
                break;
        }
    }
}
