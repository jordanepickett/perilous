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

    public GameObject unitPanel;
    //public Image unitTimer;
    public GameObject infoPanel;
    public UiPortraitManager unitPortrait;

    private List<Icommand> unitCommands = new List<Icommand>();
    public List<Icommand> GetUnitCommands()
    {
        return unitCommands;
    }

    private Icommand openBuildCommand;

    public static UiManager main;

    public SelectableUnit GetFirstUnit()
    {
        return firstUnit;
    }

    private void Awake()
    {
        main = this;
    }

    // Use this for initialization
    void Start () {
        //UnitCommands();
        SelectionManager.main.FirstUnitChanged += CheckFirstUnit;
        unitPortrait = GetComponent<UiPortraitManager>();
        InitializeCommands();
    }
	
	// Update is called once per frame
	void Update () {
        //DisplayUnitPanel();
    }

    public void UnitCommands()
    {
        if(SelectionManager.main.FirstUnit().GetComponent<Unit>().IsInteractable())
        {
            GameObject newObj;
            foreach (var command in unitCommands)
            {
                if(command.GetUnitCommand() == global::UnitCommands.Gather && firstUnit.GetComponent<Unit>().unitType != UnitType.Worker)
                {
                    continue;
                }
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
        if (SelectionManager.main.FirstUnit().GetComponent<Unit>().IsInteractable())
        {
            isBuildingPanelOpen = true;
            GameObject newObj;
            firstUnit.GetComponent<FactionA>().SyncUnitAvailability();
            foreach (var item in firstUnit.GetComponent<FactionA>().GetBuildableBuildings())
            {
                if (item.BuildingId == firstUnit.GetComponent<RtsObject>().GetItem().ID)
                {
                    newObj = (GameObject)Instantiate(prefab, transform);
                    if (!item.isAvailable)
                    {
                        newObj.GetComponent<Button>().interactable = false;
                    }
                    BuildCommand buildCommand = new BuildCommand(item.keyBind);
                    buildCommand.SetUnit(item.Prefab);
                    newObj.GetComponent<CommandUiSender>().SetCommand(buildCommand);
                    newObj.GetComponent<CommandUiSender>().infoPanel = infoPanel;
                    newObj.GetComponent<Image>().sprite = item.ItemImage;
                    item.Prefab.GetComponent<RtsObject>().SetItem(item);
                    UnitPanel.Add(BtnDecorator.BuildDecorate(newObj));
                }
            }
        }
    }

    public void CheckFirstUnit()
    {
        if(firstUnit == null || firstUnit.gameObject != SelectionManager.main.FirstUnit().gameObject || isBuildingPanelOpen == true)
        {
            firstUnit = SelectionManager.main.FirstUnit();
            RtsObject obj = firstUnit.GetComponent<RtsObject>();
            ClearUnitPanel();
            obj.DisplayPanel();
            unitPortrait.UpdatePortrait();
            isBuildingPanelOpen = false;
        }
    }

    public void ShowUnitPanel()
    {
        RtsObject obj = firstUnit.GetComponent<RtsObject>();
        ClearUnitPanel();
        obj.DisplayPanel();
        unitPortrait.UpdatePortrait();
        isBuildingPanelOpen = false;
    }

    public void ClearUnitPanel()
    {
        foreach(GameObject command in UnitPanel)
        {
            Destroy(command);
        }
        UnitPanel.Clear();
        UiBuildingPanelManager.main.unitTimer.transform.parent.gameObject.SetActive(false);
    }

    //WORKERS BUILDING PANEL
    public void CreateBuildingsPanel()
    {
        isBuildingPanelOpen = true;
        ClearUnitPanel();
        GameObject newObj;
        if(SelectionManager.main.FirstUnit().GetComponent<Unit>().IsInteractable())
        {
            firstUnit.GetComponent<FactionA>().SyncUnitAvailability();
            foreach (var item in firstUnit.GetComponent<FactionA>().GetBuildableBuildings())
            {
                if (item.TypeIdentifier == UnitType.Building)
                {
                    Debug.Log(item.Name + " AVAILABLE: " + item.isAvailable);
                    newObj = (GameObject)Instantiate(prefab, transform);
                    if (!item.isAvailable)
                    {
                        newObj.GetComponent<Button>().interactable = false;
                    }
                    BuildCommand buildCommand = new BuildCommand(item.keyBind);
                    buildCommand.SetUnit(item.Prefab);
                    newObj.GetComponent<CommandUiSender>().SetCommand(buildCommand);
                    newObj.GetComponent<CommandUiSender>().infoPanel = infoPanel;
                    newObj.GetComponent<Image>().sprite = item.ItemImage;
                    item.Prefab.GetComponent<RtsObject>().SetItem(item);
                    UnitPanel.Add(BtnDecorator.KeyBindDecorate(newObj));
                }
            }
        }

        //newObj = (GameObject)Instantiate(prefab, transform);
    }
}
