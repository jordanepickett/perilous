using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {

    public GameObject TeamSelectionPrefab;

    public static SelectionManager main;
    public static int MAX_SELECTION = 12;

    private List<SelectableUnit> selectedUnits = new List<SelectableUnit>();

    public List<SelectableUnit> GetSelectedUnits()
    {
        return selectedUnits;
    }

	// Use this for initialization
	void Start () {
        main = this;
        EventsManager.main.MouseSelection += SelectUnits;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SelectUnits(Bounds viewPortBounds)
    {
        //ClearSelectedUnits();
        List<SelectableUnit> potentialUnits = new List<SelectableUnit>();

        int i = 0;
        foreach (var selectableObject in FindObjectsOfType<SelectableUnit>())
        {
            if(i == MAX_SELECTION)
            {
                break;
            }
            if (IsWithinSelectionBounds(selectableObject.gameObject, viewPortBounds))
            {
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
        }
    }

    private bool IsWithinSelectionBounds(GameObject gameObject, Bounds viewPortBounds)
    {
        var camera = Camera.main;
        if(viewPortBounds.min.x == viewPortBounds.max.x && viewPortBounds.min.y == viewPortBounds.max.y)
        {
            if(Utils.SingleMouseClick() && Utils.SingleMouseClick().name == gameObject.name)
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
}
