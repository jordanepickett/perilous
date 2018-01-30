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
            selectionCircle = Instantiate(SelectionManager.main.TeamSelectionPrefab);
            selectionCircle.transform.SetParent(transform, false);
            selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);
        }
        else
        {
            if (selectionCircle != null)
            {
                Destroy(selectionCircle.gameObject);
                selectionCircle = null;
            }
        }
    }
}
