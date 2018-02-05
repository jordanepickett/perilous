using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    public GameObject prefab;
    private List<GameObject> UnitPanel = new List<GameObject>();
    private SelectableUnit firstUnit;

	// Use this for initialization
	void Start () {
        //UnitCommands();
        SelectionManager.main.FirstUnitChanged += CheckFirstUnit;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void UnitCommands()
    {
        GameObject newObj;
        foreach (UnitCommands command in Enum.GetValues(typeof(UnitCommands)))
        {
            if(command == global::UnitCommands.Move)
            {
                newObj = (GameObject)Instantiate(prefab, transform);
                UnitPanel.Add(newObj);
            }
        }
    }

    private void CheckFirstUnit()
    {
        if(firstUnit == null || firstUnit.GetComponent<RtsObject>().unitType != SelectionManager.main.FirstUnit().GetComponent<RtsObject>().unitType)
        {
            firstUnit = SelectionManager.main.FirstUnit();
            ClearUnitPanel();
            UnitCommands();
        }
    }

    private void ClearUnitPanel()
    {
        foreach(GameObject command in UnitPanel)
        {
            Destroy(command);
        }
    }
}
