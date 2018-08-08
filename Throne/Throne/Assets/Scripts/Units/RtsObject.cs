using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class RtsObject : NetworkBehaviour {

    public UnitType unitType;

    public Factions faction;

    protected bool isDeployed;

    [SerializeField]
    [SyncVar(hook = "OnHealthChange")]
    public int health;

    private Item item;

    public NetworkConnection conn;

    public Item GetItem()
    {
        return item;
    }

    public void SetItem(Item newItem)
    {
        item = newItem;
        health = (int)newItem.Health;
    }

    public bool IsDeployed()
    {
        return isDeployed;
    }

    public string Name
    {
        get;
        private set;
    }

    [SerializeField]
    public bool isMovable;
    [SerializeField]
    protected bool isAttackable;
    [SerializeField]
    protected bool isInteractable;

    public bool IsInteractable()
    {
        return isInteractable;
    }

    public bool IsMoveable()
    {
        return isMovable;
    }

    public bool IsAttackable()
    {
        return isAttackable;
    }

    public abstract void Command(Icommand command, bool addToQueue = true);

    public virtual void DisplayPanel()
    {
        UiManager.main.ClearUnitPanel();
    }

    public void OnHealthChange(int health)
    {
        Debug.Log(health);
    }
}
