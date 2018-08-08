using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnitController : NetworkBehaviour {

    private List<GameObject> allUnits = new List<GameObject>();

    public List<GameObject> GetAllUnits()
    {
        return allUnits;
    }

    public void AddUnit(GameObject obj)
    {
        allUnits.Add(obj);
    }

    public void RemoveUnit(GameObject obj)
    {
        allUnits.Remove(obj);
    }

    public List<GameObject> FindTownHalls()
    {
        List<GameObject> townhalls = new List<GameObject>();
        foreach(var unit in allUnits)
        {
            if(unit.GetComponent<Building>())
            {
                if(unit.GetComponent<Building>().buildingType == BuildingType.MAIN)
                {
                    townhalls.Add(unit);
                }
            }
        }
        return townhalls;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
