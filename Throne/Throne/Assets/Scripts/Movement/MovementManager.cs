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

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GiveMovementCommand(MoveCommand command)
    {
        var units = SelectionManager.main.GetSelectedUnits();
        var newPoint = command.GetPoint();
        int unitsInLine = (int)Mathf.Sqrt(SelectionManager.main.GetSelectedUnits().Count);
        int i = 0;
        int offset = 1;
        foreach (var unit in units)
        {
            if (SelectionManager.main.GetSelectedUnits().Count == 1)
            {
                unit.gameObject.GetComponent<Unit>().Command(command);
                break;
            }
            command.SetXPoint(newPoint.x += -.8f);
            unit.gameObject.GetComponent<Unit>().Command(command);
            i++;
            offset--;
            if(i%unitsInLine == 0 && SelectionManager.main.GetSelectedUnits().Count > 3)
            {
                newPoint.x = command.GetPoint().x;
                newPoint.z -= .8f;
                offset = 1;
            }
        }
    }
}
