using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum GatherType
{
    GOLD,
    LUMBER
}

public class GatherNode : NetworkBehaviour {

    [SyncVar(hook = "OnGatherTypeId")]
    public int gatherTypeId;

    public GatherType gatherType;

    [SyncVar]
    public int nodeAmount = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Gather()
    {
        nodeAmount -= 2;
        if(nodeAmount <= 0)
        {
            nodeAmount = 0;
            Destroy(gameObject);
        }
    }

    void OnGatherTypeId(int typeId)
    {
        CmdGatherType(typeId);
    }

    [Command]
    void CmdGatherType(int typeId)
    {
        SetGatherType(typeId);
        RpcGatherType(typeId);
    }

    [ClientRpc]
    void RpcGatherType(int typeId)
    {
        SetGatherType(typeId);
    }

    void SetGatherType(int typeId)
    {
        gatherType = (GatherType)typeId;
    }
}
