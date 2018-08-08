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
           
                Ray ray = GetComponent<EventsManager>().gameCamera.ScreenPointToRay(Input.mousePosition);
                //Plane plane = new Plane(Vector3.up, new Vector3(0,1.5f,0));

                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    //Vector3 point = ray.GetPoint(rayDistance);
                    buildingToBePlaced.transform.position = new Vector3(
                        Mathf.Round(hitInfo.point.x), hitInfo.point.y, Mathf.Round(hitInfo.point.z));
                }
                break;
        }
    }

    private void OnChangedMouseState(MouseState newState)
    {
        switch (newState)
        {
            case MouseState.DEFAULT:
                Destroy(buildingToBePlaced);
                break;
            case MouseState.ATTACK:
                break;
            case MouseState.MOVE:
                break;
            case MouseState.BUILDING_PLACEMENT:
                GameObject placement = (GameObject)Resources.Load("Placement") as GameObject;
                buildingToBePlaced = Instantiate(placement);
                break;
        }
    }
}
