using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

public class PlayerController : NetworkBehaviour
{
    public GameObject spawnPrefab;

    private Faction playerFaction;

    public Camera gCamera;

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
        Debug.Log(goldAmount);
        gold += goldAmount;
    }

    public void RemoveGold(int goldAmount)
    {
        Debug.Log("GOLD REMOVED");
        gold -= goldAmount;
    }

    public int Lumber()
    {
        return lumber;
    }

    public void AddLumber(int lumberAmount)
    {
        Debug.Log("LUMBER ADDED " + lumberAmount);
        lumber += lumberAmount;
    }

    public void RemoveLumber(int lumberAmount)
    {
        Debug.Log("LUMBER REMOVED " + lumberAmount);
        lumber -= lumberAmount;
    }

    public Faction GetFaction()
    {
        return playerFaction;
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            gCamera.gameObject.SetActive(true);
            gCamera.gameObject.SetActive(true);
            GameObject miniMap = Resources.Load("Camera") as GameObject;
            miniMap = Instantiate(miniMap) as GameObject;
            if(!FoW.FogOfWar.GetFogOfWarTeam(teamId))
            {
                gCamera.GetComponent<FoW.FogOfWar>().AddInstance(gCamera.GetComponent<FoW.FogOfWar>());
            }
            miniMap.AddComponent<FoW.FogOfWarSecondary>();
            miniMap.GetComponent<FoW.FogOfWarSecondary>().team = teamId;
            miniMap.GetComponent<MinimapSelector>().SetGCamera(gCamera);
            SelectionManager.main.teamId = teamId;
            Debug.Log("LOCAL " + teamId);
        }
        else
        {
            gCamera.gameObject.SetActive(false);
            Debug.Log("NOT LOCAL " + teamId);
        }
    }

    public override void OnStartLocalPlayer()
    {
        //I kept this part in, because I don't know if this is the function that sets isLocalPlayer to true, 
        //or this function triggers after isLocalPlayer is set to true.
        base.OnStartLocalPlayer();
        FactionABuildings.Initialise();

        //playerFaction = gameObject.AddComponent<FactionA>();
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

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isLocalPlayer)
        {

        }
        else
        {
            gCamera.gameObject.SetActive(false);
        }
        CustomNetworkManager.singleton.GetComponent<PlayersContainer>().AddPlayer(gameObject);
        gCamera.GetComponent<FoW.FogOfWar>().team = teamId;
        gCamera.GetComponent<FoW.FogOfWar>().AddInstance(gCamera.GetComponent<FoW.FogOfWar>());
        gCamera.GetComponent<MainCameraTest>().player = this;

    }

    [Command]
    public void CmdCall()
    {
        RpcLog(0);
        //Calling [ClientRpc] on the server.
        FactionABuildings.Initialise();
        UpgradeDB.Initialise();
        //gameObject.AddComponent<FactionA>();
        for (int i = 0; i < 3; i++)
        {
            spawnPrefab.transform.position = transform.position;
            GameObject obj = MonoBehaviour.Instantiate(this.spawnPrefab) as GameObject;
            obj.transform.position = transform.position;
            obj.GetComponent<Unit>().previousPos = transform.position;
            RtsObjectController objController = obj.GetComponent<RtsObjectController>();
            Debug.Log(transform.position + " " + obj.transform.position);
            objController.teamId = teamId;
            objController.instantSpawn = true;
            objController.ParentObject = gameObject;
            objController.color = color;
            objController.factionId = 0;
            obj.GetComponent<RtsObject>().conn = this.connectionToClient;
            objController.SetPlayer(gameObject);
            foreach (var item in GetComponent<FactionA>().GetBuildableBuildings())
            {
                if (item.TypeIdentifier == UnitType.Worker)
                {
                    obj.GetComponent<RtsObjectController>().itemId = item.ID;
                    break;
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
        gold = amount;
    }

    public void OnLumber(int amount)
    {
        lumber = amount;
    }

    void OnGUI()
    {
        if(isLocalPlayer)
        {
            int TextWidth = 100;
            GUI.Box(new Rect(Screen.width - TextWidth, 10, TextWidth, 55), "Current Gold");
            GUI.Box(new Rect(Screen.width - TextWidth, 40, TextWidth, 55), gold.ToString());
            GUI.Box(new Rect(Screen.width - TextWidth, 100, TextWidth, 55), "Current Lumber");
            GUI.Box(new Rect(Screen.width - TextWidth, 130, TextWidth, 55), lumber.ToString());

            GUI.Box(new Rect(Screen.width - (TextWidth + 200), 10, TextWidth, 55), "Food");
            GUI.Box(new Rect(Screen.width - (TextWidth + 200), 40, TextWidth, 55), GetComponent<PlayerUnitController>().unitsExisting + "/" + GetComponent<PlayerUnitController>().allowedUnits);
        }
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        CustomNetworkManager.singleton.GetComponent<PlayersContainer>().RemovePlayer(gameObject);
        RpcRemovePlayer(gameObject);
        Debug.Log("Clean up after player " + player);

        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    [ClientRpc]
    void RpcRemovePlayer(GameObject player)
    {
        CustomNetworkManager.singleton.GetComponent<PlayersContainer>().RemovePlayer(gameObject);
    }
}


