using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FactionABuildings {

    //LAST ID = 8;

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
        Health = 1000.0f,
        Armour = 15.0f,
        Prefab = Resources.Load("Models/FactionA/Units/Buildings/Town Hall/TownHall", typeof(GameObject)) as GameObject,
        PrefabPlacement = Resources.Load("Models/FactionA/Units/Buildings/Town Hall/TownHall_Placement", typeof(GameObject)) as GameObject,
        ItemImage = Resources.Load("Item Images/FactionA/Units/Buildings/Town Hall/TownHall", typeof(Sprite)) as Sprite,
        SortOrder = 0,
        Cost = 320,
        Lumber = 200,
        BuildingId = 0,
        BuildTime = 20.0f,
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
        Health = 700.0f,
        Armour = 10.0f,
        Prefab = Resources.Load("Models/FactionA/Units/Buildings/Barracks/Barracks", typeof(GameObject)) as GameObject,
        PrefabPlacement = Resources.Load("Models/FactionA/Units/Buildings/Barracks/Barracks_Placement", typeof(GameObject)) as GameObject,
        ItemImage = Resources.Load("Item Images/FactionA/Units/Buildings/Barracks/Barracks", typeof(Sprite)) as Sprite,
        SortOrder = 0,
        Cost = 150,
        Lumber = 50,
        BuildingId = 0,
        BuildTime = 15.0f,
        keyBind = KeyCode.W,
        RequiredBuildings = new int[] { 3 },
        attack = new Attack
        {

        }
    };

    public static Item BlackSmith = new Item
    {
        ID = 8,
        TypeIdentifier = UnitType.Building,
        Name = "BlackSmith",
        Health = 500.0f,
        Armour = 8.0f,
        Prefab = Resources.Load("Models/FactionA/Units/Buildings/BlackSmith/BlackSmith", typeof(GameObject)) as GameObject,
        PrefabPlacement = Resources.Load("Models/FactionA/Units/Buildings/BlackSmith/BlackSmith_Placement", typeof(GameObject)) as GameObject,
        ItemImage = Resources.Load("Item Images/FactionA/Units/Buildings/BlackSmith/BlackSmith", typeof(Sprite)) as Sprite,
        SortOrder = 0,
        Cost = 130,
        Lumber = 40,
        BuildingId = 0,
        BuildTime = 15.0f,
        keyBind = KeyCode.R,
        RequiredBuildings = new int[] { 3, 2 },
        attack = new Attack
        {

        }
    };

    public static Item Farm = new Item
    {
        ID = 5,
        TypeIdentifier = UnitType.Building,
        Name = "Farm",
        Health = 300.0f,
        Armour = 0.0f,
        Prefab = Resources.Load("Models/FactionA/Units/Buildings/Farm/Farm", typeof(GameObject)) as GameObject,
        PrefabPlacement = Resources.Load("Models/FactionA/Units/Buildings/Farm/Farm_Placement", typeof(GameObject)) as GameObject,
        ItemImage = Resources.Load("Item Images/FactionA/Units/Buildings/Farm/farm", typeof(Sprite)) as Sprite,
        SortOrder = 0,
        Cost = 75,
        Lumber = 20,
        BuildingId = 0,
        BuildTime = 10.0f,
        keyBind = KeyCode.E,
        attack = new Attack
        {

        }
    };

    public static Item Peasant = new Item
    {
        ID = 1,
        TypeIdentifier = UnitType.Worker,
        Name = "Peasant",
        Health = 30.0f,
        Armour = 0.0f,
        Prefab = Resources.Load("Models/FactionA/Units/Humanoids/Peasant/Peasant", typeof(GameObject)) as GameObject,
        ItemImage = Resources.Load("Item Images/FactionA/Units/Humanoids/Peasant/Peasant", typeof(Sprite)) as Sprite,
        SortOrder = 0,
        Cost = 50,
        BuildingId = 3,
        food = 1,
        BuildTime = 10.0f,
        keyBind = KeyCode.Q,
        attack = new Attack {
            damage = 4,
            range = 1,
            attackSpeed = 1f
        }
    };

    public static Item Infantry = new Item
    {
        ID = 4,
        TypeIdentifier = UnitType.Infantry,
        Name = "Infantry",
        Health = 60.0f,
        Armour = 2.0f,
        Prefab = Resources.Load("Models/FactionA/Units/Humanoids/Infantry/Worker (1)", typeof(GameObject)) as GameObject,
        ItemImage = Resources.Load("Item Images/FactionA/Units/Humanoids/Infantry/Infantry", typeof(Sprite)) as Sprite,
        SortOrder = 0,
        Cost = 120,
        BuildingId = 2,
        food = 2,
        BuildTime = 15.0f,
        keyBind = KeyCode.Q,
        attack = new Attack
        {
            damage = 7,
            range = 1,
            attackSpeed = 1.5f
        }
    };

    public static Item Archer = new Item
    {
        ID = 6,
        TypeIdentifier = UnitType.Archer,
        Name = "Archer",
        Health = 45.0f,
        Armour = 1.0f,
        Prefab = Resources.Load("Models/FactionA/Units/Humanoids/Archer/Capsule", typeof(GameObject)) as GameObject,
        ItemImage = Resources.Load("Item Images/FactionA/Units/Humanoids/Archer/Archer", typeof(Sprite)) as Sprite,
        SortOrder = 0,
        Cost = 165,
        Lumber = 30,
        BuildingId = 2,
        food = 3,
        BuildTime = 18.0f,
        keyBind = KeyCode.W,
        attack = new Attack
        {
            damage = 8,
            range = 4,
            attackSpeed = 2.5f
        }
    };

    public static Item Knight = new Item
    {
        ID = 7,
        TypeIdentifier = UnitType.Knight,
        Name = "Knight",
        Health = 80.0f,
        Armour = 4.0f,
        Prefab = Resources.Load("Models/FactionA/Units/Humanoids/Knight/Cube (6)", typeof(GameObject)) as GameObject,
        ItemImage = Resources.Load("Item Images/FactionA/Units/Humanoids/Knight/Knight", typeof(Sprite)) as Sprite,
        SortOrder = 0,
        Cost = 225,
        Lumber = 60,
        BuildingId = 2,
        food = 4,
        BuildTime = 25.0f,
        keyBind = KeyCode.E,
        RequiredBuildings = new int[] { 5 },
        attack = new Attack
        {
            damage = 13,
            range = 2,
            attackSpeed = 3f
        }
    };

    public static void Initialise()
    {
        InitialiseItem(TownHall);
        InitialiseItem(Barracks);
        InitialiseItem(Peasant);
        InitialiseItem(Infantry);
        InitialiseItem(Farm);
        InitialiseItem(Archer);
        InitialiseItem(Knight);
    }

    private static void InitialiseItem(Item item)
    {
        item.Initialise();
        AllBuildings.Add(item);
    }
}
