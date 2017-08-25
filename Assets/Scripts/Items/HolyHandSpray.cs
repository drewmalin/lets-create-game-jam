using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyHandSpray : Weapon {

    public Collider AreaOfEffect;
	
	void Start () {
        this.minDamage = 10f;
        this.maxDamage = 30f;
        this.attackSpeed = 0.2f;
	}
	
    public override void Attack(Vector3 startFrom, Vector3 direction) {
        AreaOfEffect.enabled = true;
    }
}
