using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class RtsObject : NetworkBehaviour {

    public UnitType unitType;

    protected bool isDeployed;

    private Item item;

    public Item GetItem()
    {
        return item;
    }

    public void SetItem(Item item)
    {
        this.item = item;
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

    public Team Team
    {
        get;
        set;
    }

    [SerializeField]
    protected bool isMovable;
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
}
