using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

public class PlayerController : NetworkBehaviour
{
    public GameObject spawnPrefab;

    private Faction playerFaction;

    [SerializeField]
    [SyncVar(hook = "OnGold")]
    public int gold;

    [SerializeField]
    [SyncVar(hook = "OnLumber")]
    public int lumber;

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

    public int Lumber()
    {
        return lumber;
    }

    public void AddLumber(int lumberAmount)
    {
        Debug.Log(lumberAmount);
        lumber += lumberAmount;
    }

    public void RemoveLumber(int lumberAmout)
    {
        lumber -= lumber;
    }

    public Faction GetFaction()
    {
        return playerFaction;
    }

    private void Awake()
    {
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

    public void OnGold(int amount)
    {
        gold += amount;
        Debug.Log("GOLD AMOUNT  = " + amount);
    }

    public void OnLumber(int amount)
    {
        lumber += amount;
        Debug.Log("LUMBER AMOUNT = " + amount);
    }

    void OnGUI()
    {
        int TextWidth = 100;
        GUI.Box(new Rect(Screen.width - TextWidth, 10, TextWidth, 55), "Current Gold");
        GUI.Box(new Rect(Screen.width - TextWidth, 40, TextWidth, 55), gold.ToString());
        GUI.Box(new Rect(Screen.width - TextWidth, 100, TextWidth, 55), "Current Lumber");
        GUI.Box(new Rect(Screen.width - TextWidth, 130, TextWidth, 55), lumber.ToString());
    }
}


