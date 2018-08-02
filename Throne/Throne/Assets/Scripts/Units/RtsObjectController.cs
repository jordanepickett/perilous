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

    private GameObject player;

    public GameObject GetPlayer()
    {
        return player;
    }

    public void SetPlayer(GameObject newPlayer)
    {
        if(isServer)
        {
            RpcAddPlayerToUnit(newPlayer);
            player = newPlayer;
        }
    }

    private void Awake()
    {
        gameObject.AddComponent<UnitAttack>();
    }

    [ClientRpc]
    public void RpcAddPlayerToUnit(GameObject player)
    {
        GetComponent<RtsObjectController>().SetPlayer(player);
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
                    damage = (int)item.Damage;
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
        //GetComponent<UnitAttack>().DamageType = (DamageType)damage;
        GetComponent<UnitAttack>().Damage = damage;
    }

    public override void OnStartClient()
    {
        SetColor(color);
    }
}
