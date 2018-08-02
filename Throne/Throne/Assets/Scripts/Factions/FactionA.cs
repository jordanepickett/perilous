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
}
