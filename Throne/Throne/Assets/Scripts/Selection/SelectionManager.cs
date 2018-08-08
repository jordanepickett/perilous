using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public delegate void FirstUnitChanged();

public class SelectionManager : MonoBehaviour {

    public GameObject TeamSelectionPrefab;
    
    public static SelectionManager main;
    public static int MAX_SELECTION = 12;

    private List<SelectableUnit> selectedUnits = new List<SelectableUnit>();

    public event FirstUnitChanged FirstUnitChanged;


    public List<SelectableUnit> GetSelectedUnits()
    {
        return selectedUnits;
    }

    public SelectableUnit FirstUnit()
    {
        return selectedUnits[0];
    }

    private void Awake()
    {
        main = this;
    }

    // Use this for initialization
    void Start () {
        EventsManager.main.MouseSelection += SelectUnits;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SelectUnits(Bounds viewPortBounds)
    {
        //ClearSelectedUnits();
        List<SelectableUnit> potentialUnits = new List<SelectableUnit>();
        List<int> indexes = new List<int>();

        int i = 0;
        foreach (var selectableObject in FindObjectsOfType<SelectableUnit>())
        {
            if(i == MAX_SELECTION)
            {
                break;
            }

            if(selectableObject.GetComponent<RtsObjectController>() && selectedUnits.Count > 0)
            {
                if (FirstUnit().GetComponent<RtsObjectController>().teamId != selectableObject.GetComponent<RtsObjectController>().teamId)
                {
                    continue;
                }
            }
            if (IsWithinSelectionBounds(selectableObject.gameObject, viewPortBounds))
            {
                foreach(var pot in potentialUnits)
                {
                    if(pot.GetComponent<Unit>().unitType == UnitType.Infantry || pot.GetComponent<Unit>().unitType == UnitType.Worker)
                    {
                        if(selectableObject.GetComponent<Unit>().unitType == UnitType.Building)
                        {

                        }
                    }
                }
                potentialUnits.Add(selectableObject);
                i++;
            }
        }
        if (potentialUnits.Count > 0)
        {
            ClearSelectedUnits();
            selectedUnits = potentialUnits;
            foreach (var selectableObject in selectedUnits)
            {
                selectableObject.ChangeSelectionStatus(true);
            }
            FirstUnitChanged();
        }
    }

    private bool IsWithinSelectionBounds(GameObject gameObject, Bounds viewPortBounds)
    {
        var camera = Camera.main;
        if(viewPortBounds.min.x == viewPortBounds.max.x && viewPortBounds.min.y == viewPortBounds.max.y)
        {
            if(Utils.SingleMouseClick() && Utils.SingleMouseClick().GetComponent<NetworkIdentity>() == gameObject.GetComponent<NetworkIdentity>())
            {
                return true;
            }
            return false;
        }
        return viewPortBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }

    private void ClearSelectedUnits()
    {
        foreach (var selectedUnit in selectedUnits)
        {
            selectedUnit.GetComponent<SelectableUnit>().ChangeSelectionStatus(false);
        }
        selectedUnits.Clear();
    }

    public void GiveAllUnitsCommand(Icommand command)
    {
        foreach(var unit in selectedUnits)
        {
            unit.gameObject.GetComponent<Unit>().Command(command, false);
        }
    }
}
