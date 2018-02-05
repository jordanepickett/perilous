using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RtsObject : MonoBehaviour {

    public UnitType unitType;

	public string Name
    {
        get;
        private set;
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

    public abstract void Command(Command command);
}
