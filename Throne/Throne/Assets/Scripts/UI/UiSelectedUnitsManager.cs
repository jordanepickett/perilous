using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSelectedUnitsManager : MonoBehaviour {

    public GameObject UnitHolder;

    public Button[] unitBtns;

    public Text unitInfo;

    // Use this for initialization
    void Start () {
        SelectionManager.main.FirstUnitChanged += ChangeUnitInfoPanel;
        UnitHolder.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ChangeUnitInfoPanel()
    {
        HideMultiUnitBtns();
        unitInfo.gameObject.SetActive(false);
        if (!UnitHolder.activeSelf)
        {
            UnitHolder.SetActive(true);
        }
        if (SelectionManager.main.GetSelectedUnits().Count > 1)
        {
            DisplayMultipleUnits();
        }
        else if(SelectionManager.main.GetSelectedUnits().Count == 1)
        {
            DisplaySingleUnit();
        }
        else
        {
            UnitHolder.SetActive(false);
        }
    }

    void DisplayMultipleUnits()
    {
        for(int i = 0; i < SelectionManager.main.GetSelectedUnits().Count; i++)
        {
            Button btn = unitBtns[i];
            Image btnImage = btn.GetComponent<Image>();
            btn.gameObject.SetActive(true);
            btn.interactable = true;
            btnImage.sprite = SelectionManager.main.GetSelectedUnits()[i].GetComponent<RtsObject>().GetUnitImage();
            btn.onClick.AddListener(delegate { SelectUnit(btn); });
        }
    }

    void DisplaySingleUnit()
    {
        if(SelectionManager.main.FirstUnit().GetComponent<Unit>().unitType == UnitType.Building &&
            SelectionManager.main.FirstUnit().GetComponent<Unit>().GetState() == state.BUILDING)
        {
            return;
        }
        unitInfo.gameObject.SetActive(true);
        Item item = SelectionManager.main.FirstUnit().GetComponent<RtsObject>().GetItem();
        unitInfo.text = "ATTACK DMG: " + item.attack.damage.ToString() + "\n" +
            "ARMOR: " + item.Armour;
    }

    void SelectUnit(Button btn)
    {
        int index = Array.IndexOf(unitBtns, btn);
        Debug.Log(index);
        SelectionManager.main.SelectUnitByIndex(index);
    }

    void HideMultiUnitBtns()
    {
        foreach(Button btn in unitBtns)
        {
            btn.interactable = false;
            btn.onClick.RemoveAllListeners();
            btn.gameObject.SetActive(false);
        }
    }
}
