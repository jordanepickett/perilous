using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UpgradeDB
{

    private static List<Upgrade> AllUpgrades = new List<Upgrade>();

    public static List<Upgrade> GetAllUpgrades()
    {
        return AllUpgrades;
    }

    public static Upgrade InfantryArmorUpgrade = new Upgrade
    {
        Info = new Info(1, "Defensive Prowess", "Upgrade Images/FactionA/Units/Infantry/Damage/Upgrade", "Upgrade Infantry Armor", true),
        GameObjectInfo = new GameObjectInfo(null),
        BuildingInfo = new BuildInfo(10, 3),
        UnitTypes = new List<UnitType>() { UnitType.Infantry },
        UpgradeType = UpgradeType.DEFENSE,
        Numbers = new UpgradeNumbers(5, 2),
        Costruct = new Cost(120, 70),

    };

    public static Upgrade InfantryDamageUpgrade = new Upgrade
    {
        Info = new Info(1, "Offensive Prowess", "Upgrade Images/FactionA/Units/Infantry/Damage/Upgrade", "Upgrade Infantry Weaponry", true),
        GameObjectInfo = new GameObjectInfo(null),
        BuildingInfo = new BuildInfo(10, 3),
        UnitTypes = new List<UnitType>() { UnitType.Infantry },
        UpgradeType = UpgradeType.OFFENSE,
        Numbers = new UpgradeNumbers(6, 2),
        Costruct = new Cost(160, 90),

    };

    public static void Initialise()
    {
        InitialiseUpgrade(InfantryArmorUpgrade);
        InitialiseUpgrade(InfantryDamageUpgrade);
    }

    private static void InitialiseUpgrade(Upgrade upgrade)
    {
        //upgrade.Initialise();
        AllUpgrades.Add(upgrade);
    }
}

