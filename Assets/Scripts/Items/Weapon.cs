using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : EquippableItem {

    [SerializeField]
    protected float minDamage;
    [SerializeField]
    protected float maxDamage;
    [SerializeField]
    protected float attackSpeed;

    public float GetMinDamage() {
        return this.minDamage;
    }

    public float GetMaxDamage() {
        return this.maxDamage;
    }

    public float GetAttackSpeed() {
        return this.attackSpeed;
    }

    public float GetDamagePerSecond() {
        return (((this.maxDamage + this.minDamage) / 2 ) / this.attackSpeed);
    }

    virtual public void Attack(Vector3 startFrom, Vector3 direction) {

    }
}
