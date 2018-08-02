using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

public class PlayerController : NetworkBehaviour
{
    public GameObject spawnPrefab;

    private Faction playerFaction;

    private int gold, lumber;

    [SerializeField]
    [SyncVar(hook = "OnTeam")]
    public int teamId;

    public Color color; 

    public Team Team
    {
        get; set;
    }

    public int Gold()
    {
        return gold;
    }

    public void AddGold(int goldAmount)
    {
        gold += goldAmount;
    }

    public void RemoveGold(int goldAmount)
    {
        gold -= goldAmount;
    }

    public Faction GetFaction()
    {
        return playerFaction;
    }

    public override void OnStartLocalPlayer()
    {
        //I kept this part in, because I don't know if this is the function that sets isLocalPlayer to true, 
        //or this function triggers after isLocalPlayer is set to true.
        base.OnStartLocalPlayer();
        FactionABuildings.Initialise();

        playerFaction = gameObject.AddComponent<FactionA>();
        playerFaction = GetComponent<Faction>();
        playerFaction.SetName("Human");
        //On initialization, make the client (local client and remote clients) tell the server to call on an [ClientRpc] method.
        CmdCall();

    }

    void OnNetworkInstantiate(NetworkMessageInfo info)
    {
        if (GetComponent<NetworkView>().isMine)
        {
            MainCamera.main.enabled = true;
            Debug.Log("camera enabled");
        }
        else
        {
            MainCamera.main.enabled = false;
        }
    }

    [Command]
    public void CmdCall()
    {
        //Calling [ClientRpc] on the server.
        FactionABuildings.Initialise();
        gameObject.AddComponent<FactionA>();
        RpcLog(0);
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = MonoBehaviour.Instantiate(this.spawnPrefab) as GameObject;
            obj.GetComponent<RtsObjectController>().teamId = teamId;
            obj.GetComponent<RtsObjectController>().color = color;
            obj.GetComponent<RtsObjectController>().factionId = 0;
            obj.GetComponent<RtsObject>().conn = this.connectionToClient;
            obj.GetComponent<RtsObjectController>().SetPlayer(gameObject);
            foreach (var item in GetComponent<FactionA>().GetBuildableBuildings())
            {
                if (item.TypeIdentifier == UnitType.Worker)
                {
                    obj.GetComponent<RtsObjectController>().itemId = item.ID;
                }
            }
            NetworkServer.SpawnWithClientAuthority(obj, this.connectionToClient);
         
        }
    }

    [ClientRpc]
    public void RpcLog(int factionId)
    {

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
}


