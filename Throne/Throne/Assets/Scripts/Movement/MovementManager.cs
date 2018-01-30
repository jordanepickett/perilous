using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementManager : MonoBehaviour {

    public static MovementManager main;

	// Use this for initialization
	void Awake () {
        main = this;
	}

    void Start()
    {
        EventsManager.main.RightMouseClick += GiveMovementCommand;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GiveMovementCommand(Vector3 point)
    {
        var units = SelectionManager.main.GetSelectedUnits();
        var newPoint = point;
        int unitsInLine = (int)Mathf.Sqrt(SelectionManager.main.GetSelectedUnits().Count);
        int i = 0;
        int offset = 1;
        foreach (var unit in units)
        {
            if(SelectionManager.main.GetSelectedUnits().Count == 1)
            {
                unit.gameObject.GetComponent<Movement>().MoveUnit(newPoint);
                break;
            }
            newPoint.x += -1;
            unit.gameObject.GetComponent<Movement>().MoveUnit(newPoint);
            i++;
            Debug.Log(offset);
            offset--;
            if(i%unitsInLine == 0 && SelectionManager.main.GetSelectedUnits().Count > 3)
            {
                newPoint.x = point.x;
                newPoint.z -= 2;
                offset = 1;
            }
        }
    }
}
