using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    public GameObject prefab;
    public GameObject buildPrefab;
    private List<GameObject> UnitPanel = new List<GameObject>();
    private SelectableUnit firstUnit;

    public bool isBuildingPanelOpen = false;

    private List<Icommand> unitCommands = new List<Icommand>();
    public List<Icommand> GetUnitCommands()
    {
        return unitCommands;
    }

    private Icommand openBuildCommand;

    public static UiManager main;

    private void Awake()
    {
        main = this;
    }

    // Use this for initialization
    void Start () {
        //UnitCommands();
        SelectionManager.main.FirstUnitChanged += CheckFirstUnit;
        InitializeCommands();
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void UnitCommands()
    {
        Debug.Log(SelectionManager.main.FirstUnit().GetComponent<Unit>().IsInteractable());
        if(SelectionManager.main.FirstUnit().GetComponent<Unit>().IsInteractable())
        {
            GameObject newObj;
            foreach (var command in unitCommands)
            {
                newObj = (GameObject)Instantiate(prefab, transform);
                newObj.GetComponent<CommandUiSender>().SetCommand(command);
                UnitPanel.Add(BtnDecorator.Decorate(newObj));
            }

            if (firstUnit.GetComponent<RtsObject>().unitType == UnitType.Worker)
            {
                newObj = (GameObject)Instantiate(prefab, transform);
                newObj.GetComponent<CommandUiSender>().SetCommand(openBuildCommand);
                UnitPanel.Add(BtnDecorator.BuildDecorate(newObj));
            }
        }
    }

    void InitializeCommands()
    {
        foreach (UnitCommands command in Enum.GetValues(typeof(UnitCommands)))
        {
            if (command == global::UnitCommands.Move)
            {
                MoveCommand unitCommand = new MoveCommand();
                unitCommands.Add(unitCommand);

            }
            if (command == global::UnitCommands.Hold)
            {
                StopCommand unitCommand = new StopCommand();
                unitCommands.Add(unitCommand);
            }
            if (command == global::UnitCommands.Attack)
            {
                AttackCommand unitCommand = new AttackCommand();
                unitCommands.Add(unitCommand);
            }
            if (command == global::UnitCommands.Gather)
            {
                GatherCommand unitCommand = new GatherCommand();
                unitCommands.Add(unitCommand);
            }
            // if (command == global::UnitCommands.Build || command == global::UnitCommands.Repair && firstUnit.GetComponent<RtsObject>().unitType == UnitType.Worker)
            // {
            //newObj = (GameObject)Instantiate(prefab, transform);
            //newObj.GetComponent<CommandUiSender>().SetCommand(new MoveCommand());
            //UnitPanel.Add(BtnDecorator.Decorate(newObj));
            //}
        }

        openBuildCommand = new OpenBuildCommand();
    } 

    public void BuildingCommands()
    {
        isBuildingPanelOpen = true;
        GameObject newObj;

        foreach (var item in firstUnit.GetComponent<FactionA>().GetBuildableBuildings())
        {
            if(item.BuildingId == firstUnit.GetComponent<RtsObject>().GetItem().ID)
            {
                newObj = (GameObject)Instantiate(prefab, transform);
                BuildCommand buildCommand = new BuildCommand(item.keyBind);
                buildCommand.SetUnit(item.Prefab);
                newObj.GetComponent<CommandUiSender>().SetCommand(buildCommand);
                newObj.GetComponent<Image>().sprite = item.ItemImage;
                item.Prefab.GetComponent<RtsObject>().SetItem(item);
                UnitPanel.Add(BtnDecorator.BuildDecorate(newObj));
            }
        }
    }

    public void CheckFirstUnit()
    {
        if(firstUnit == null || firstUnit.GetComponent<RtsObject>().gameObject != SelectionManager.main.FirstUnit().GetComponent<RtsObject>().gameObject || isBuildingPanelOpen == true)
        {
            firstUnit = SelectionManager.main.FirstUnit();
            ClearUnitPanel();
            firstUnit.GetComponent<RtsObject>().DisplayPanel();
            isBuildingPanelOpen = false;
        }
       
    }

    public void ClearUnitPanel()
    {
        foreach(GameObject command in UnitPanel)
        {
            Destroy(command);
        }
    }

    public void CreateBuildingsPanel()
    {
        isBuildingPanelOpen = true;
        ClearUnitPanel();
        GameObject newObj;

        foreach(var item in firstUnit.GetComponent<FactionA>().GetBuildableBuildings())
        {
            if(item.TypeIdentifier == UnitType.Building)
            {
                newObj = (GameObject)Instantiate(prefab, transform);
                BuildCommand buildCommand = new BuildCommand(item.keyBind);
                buildCommand.SetUnit(item.Prefab);
                newObj.GetComponent<CommandUiSender>().SetCommand(buildCommand);
                newObj.GetComponent<Image>().sprite = item.ItemImage;
                item.Prefab.GetComponent<RtsObject>().SetItem(item);
                UnitPanel.Add(BtnDecorator.BuildDecorate(newObj));
            }
        }

        //newObj = (GameObject)Instantiate(prefab, transform);


    }
}
