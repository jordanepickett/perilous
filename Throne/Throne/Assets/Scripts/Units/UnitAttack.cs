using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public enum DamageType
{
    PHYSICAL,
    FIRE,
    FROST,
    DARK,
    NATURE
}
public class UnitAttack : NetworkBehaviour {

    [SyncVar]
    public int damage;

    [SyncVar]
    public int damageType;

    public DamageType DamageType { get; set; }

    [SyncVar]
    public int piercingDamage;

    [SyncVar]
    public int range;

    [SyncVar]
    public float attackSpeed;

    private GameObject target;

    private Movement movement;

    private bool attackCanceled = false;

    float timestamp = 0.0f;

    public GameObject GetTarget()
    {
        return target;
    }

    public void SetTarget(GameObject tar)
    {
        CmdSetTarget(tar);
    }

    [Command]
    private void CmdSetTarget(GameObject tar)
    {
        SyncTarget(tar);
        RpcSetTarget(tar);
    }

    [ClientRpc]
    private void RpcSetTarget(GameObject tar)
    {
        SyncTarget(tar);
    }

    private void SyncTarget(GameObject tar)
    {
        target = tar;
        attackCanceled = false;
        GetComponent<Unit>().SetState(state.ATTACKING);
    }

	// Use this for initialization
	void Start () {
        movement = GetComponent<Movement>();
        EventsManager.main.RightMouseClick += CancelAttack;
    }
	
	public void Attack()
    {
        if (!attackCanceled && GetTarget())
        {
            if (DetectRange())
            {
                FireAttack();
            }
            else
            {
                movement.MoveUnit(GetTarget().transform.position);
            }
        }
    }

    private void FireAttack()
    {
        if (Time.time >= timestamp)
        {
            GetComponent<Unit>().isMovable = false;
            timestamp = Time.time + attackSpeed;
            if (!isServer)
                CmdSendAttack();
        }
        GetComponent<Unit>().isMovable = true;
    }

    [Command]
    public void CmdSendAttack()
    {
        int[] damageInfo = new int[] { damage, piercingDamage, damageType};
        target.GetComponent<Unit>().TakeDamage(damageInfo);
        Debug.Log("Damage sent " + damageInfo[0]);
        RpcDamageSent();
    }

    [ClientRpc]
    public void RpcDamageSent()
    {
        Debug.Log("Damage sent");
    }

    private bool DetectRange()
    {
        if(Vector3.Distance(transform.position, GetTarget().transform.position) < range)
        {
            movement.Hold(state.ATTACKING);
            return true;
        }
        return false;
    }

    public void CancelAttack(Vector3 point)
    {
        if(GetComponent<Unit>().GetState() == state.ATTACKING)
        {
            GetComponent<Unit>().SetState(state.IDLE);
            attackCanceled = true;
        }
    }

    private void OnDestroy()
    {
        EventsManager.main.RightMouseClick -= CancelAttack;
    }
}
