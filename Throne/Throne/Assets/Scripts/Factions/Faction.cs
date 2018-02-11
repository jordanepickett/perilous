using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour {

    [SerializeField]
    protected List<Item> buildings;

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
}
