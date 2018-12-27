using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SelectionStatus();

public class SelectableUnit : MonoBehaviour {

    public GameObject selectionCircle;
    private bool isSelected;

    public event SelectionStatus SelectionStatus;

    public bool GetIsSelected()
    {
        return isSelected;
    }

    public void SetIsSelected(bool selectionStatus)
    {
        isSelected = selectionStatus;
    }

    private void Awake()
    {
        SelectionStatus += HighlightUnit;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ChangeSelectionStatus(bool status)
    {
        if(status == true)
        {
            isSelected = true;
            SelectionStatus();
        }
        else
        {
            isSelected = false;
            SelectionStatus();
        }
    }

    public void HighlightUnit()
    {
        if (isSelected && selectionCircle == null)
        {
            RenderCircle();
        }
        else
        {
            RemoveCircle();
        }
    }

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.gray);
        Debug.Log("ENETER");
        var renderes = GetComponentsInChildren<Renderer>();
        foreach (var rendered in renderes)
        {
            rendered.material.SetColor("_EmissionColor", Color.gray);
        }
    }

    // ...and the mesh finally turns white when the mouse moves away.
    void OnMouseExit()
    {
        GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);

        var renderes = GetComponentsInChildren<Renderer>();
        foreach (var rendered in renderes)
        {
            rendered.material.SetColor("_EmissionColor", Color.black);
        }
    }

    void RenderCircle()
    {
        selectionCircle = Instantiate(SelectionManager.main.TeamSelectionPrefab);
        selectionCircle.transform.SetParent(transform, false);
        selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);
        selectionCircle.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        if (GetComponent<Unit>().unitType == UnitType.Building)
        {
            selectionCircle.GetComponent<Projector>().orthographicSize = 2;
        }
        else
        {
            selectionCircle.GetComponent<Projector>().orthographicSize = 1;
        }
    }

    void RemoveCircle()
    {
        if (selectionCircle != null)
        {
            Destroy(selectionCircle.gameObject);
            selectionCircle = null;
        }
    }
}
