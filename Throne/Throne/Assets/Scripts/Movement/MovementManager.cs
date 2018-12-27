using System;
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
        int unitsInLine = (int)Math.Ceiling(Mathf.Sqrt(SelectionManager.main.GetSelectedUnits().Count));
        if(unitsInLine > 4)
        {
            unitsInLine = 4;
        }
        int counter = 0;
        int z = 0;
        int x = 0;
        foreach (var unit in units)
        {
            if(unit.GetComponent<Unit>().unitType != UnitType.Building)
            {
                command.SetPoint(new Vector3((newPoint.x + (x * 2)), newPoint.y, (newPoint.z + (z * 2))));
                x++;
                counter++;

                unit.gameObject.GetComponent<Unit>().Command(command);

                if (counter == unitsInLine)
                {
                    x = 0;
                    counter = 0;
                    z++;
                }
            }
        }
    }

    public void GiveMovementAttackCommandOne(MoveCommand command)
    {
        var units = SelectionManager.main.GetSelectedUnits();
        var newPoint = command.GetPoint();
        //int unitsInLine = (int)Math.Ceiling(Mathf.Sqrt(SelectionManager.main.GetSelectedUnits().Count));
        int x = 0;
        foreach (var unit in units)
        {
            if (unit.GetComponent<Unit>().unitType != UnitType.Building)
            {
                command.SetPoint(new Vector3((newPoint.x + (x * 2)), newPoint.y, newPoint.z));
                x++;

                unit.gameObject.GetComponent<Unit>().Command(command);
            }
        }
    }

    public void GiveMovementAttackCommand(MoveCommand command)
    {
        var units = SelectionManager.main.GetSelectedUnits();
        var newPoint = command.GetPoint();
        foreach (var unit in units)
        {
            if (unit.GetComponent<Unit>().unitType != UnitType.Building)
            {
                //Vector3 pos = RandomCircle(command.GetPoint(), 5.0f);
                //command.SetPoint(pos);
                unit.gameObject.GetComponent<Unit>().Command(command);
            }
        }
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = UnityEngine.Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }
}
