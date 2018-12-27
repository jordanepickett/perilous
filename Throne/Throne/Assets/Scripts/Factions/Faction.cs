using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Faction : NetworkBehaviour {

    [SerializeField]
    protected List<Item> buildings;

    [SerializeField]
    protected List<Upgrade> upgrades;

    private string name;

    public string GetName()
    {
        return name;
    }

    public void SetName(string newName)
    {
        name = newName;
    }

    public List<Item> GetBuildableBuildings()
    {
        return buildings;
    }

    public List<Upgrade> GetUpgradeableUpgrades()
    {
        return upgrades;
    }
}
