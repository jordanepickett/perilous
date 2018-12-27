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

    [SerializeField]
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
        if(GetTarget() && GetTarget() == tar)
        {
            return;
        }
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
        if (!attackCanceled && GetTarget() && GetComponent<Unit>().isMovable)
        {
            if (DetectRange())
            {
                FireAttack();
            }
            else
            {
                //MoveCommand moveCommand = (MoveCommand)UiManager.main.GetUnitCommands().Find(c => c.GetUnitCommand() == UnitCommands.Move);
                //Debug.Log(GetTarget().gameObject.GetComponent<Collider>().ClosestPoint(transform.position));
                //moveCommand.SetPoint(GetTarget().gameObject.GetComponent<Collider>().ClosestPoint(transform.position));
                //MovementManager.main.GiveMovementAttackCommand(moveCommand);
                movement.MoveUnit(GetTarget().gameObject.GetComponent<Collider>().ClosestPoint(transform.position));
            }
        }
    }

    private void FireAttack()
    {
        if (Time.time >= timestamp)
        {
            //GetComponent<Unit>().isMovable = false;
            Debug.Log("FIRE ATTACK");
            transform.LookAt(new Vector3(GetTarget().transform.position.x, transform.position.y, GetTarget().transform.position.z));
            //StartCoroutine(Animation(1f));
            timestamp = Time.time + attackSpeed;
            if (!isServer && hasAuthority)
                CmdSendAttack();
        }
        //if(Time.time >= timestamp + 2f)
        //{
            //GetComponent<Unit>().isMovable = true;
        //}
    }

    public IEnumerator Animation(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponent<Unit>().isMovable = true;
    }

    [Command]
    public void CmdSendAttack()
    {
        int minDamage = damage - 3;
        int maxDamage = damage + 2;
        int rngDamage = Random.Range(minDamage, maxDamage);
        int[] damageInfo = new int[] { rngDamage, piercingDamage, damageType};
        target.GetComponent<Unit>().TakeDamage(damageInfo);
        Debug.Log("Damage sent " + rngDamage);
        RpcDamageSent();
    }

    [ClientRpc]
    public void RpcDamageSent()
    {
        Debug.Log("Damage sent");
        GetComponent<Unit>().isMovable = true;
    }

    private bool DetectRange()
    {
        if(Vector3.Distance(transform.position, GetTarget().gameObject.GetComponent<Collider>().ClosestPoint(transform.position)) <= range + 0.5f)
        {
            movement.Hold(state.ATTACKING);
            return true;
        }
        return false;
    }

    public void CancelAttack(Vector3 point)
    {
        if(hasAuthority && SelectionManager.main.ContainsUnit(GetComponent<SelectableUnit>()))
        {
            CmdCancelAttack(point);
        }
    }

    [Command]
    void CmdCancelAttack(Vector3 point)
    {
        MoveAfterCancel(point);
        RpcCancelAttack(point);
    }

    [ClientRpc]
    void RpcCancelAttack(Vector3 point)
    {
        MoveAfterCancel(point);
    }

    void MoveAfterCancel(Vector3 point)
    {
        target = null;
        if (GetComponent<Unit>().GetState() == state.ATTACKING)
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
