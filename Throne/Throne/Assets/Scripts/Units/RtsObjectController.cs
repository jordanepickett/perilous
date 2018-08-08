using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

    public Team Team
    {
        get; set;
    }

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
    }

    private void Awake()
    {
        //TODO: THIS NEEDS TO BE CHANGED
        //gameObject.AddComponent<UnitAttack>();
    }

    public void OnItem(int itemId)
    {
        CmdSetUnitItem(itemId);
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
                Debug.Log("ITEM NAME = " + item.Name);
                GetComponent<RtsObject>().SetItem(item);
                if(isServer)
                {
                    damage = (int)item.attack.damage;
                }
                GetComponent<Unit>().DeployPlacement();
                if(ParentObject && 
                    (ParentObject.GetComponent<Unit>().unitType == UnitType.Worker || (ParentObject.GetComponent<Unit>().unitType == UnitType.Building)))
                {
                    StartCoroutine(ParentObject.GetComponent<Unit>().BuildingLength(GetComponent<RtsObject>().GetItem().BuildTime));
                }
            }
        }
    }

    public void OnFaction(int factionId)
    {
        GetComponent<RtsObject>().faction = (Factions)factionId;
    }

    public void OnTeam(int teamId)
    {
        CmdSetTeam(teamId);
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

        //Set the main Color of the Material to green
        //rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", color);
    }

    public void OnDamage(int damage)
    {
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
        Debug.Log("SETTING DAMAGE " + damage);
        UnitAttack unitAttack = GetComponent<UnitAttack>();
        RtsObject obj = GetComponent<RtsObject>();
        //GetComponent<UnitAttack>().DamageType = (DamageType)damage;
        unitAttack.damage = damage;
        unitAttack.piercingDamage = obj.GetItem().attack.piercingDamage;
        unitAttack.range = obj.GetItem().attack.range;
        unitAttack.attackSpeed = obj.GetItem().attack.attackSpeed;
        unitAttack.damageType = obj.GetItem().attack.damageType;
        unitAttack.DamageType = (DamageType)unitAttack.damageType;
    }

    public override void OnStartClient()
    {
        SetColor(color);
    }

    public bool isSameTeam(RtsObjectController objController)
    {
        if(objController.teamId != GetComponent<RtsObjectController>().teamId)
        {
            return false;
        }
        return true;
    }
}
