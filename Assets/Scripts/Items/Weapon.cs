using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : EquippableItem {

    [SerializeField]
    private float minDamage;
    [SerializeField]
    private float maxDamage;
    [SerializeField]
    private float attackSpeed;

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
}
