using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public delegate void FirstUnitChanged();

public class SelectionManager : MonoBehaviour {

    public GameObject TeamSelectionPrefab;

    public static SelectionManager main;
    public static int MAX_SELECTION = 12;

    [SerializeField]
    private List<SelectableUnit> selectedUnits = new List<SelectableUnit>();

    public event FirstUnitChanged FirstUnitChanged;

    public int teamId;

    private List<List<SelectableUnit>> boundUnits = new List<List<SelectableUnit>>();


    public List<SelectableUnit> GetSelectedUnits()
    {
        return selectedUnits;
    }

    public SelectableUnit FirstUnit()
    {
        return selectedUnits[0];
    }

    public bool ContainsUnit(SelectableUnit unit)
    {
        if (selectedUnits.Contains(unit))
        {
            return true;
        }
        return false;
    }

    public void RemoveUnit(SelectableUnit unit)
    {
        selectedUnits.Remove(unit);
    }

    private void Awake()
    {
        main = this;
        for (int i = 0; i < 10; i++)
        {
            boundUnits.Add(new List<SelectableUnit>());
        }
    }

    // Use this for initialization
    void Start() {
        EventsManager.main.MouseSelection += SelectUnits;
        EventsManager.main.LeftMouseClick += SingleSelection;

    }

    // Update is called once per frame
    void Update() {
    }

    public void SelectUnits(Bounds viewPortBounds)
    {
        //ClearSelectedUnits();
        List<SelectableUnit> potentialUnits = new List<SelectableUnit>();
        List<int> indexes = new List<int>();

        int i = 0;
        foreach (var selectableObject in FindObjectsOfType<SelectableUnit>())
        {
            if (i == MAX_SELECTION)
            {
                break;
            }

            if (selectableObject.GetComponent<RtsObjectController>() && selectedUnits.Count > 0)
            {
                if (teamId != selectableObject.GetComponent<RtsObjectController>().teamId)
                {
                    continue;
                }
            }
            if (IsWithinSelectionBounds(selectableObject.gameObject, viewPortBounds))
            {
                foreach (var pot in potentialUnits)
                {
                    if (pot.GetComponent<Unit>().unitType == UnitType.Infantry || pot.GetComponent<Unit>().unitType == UnitType.Worker)
                    {
                        if (selectableObject.GetComponent<Unit>().unitType == UnitType.Building)
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
            if(potentialUnits.Find(u => u.GetComponent<Unit>().unitType != UnitType.Building))
            {
                potentialUnits.RemoveAll(u => u.GetComponent<Unit>().unitType == UnitType.Building);
            }
            UnitSelection(potentialUnits);
        }
    }

    void UnitSelection(List<SelectableUnit> potentialUnits)
    {
        ClearSelectedUnits();
        foreach (var selectableObject in potentialUnits)
        {
            selectedUnits.Add(selectableObject);
            DisplaySelectionOnUnit(selectableObject);
        }
        FirstUnitChanged();
    }

    private bool IsWithinSelectionBounds(GameObject gameObject, Bounds viewPortBounds)
    {
        var camera = Camera.main;
        if (viewPortBounds.min.x == viewPortBounds.max.x && viewPortBounds.min.y == viewPortBounds.max.y)
        {
            if (Utils.SingleMouseClick() && Utils.SingleMouseClick().GetComponent<NetworkIdentity>() == gameObject.GetComponent<NetworkIdentity>())
            {
                return true;
            }
            return false;
        }
        return viewPortBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }

    private void ClearSelectedUnits()
    {
        if (selectedUnits.Count > 0)
        {
            foreach (var selectedUnit in selectedUnits)
            {
                selectedUnit.GetComponent<SelectableUnit>().ChangeSelectionStatus(false);
            }
            selectedUnits.Clear();
        }
    }

    public void GiveAllUnitsCommand(Icommand command)
    {
        foreach (var unit in selectedUnits)
        {
            unit.gameObject.GetComponent<Unit>().Command(command, false);
        }
    }

    public void AddToBoundedUnits(int index)
    {
        boundUnits[index].Clear();
        foreach (SelectableUnit obj in selectedUnits)
        {
            boundUnits[index].Add(obj);

        }
    }

    public void SelectBoundUnits(int index)
    {
        UnitSelection(boundUnits[index]);
    }

    public void SelectUnitByIndex(int index)
    {
        FinalizeSingleSelection(selectedUnits[index]);
    }

    public void RemoveUnitFromBoundGroup(SelectableUnit unit)
    {
        Debug.Log("HERE");
        foreach(var list in boundUnits)
        {
            if(list.Contains(unit))
            {
                list.Remove(unit);
            }
        }
    }

    void SingleSelection(RaycastHit hit)
    {
        if(hit.collider.gameObject.GetComponent<SelectableUnit>() && hit.collider.gameObject.GetComponent<RtsObjectController>().teamId == teamId)
        {
            SelectableUnit selectableObject = hit.collider.gameObject.GetComponent<SelectableUnit>();
            FinalizeSingleSelection(selectableObject);
        }
    }

    void DisplaySelectionOnUnit(SelectableUnit selectableObject)
    {
        selectableObject.ChangeSelectionStatus(true);
    }

    void FinalizeSingleSelection(SelectableUnit unit)
    {
        ClearSelectedUnits();
        selectedUnits.Add(unit);
        FirstUnitChanged();
        DisplaySelectionOnUnit(unit);
    }
}
