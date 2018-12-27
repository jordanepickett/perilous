using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RtsObjectController : NetworkBehaviour
{

    [SerializeField]
    [SyncVar(hook = "OnItem")]
    public int itemId;

    [SerializeField]
    [SyncVar(hook = "OnFaction")]
    public int factionId;

    [SerializeField]
    [SyncVar(hook = "OnTeam")]
    public int teamId;

    [SyncVar]
    public GameObject ParentObject;

    public GameObject localPlayer;

    public Team Team
    {
        get; set;
    }

    [SyncVar]
    public bool instantSpawn = false;

    public Image healthBar;

    [SerializeField]
    [SyncVar(hook = "OnColor")]
    public Color color;

    [SerializeField]
    [SyncVar(hook = "OnDamage")]
    public int damage;

    [SerializeField]
    [SyncVar(hook = "OnPlayer")]
    private GameObject player;

    public GameObject GetPlayer()
    {
        return player;
    }

    public void SetPlayer(GameObject newPlayer)
    {
        Debug.Log("WE ARE THE SERVER");
        player = newPlayer;
    }

    void OnPlayer(GameObject obj)
    {
        player = obj;
        GetComponent<FactionA>().SyncUnitAvailability();
    }

    private void Awake()
    {
        //TODO: THIS NEEDS TO BE CHANGED
        //gameObject.AddComponent<UnitAttack>();
    }

    public void OnItem(int itemId)
    {
        if(hasAuthority)
        {
            CmdSetUnitItem(itemId);
        }
        else
        {
            SetUnitItem(itemId);
        }
    }

    [Command]
    public void CmdSetUnitItem(int itemId)
    {
        SetUnitItem(itemId);
        RpcSetUnitItem(itemId);
    }

    [ClientRpc]
    public void RpcSetUnitItem(int itemId)
    {
        SetUnitItem(itemId);
    }

    private void SetUnitItem(int itemId)
    {
        foreach (var item in GetComponent<FactionA>().GetBuildableBuildings())
        {
            if (itemId == item.ID)
            {
                //Debug.Log("ITEM NAME = " + item.Name);
                //Debug.Log("ITEM COST = " + item.Lumber);
                GetComponent<RtsObject>().SetItem(item);
                if (isServer)
                {
                    damage = (int)item.attack.damage;
                    GetPlayer().GetComponent<PlayerController>().RemoveGold(item.Cost);
                    GetPlayer().GetComponent<PlayerController>().RemoveLumber(item.Lumber);
                }
                if (!instantSpawn)
                {
                    GetComponent<Unit>().DeployPlacement();
                }
                if(ParentObject && ParentObject.GetComponent<RtsObject>() &&
                    (ParentObject.GetComponent<Unit>().unitType == UnitType.Worker || (ParentObject.GetComponent<Unit>().unitType == UnitType.Building)))
                {
                    StartCoroutine(ParentObject.GetComponent<Unit>().BuildingLength(GetComponent<RtsObject>().GetItem().BuildTime));
                }

                break;
            }
        }
    }

    public void OnFaction(int factionId)
    {
        GetComponent<RtsObject>().faction = (Factions)factionId;
    }

    public void OnTeam(int teamId)
    {
        if(hasAuthority)
        {
            CmdSetTeam(teamId);
        }
        else
        {
            SetTeam(teamId);
        }
    }

    [Command]
    public void CmdSetTeam(int teamId)
    {
        SetTeam(teamId);
        RpcSetTeam(teamId);
    }

    [ClientRpc]
    public void RpcSetTeam(int teamId)
    {
        SetTeam(teamId);
    }

    public void SetTeam(int teamId)
    {
        Team = (Team)teamId;
    }

    public void OnColor(Color color)
    {
        CmdSetColor(color);
    }

    [Command]
    public void CmdSetColor(Color color)
    {
        SetColor(color);
        RpcSetColor(color);
    }

    [ClientRpc]
    public void RpcSetColor(Color color)
    {
        SetColor(color);
    }

    public void SetColor(Color color)
    {
        //Fetch the Renderer from the GameObject
        Renderer rend = GetComponent<Renderer>();
        var renderes = GetComponentsInChildren<Renderer>();
        foreach(var rendered in renderes)
        {
            rendered.material.SetColor("_Color", color);
        }

        //Set the main Color of the Material to green
        //rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", color);
    }

    public void OnDamage(int damage)
    {
        if(hasAuthority)
            CmdSetUnitAttack(damage);
    }

    [Command]
    public void CmdSetUnitAttack(int damage)
    {
        SetUnitAttack(damage);
        RpcSetUnitAttack(damage);
    }

    [ClientRpc]
    public void RpcSetUnitAttack(int damage)
    {
        SetUnitAttack(damage);
    }

    public void SetUnitAttack(int damage)
    {
        UnitAttack unitAttack = GetComponent<UnitAttack>();
        RtsObject obj = GetComponent<RtsObject>();
        //GetComponent<UnitAttack>().DamageType = (DamageType)damage;
        unitAttack.damage = damage;
        Debug.Log(obj.GetItem());
        unitAttack.piercingDamage = obj.GetItem().attack.piercingDamage;
        unitAttack.range = obj.GetItem().attack.range;
        unitAttack.attackSpeed = obj.GetItem().attack.attackSpeed;
        unitAttack.damageType = obj.GetItem().attack.damageType;
        unitAttack.DamageType = (DamageType)unitAttack.damageType;
    }

    public override void OnStartClient()
    {
        SetColor(color);
        GetComponent<FoW.FogOfWarUnit>().team = teamId;
        GetComponent<FoW.HideInFog>().team = teamId;
    }

    public bool isSameTeam(RtsObjectController objController)
    {
        if(objController.teamId != GetComponent<RtsObjectController>().teamId)
        {
            return false;
        }
        return true;
    }

    public void SetVisible()
    {
        GetComponent<Renderer>().enabled = true;
    }

    public void SetInvisible()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
