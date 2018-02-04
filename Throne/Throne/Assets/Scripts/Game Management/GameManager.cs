using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
    public GameObject spawnPrefab;

    public override void OnStartLocalPlayer()
    {
        //I kept this part in, because I don't know if this is the function that sets isLocalPlayer to true, 
        //or this function triggers after isLocalPlayer is set to true.
        base.OnStartLocalPlayer();

        //On initialization, make the client (local client and remote clients) tell the server to call on an [ClientRpc] method.
        CmdCall();

    }

    void OnNetworkInstantiate(NetworkMessageInfo info)
    {
        if (GetComponent<NetworkView>().isMine)
            MainCamera.main.enabled = true;
        else
            MainCamera.main.enabled = false;
    }

    [Command]
    public void CmdCall()
    {
        //Calling [ClientRpc] on the server.
        RpcLog();
    }

    [ClientRpc]
    public void RpcLog()
    {
        //First, checks to see what type of recipient is receiving this message. If it's server, the output message should tell the user what the type is.
        Debug.Log("RPC: This is " + (this.isServer ? " Server" : " Client"));

        if (this.isServer)
        {
            //Server code
            //This is run for spawning new non-player objects. Since it is a server calling to all clients (local and remote), it needs to pass in a
            //NetworkConnection that connects from server to THAT PARTICULAR client, who is going to own client authority on the spawned object.
            for(int i = 0; i < 3; i++)
            {
                GameObject obj = MonoBehaviour.Instantiate(this.spawnPrefab) as GameObject;
                NetworkServer.SpawnWithClientAuthority(obj, this.connectionToClient);
                Debug.Log("TEST");
            }
        }
        else
        {
            //Client code
            //I realized this hardly runs. Placed a log message here for completeness.
            Debug.Log("Testing.");
        }
    }
}

