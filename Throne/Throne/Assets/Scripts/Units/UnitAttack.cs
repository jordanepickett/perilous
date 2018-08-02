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

    public int Damage { get; set; }

    public DamageType DamageType { get; set; }

    public int PiercingDamage { get; set; }

    public int Range { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Damage);
	}
}
