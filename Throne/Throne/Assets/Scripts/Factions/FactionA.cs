using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionA : Faction {

    private void Awake()
    {
        FactionABuildings.Initialise();
        buildings = FactionABuildings.GetAllBuildings();
    }
}
