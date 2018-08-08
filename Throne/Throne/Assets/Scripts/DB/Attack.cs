using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack {

    public int damage = 0;

    public int damageType = (int)DamageType.PHYSICAL;

    public int piercingDamage = 0;

    public int range = 1;

    public float attackSpeed = 0.5f;

    public Attack()
    {

    }

    public Attack(Attack attack)
    {
        damage = attack.damage;
        damageType = attack.damageType;
        piercingDamage = attack.piercingDamage;
        range = attack.range;
        attackSpeed = attack.attackSpeed;
    }
}
