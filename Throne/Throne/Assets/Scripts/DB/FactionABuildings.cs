using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FactionABuildings {

    private static List<Item> AllBuildings = new List<Item>();

    public static List<Item> GetAllBuildings()
    {
        return AllBuildings;
    }

    public static Item TownHall = new Item
    {
        ID = 3,
        TypeIdentifier = UnitType.Building,
        Name = "Town Hall",
        Health = 100.0f,
        Armour = 3.0f,
        Prefab = Resources.Load("Models/FactionA/Units/Buildings/Town Hall/TownHall", typeof(GameObject)) as GameObject,
        ItemImage = Resources.Load("Item Images/FactionA/Units/Buildings/Town Hall/TownHall", typeof(Sprite)) as Sprite,
        SortOrder = 0,
        Cost = 700,
        BuildingId = 0,
        BuildTime = 10.0f,
        keyBind = KeyCode.Q,
        attack = new Attack
        {

        }
    };

    public static Item Barracks = new Item
    {
        ID = 2,
        TypeIdentifier = UnitType.Building,
        Name = "Barracks",
        Health = 100.0f,
        Armour = 3.0f,
        Prefab = Resources.Load("Models/FactionA/Units/Buildings/Barracks/Barracks", typeof(GameObject)) as GameObject,
        ItemImage = Resources.Load("Item Images/FactionA/Units/Buildings/Barracks/Barracks", typeof(Sprite)) as Sprite,
        SortOrder = 0,
        Cost = 700,
        BuildingId = 0,
        BuildTime = 10.0f,
        keyBind = KeyCode.W,
        attack = new Attack
        {

        }
    };

    public static Item Peasant = new Item
    {
        ID = 1,
        TypeIdentifier = UnitType.Worker,
        Name = "Peasant",
        Health = 25.0f,
        Armour = 0.0f,
        Prefab = Resources.Load("Models/FactionA/Units/Humanoids/Peasant/Peasant", typeof(GameObject)) as GameObject,
        ItemImage = Resources.Load("Item Images/FactionA/Units/Humanoids/Peasant/Peasant", typeof(Sprite)) as Sprite,
        SortOrder = 0,
        Cost = 100,
        BuildingId = 3,
        BuildTime = 5.0f,
        keyBind = KeyCode.Q,
        attack = new Attack {
            damage = 1,
            range = 1,
        }
    };

    public static void Initialise()
    {
        InitialiseItem(TownHall);
        InitialiseItem(Barracks);
        InitialiseItem(Peasant);
    }

    private static void InitialiseItem(Item item)
    {
        item.Initialise();
        AllBuildings.Add(item);
    }
}
