using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnitController : NetworkBehaviour {

    private List<GameObject> allUnits = new List<GameObject>();
    private List<Item> availableItems = new List<Item>();
    private List<Item> unitItems = new List<Item>();
    private List<Item> unitDB = new List<Item>();

    private List<Upgrade> upgradeDB = new List<Upgrade>();
    public List<Cost> Costs = new List<Cost>();
    public List<UpgradeNumbers> UpgradeNumbers = new List<UpgradeNumbers>();
    PlayerController playerController;

    [SyncVar]
    public int allowedUnits = 5;

    [SyncVar]
    public int unitsExisting = 3;

    public SyncListInt availableUnitIds;

    public SyncListInt unavailableUnitIds;

    public static int MAX_ALLOWED_UNITS = 50;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if(isServer)
        {
            //FactionABuildings.Initialise();
            unitDB = FactionABuildings.GetAllBuildings();
            upgradeDB = UpgradeDB.GetAllUpgrades();
            UpdateAllAvailableUnits();
        }
    }

    public List<GameObject> GetAllUnits()
    {
        return allUnits;
    }

    public void AddUnit(GameObject obj)
    {
        if(isLocalPlayer || isServer)
        {
            allUnits.Add(obj);
            unitItems.Add(obj.GetComponent<RtsObject>().GetItem());
            UpdateAllAvailableUnits();
            if (obj.GetComponent<Unit>().unitType != UnitType.Building)
            {
                unitsExisting += obj.GetComponent<Unit>().GetItem().food;
            }
            if (obj.GetComponent<Unit>().unitType == UnitType.Building)
            {
                if (obj.GetComponent<Building>().buildingType == BuildingType.FARM)
                {
                    allowedUnits += 5;
                }
            }
        }
    }

    public void RemoveUnit(GameObject obj)
    {
        if(isLocalPlayer || isServer)
        {
            allUnits.Remove(obj);
            RemoveFromBoundGroup(obj.GetComponent<SelectableUnit>());
            unitItems.Remove(obj.GetComponent<RtsObject>().GetItem());
            UpdateAllAvailableUnits();
            if (obj.GetComponent<Unit>().unitType != UnitType.Building)
            {
                unitsExisting -= obj.GetComponent<Unit>().GetItem().food;
            }
            if (obj.GetComponent<Unit>().unitType == UnitType.Building)
            {
                if (obj.GetComponent<Building>().buildingType == BuildingType.FARM)
                {
                    allowedUnits -= 5;
                }
            }
        }
    }

    private void RemoveFromBoundGroup(SelectableUnit unit)
    {
        SelectionManager.main.RemoveUnitFromBoundGroup(unit);
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

    void UpdateAllAvailableUnits()
    {
        Debug.Log(unitDB.Count);
        foreach(var item in unitDB)
        {
            if(item.RequiredBuildings.Length == 0)
            {
                item.isAvailable = true;
            }
            else
            {
                bool requiredUnitsExist = false;
                foreach(int rId in item.RequiredBuildings)
                {
                    Item rItem = unitItems.Find(i => i.ID == rId);
                    if(rItem != null)
                    {
                        requiredUnitsExist = true;
                    }
                    else
                    {
                        requiredUnitsExist = false;
                        break;
                    }
                }
                if (requiredUnitsExist)
                {
                    item.isAvailable = true;
                    availableItems.Add(item);
                }
            }
            Debug.Log(item.Name + " AVAILABLE: " + item.isAvailable);
        }
    }

    [Command]
    public void CmdGetAvailableBuildings()
    {
        UpdateAllAvailableUnits();
        availableUnitIds.Clear();
        foreach(var id in unitDB)
        {
            if (id.isAvailable)
            {
                availableUnitIds.Add(id.ID);
            }
        }
    }

    [Command]
    public void CmdGetUnavailableBuildings()
    {
        unavailableUnitIds.Clear();
        foreach (var id in unitDB)
        {
            if(!id.isAvailable)
            {
                unavailableUnitIds.Add(id.ID);
            }
        }
    }

    [ClientRpc]
    void RpcAvailableUnitIds()
    {

    }

    [ClientRpc]
    void RpcUnavailableUnitIds()
    {

    }

    // Update is called once per frame
    void Update () {
		if(isLocalPlayer)
        {

        }
	}

    [Command]
    public void CmdGetAvailableUpgrades(int id)
    {
        List<Upgrade> buildingUpgrades = upgradeDB.FindAll(u => u.BuildingInfo.BuildFrom == id);
        foreach(Upgrade upgrade in buildingUpgrades)
        {
            RpcClientUpgrades(upgrade.Costruct, upgrade.Numbers);
        }
    }

    [ClientRpc]
    void RpcClientUpgrades(Cost cost, UpgradeNumbers numbers)
    {
        Costs.Clear();
        UpgradeNumbers.Clear();
        Costs.Add(cost);
        UpgradeNumbers.Add(numbers);
    }

}
