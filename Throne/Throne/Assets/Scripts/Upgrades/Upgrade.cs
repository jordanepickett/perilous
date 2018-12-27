using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeRank
{
    RANK_1,
    RANK_2,
    RANK_3,
    COMPLETE
}

public enum UpgradeType
{
    DEFENSE,
    OFFENSE,
    ABILITY
}

public struct UpgradeNumbers
{
    public int Amount;

    public int MultiplyByRank;

    public UpgradeNumbers(int amount, int multiply)
    {
        MultiplyByRank = multiply;
        Amount = amount;
    }
}

public class Upgrade : MonoBehaviour{

    public Info Info;

    public BuildInfo BuildingInfo;

    public List<UnitType> UnitTypes;

    public UpgradeType UpgradeType;

    public UpgradeNumbers Numbers;

    public UpgradeRank Rank = UpgradeRank.RANK_1;

    public Cost Costruct;

    public GameObjectInfo GameObjectInfo;

    public bool AtMaxRank = false;

    public Upgrade()
    {

    }

    public Upgrade(Upgrade upgrade)
    {
        Info = upgrade.Info;
        BuildingInfo = upgrade.BuildingInfo;
        UnitTypes = upgrade.UnitTypes;
        UpgradeType = upgrade.UpgradeType;
        Numbers = upgrade.Numbers;
        Rank = upgrade.Rank;
        Costruct = upgrade.Costruct;
        GameObjectInfo = upgrade.GameObjectInfo;
    }

    public void UpgradeToNextRank()
    {
        int rank = (int)Rank;
        rank++;
        UpgradeRank nextRank = (UpgradeRank)rank;
        if(nextRank == UpgradeRank.COMPLETE)
        {
            AtMaxRank = true;
            return;
        }
        Costruct.Gold *= Numbers.MultiplyByRank;
        Costruct.Lumber *= Numbers.MultiplyByRank;

        Numbers.Amount *= Numbers.MultiplyByRank;
    }
}
