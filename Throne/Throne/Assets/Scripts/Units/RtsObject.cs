using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class RtsObject : NetworkBehaviour {

    public UnitType unitType;

    public Factions faction;

    protected bool isDeployed;

    [SyncVar]
    public int health;

    [SyncVar]
    public int armor;

    [SerializeField]
    [SyncVar(hook = "OnHealthChange")]
    public int currentHealth;

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
        armor = (int)newItem.Armour;
        currentHealth = health;
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

    public override void OnStartClient()
    {
        base.OnStartClient();
        //health = (int)GetItem().Health;
        currentHealth = health;
    }

    public void OnHealthChange(int health)
    {
        currentHealth = health;
        if(currentHealth > this.health)
        {
            currentHealth = this.health;
        }

        UpdateUIHealth();
    }

    public Sprite GetUnitImage()
    {
        return GetItem().ItemImage;
    }

    void UpdateUIHealth()
    {
        GetComponent<RtsObjectController>().healthBar.fillAmount = Utils.Map(currentHealth, this.health);

        if (SelectionManager.main.GetSelectedUnits().Count > 0 && SelectionManager.main.FirstUnit() == GetComponent<SelectableUnit>())
        {
            UpdateUnitPortrait();
        }
    }

    void UpdateUnitPortrait()
    {
        UiManager.main.unitPortrait.UpdatePortraitHealthAndMana(currentHealth, health, currentHealth, health);
    }
}
