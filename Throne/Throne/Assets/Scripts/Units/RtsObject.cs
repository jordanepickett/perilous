using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class RtsObject : NetworkBehaviour {

    public UnitType unitType;

	public string Name
    {
        get;
        private set;
    }

    public Team Team
    {
        get;
        set;
    }

    [SerializeField]
    private bool isMovable;
    [SerializeField]
    private bool isAttackable;

    public bool IsMoveable()
    {
        return isMovable;
    }

    public bool IsAttackable()
    {
        return isAttackable;
    }

    public abstract void Command(Icommand command);
}
