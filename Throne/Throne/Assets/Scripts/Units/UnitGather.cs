using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnitGather : NetworkBehaviour
{
    [SerializeField]
    private GameObject gatherNode;

    private GameObject dropOffNode;

    private Movement movement;

    private RtsObjectController controller;

    private Vector3 lastNodePosition;

    private Icommand command;

    [SyncVar]
    bool isNodeDead = false;

    public static int MAX_CARRY_AMOUNT = 5;

    [SyncVar(hook = "OnCarryAmount")]
    public int carryingAmount = 0;

    public GatherType carryingType;

    public void SetIsNodeDead(bool nodeDead)
    {
        isNodeDead = nodeDead;
    }

    public GameObject GetGatherNode()
    {
        return gatherNode;
    }

    public void SetGatherNode(GameObject node)
    {
        CmdSetGatherNode(node);
    }

    [Command]
    private void CmdSetGatherNode(GameObject node)
    {
        SyncGatherNode(node);
        RpcSetGatherNode(node);
    }

    [ClientRpc]
    private void RpcSetGatherNode(GameObject node)
    {
        SyncGatherNode(node);
    }

    private void SyncGatherNode(GameObject node)
    {
        isNodeDead = false;
        gatherNode = node;
        GetComponent<Unit>().SetState(state.GATHERING);
        lastNodePosition = node.transform.position;
    }

    public GameObject GetDropOffNode()
    {
        return dropOffNode;
    }

    public void SetDropOffNode(GameObject obj)
    {
        CmdSetDropOffNode(obj);
    }

    [Command]
    void CmdSetDropOffNode(GameObject obj)
    {
        SyncDropOffNode(obj);
        RpcSetDropOffNode(obj);
    }

    [ClientRpc]
    void RpcSetDropOffNode(GameObject obj)
    {
        SyncDropOffNode(obj);
    }

    void SyncDropOffNode(GameObject obj)
    {
        dropOffNode = obj;
    }

    // Use this for initialization
    void Start()
    {
        movement = GetComponent<Movement>();
        EventsManager.main.RightMouseClick += CancelGather;
        controller = GetComponent<RtsObjectController>();
        command = UiManager.main.GetUnitCommands().Find(c => c.GetUnitCommand() == UnitCommands.Gather);
    }

    public void Gather(Icommand command)
    {
        SetGatherNode(command.GetUnit());
        if(controller.GetPlayer().GetComponent<PlayerUnitController>().FindTownHalls().Count > 0)
        {
            SetDropOffNode(controller.GetPlayer().GetComponent<PlayerUnitController>().FindTownHalls()[0]);
        }
        bool atDestination = true;
        if (GetComponent<Unit>().unitType == UnitType.Worker)
        {
            if (transform.position != command.GetPoint())
            {
                atDestination = false;
                GetComponent<Movement>().MoveUnit(command.GetPoint());
                StartCoroutine(AtGatherPlacement(command.GetPoint(), atDestination));
            }
            else
            {
                CmdGather(command.GetUnit());
            }
        }
        else
        {
            CmdGather(command.GetUnit());
        }
    }

    public IEnumerator AtGatherPlacement(Vector3 gatherPlacement, bool atDestination)
    {
        while (Vector3.Distance(transform.position, gatherPlacement) > 2f)
        {
            yield return null;
        }
        CmdGather(GetGatherNode());
        GetComponent<Movement>().Hold(state.GATHERING);
        Debug.Log("HOLD");
    }

    [Command]
    public void CmdGather(GameObject gatherNode)
    {
        InvokeRepeating("GatherAtNode", 0, 3f);
        RpcGather(gatherNode);
    }

    [ClientRpc]
    public void RpcGather(GameObject gatherNode)
    {
        InvokeRepeating("GatherAtNode", 0, 3f);
    }

    void GatherAtNode()
    {
        if(carryingAmount >= MAX_CARRY_AMOUNT)
        {
            carryingAmount = MAX_CARRY_AMOUNT;
            ReturnToDropOffNode();
        }
        else
        {
            if (gatherNode && gatherNode.GetComponent<GatherNode>().nodeAmount > 0)
            {
                if(isServer)
                {
                    gatherNode.GetComponent<GatherNode>().Gather();
                    carryingAmount += 1;
                }
                carryingType = gatherNode.GetComponent<GatherNode>().gatherType;
                return;
            }
            else
            {
                ReturnToDropOffNode();
            }
        }
        Debug.Log("INVOKE CANCELED");
        CancelInvoke();

    }

    void ReturnToDropOffNode()
    {
        if(GetDropOffNode())
        {
            bool atDestination = true;
            if (transform.position != GetDropOffNode().transform.position)
            {
                atDestination = false;
                GetComponent<Movement>().MoveUnit(GetDropOffNode().transform.position);
                StartCoroutine(AtDropOffPlacement(GetDropOffNode().transform.position, atDestination));
            }
            else
            {
                CmdDropOff(GetDropOffNode());
            }
        }
        else
        {
            CancelGather(transform.position);
        }
    }

    public IEnumerator AtDropOffPlacement(Vector3 dropOffPlacement, bool atDestination)
    {
        while (Vector3.Distance(transform.position, dropOffPlacement) > 1f)
        {
            yield return null;
        }
        if(!isServer)
        {
            CmdDropOff(GetGatherNode());
        }
        GetComponent<Movement>().Hold(state.GATHERING);
    }

    [Command]
    void CmdDropOff(GameObject node)
    {
        if(carryingType == GatherType.GOLD)
        {
            controller.GetPlayer().GetComponent<PlayerController>().AddGold(carryingAmount);
        }
        else
        {
            controller.GetPlayer().GetComponent<PlayerController>().AddLumber(carryingAmount);
        }
        carryingAmount = 0;

        if(GetGatherNode())
        {
            RpcContinueGather(GetGatherNode());
        }
        else
        {
            FindAnotherNode();
        }
    }

    void FindAnotherNode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(lastNodePosition, 3);
        foreach (var collider in hitColliders)
        {
            if (collider.gameObject.GetComponent<GatherNode>() && collider.gameObject.GetComponent<GatherNode>().gatherType == carryingType)
            {
                RpcContinueGather(collider.gameObject);
                break;
            }
        }
    }

    [ClientRpc]
    void RpcContinueGather(GameObject node)
    {
        SetGatherNode(node);
        GatherCommand(node);
    }

    [Command]
    void CmdContinueGather(GameObject node)
    {
        GatherCommand(node);
    }

    void GatherCommand(GameObject node)
    {
        command.SetUnit(node);
        command.SetPoint(node.transform.position);
        Gather(command);
    }

    public void CancelGather(Vector3 point)
    {
        if (GetComponent<Unit>().GetState() == state.GATHERING)
        {
            GetComponent<Unit>().SetState(state.IDLE);
            CancelInvoke();
            CmdCancel();
        }
    }

    [Command]
    void CmdCancel()
    {
        CancelInvoke();
    }

    private void OnDestroy()
    {
        EventsManager.main.RightMouseClick -= CancelGather;
    }

    public void OnCarryAmount(int amount)
    {
        CmdCarryAmount(amount);
    }

    void CmdCarryAmount(int amount)
    {
        RpcCarryAmount(amount);
    }

    void  RpcCarryAmount(int amount)
    {
        carryingAmount = amount;
        Debug.Log("CARRYING AMOUNT = " + carryingAmount);
    }
}
