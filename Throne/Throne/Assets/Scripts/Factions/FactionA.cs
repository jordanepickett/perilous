using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionA : Faction {

    private void Awake()
    {
        SetBuildings();
    }

    public void SetBuildings()
    {
        buildings = FactionABuildings.GetAllBuildings();
    }

    public void SetUpgrades()
    {
        upgrades = UpgradeDB.GetAllUpgrades();
    }

    public void SyncUnitAvailability()
    {
        GetAvailableBuildings();
        GetUnavailableUnits();
    }

    public void SyncUpgradeAvailability(int id)
    {
        GetAvailableUpgrades(id);
    }

    void GetAvailableBuildings()
    {
        PlayerUnitController unitController = GetComponent<RtsObjectController>().GetPlayer().GetComponent<PlayerUnitController>();
        unitController.CmdGetAvailableBuildings();
        if (unitController.isLocalPlayer)
        {
            //Debug.Log(unitController.availableUnitIds.Count);
            var ids = unitController.availableUnitIds;
            foreach (int id in ids)
            {
                Item item = GetBuildableBuildings().Find(i => i.ID == id);
                item.isAvailable = true;
                //Debug.Log(item.Name + " AVAILABLE: " + item.isAvailable);
            }
        }
    }

    void GetUnavailableUnits()
    {
        PlayerUnitController unitController = GetComponent<RtsObjectController>().GetPlayer().GetComponent<PlayerUnitController>();
        unitController.CmdGetUnavailableBuildings();
        if (unitController.isLocalPlayer)
        {
            var ids = unitController.unavailableUnitIds;
            foreach (int id in ids)
            {
                Item item = GetBuildableBuildings().Find(i => i.ID == id);
                item.isAvailable = false;
                //Debug.Log(item.Name + " AVAILABLE: " + item.isAvailable);
            }
        }
    }

    void GetAvailableUpgrades(int id)
    {
        PlayerUnitController unitController = GetComponent<RtsObjectController>().GetPlayer().GetComponent<PlayerUnitController>();
        unitController.CmdGetAvailableUpgrades(id);
    }

}
