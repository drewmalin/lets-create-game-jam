using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon {

    public Animator animator;

    void Start () {
        this.animator = GetComponentInChildren<Animator> ();
    }

    public override void Attack() {
        if (animator.gameObject.activeSelf) {
            this.animator.SetTrigger ("Attack");
        }
    }

    void OnTriggerEnter(Collider target) {
        EnemyController enemy = target.GetComponent<EnemyController> ();
        if (!enemy) {
            // todo: super-awesome destructible environments...
            return;
        }
        float damage = Random.Range (GetMinDamage(), GetMaxDamage());
        if (damage > 0) {
            Debug.Log (this.gameObject + " hits " + target + " for " + damage + " damage!");
            enemy.TakeDamage (damage);
        }
    }
}
