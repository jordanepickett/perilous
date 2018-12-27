using UnityEngine;

public struct Cost
{
    public int Gold;
    public int Lumber;
    public int Food;

    public Cost(int gold, int lumber, int food = 0)
    {
        Gold = gold;
        Lumber = lumber;
        Food = food;
    }
};

public struct Info
{
    public int ID;
    public string Name;
    public string Sprite;
    public string Description;
    public bool IsAvailable;

    public Info(int id, string name, string sprite, string description, bool isAvailable = false)
    {
        ID = id;
        Name = name;
        Sprite = sprite;
        Description = description;
        IsAvailable = isAvailable;
    }
};

public struct GameObjectInfo
{
    public GameObject Prefab;
    public GameObject PrefabPlacement;

    public GameObjectInfo(GameObject prefab, GameObject prefacePlacement = null)
    {
        Prefab = prefab;
        PrefabPlacement = prefacePlacement;
    }
};

public struct BuildInfo
{
    public int BuildTime;
    public int BuildFrom;
    
    public BuildInfo(int buildTime, int buildFrom)
    {
        BuildTime = buildTime;
        BuildFrom = buildFrom;
    }
};

public struct DefenseInfo
{
    public int MaxHealth;
    public int CurrentHealth;
    public int Armor;

    public DefenseInfo(int maxHealth, int armor)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        Armor = armor;
    }
};

public struct AttackInfo
{
    public int MinDamage;
    public int MaxDamage;
    public float AttackSpeed;

    public AttackInfo(int minDamage, int maxDamage, float attackSpeed)
    {
        MinDamage = minDamage;
        MaxDamage = maxDamage;
        AttackSpeed = attackSpeed;
    }
};
