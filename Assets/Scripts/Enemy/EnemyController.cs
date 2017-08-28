using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : EntityController {

    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private float hitChance;

    private GameObject target;
    private NavMeshAgent agent;
    private bool targetInRange = false;
    private float deltaTime;

	protected override void Start () {
        base.Start ();
        this.target = GameObject.FindGameObjectWithTag ("Player");
        this.agent = GetComponent<NavMeshAgent> ();
        this.totalHealth = 30f;
        this.health = totalHealth;
	}
	
	protected override void Update () {
        base.Update ();
        if (health != 0f) {
            this.agent.SetDestination (this.target.transform.position);
            if (!this.cantAttack && this.targetInRange) {
                Attack ();
            }
        } else {
            this.agent.SetDestination (this.transform.position);
        }
	}

    void Attack () {
        float damage = 0;
        if (deltaTime == 0f || deltaTime >= this.weapon.GetAttackSpeed()) {
            float hitRoll = Random.Range (0f, 1f);
            if (hitRoll >= .9f) {
                // critical!
                damage = Random.Range (this.weapon.GetMinDamage(), this.weapon.GetMaxDamage()) * 2;
//                Debug.Log(this.gameObject + " performs a critical hit on " + this.target + "for " + damage + " damage!");
            }
            else if (hitRoll <= this.hitChance) {
                // hit!
                damage = Random.Range (this.weapon.GetMinDamage(), this.weapon.GetMaxDamage());
//                Debug.Log(this.gameObject + " hits " + this.target + " for " + damage + " damage!");
            }
            else {
                // miss!
                damage = 0;
//                Debug.Log(this.gameObject + " misses " + this.target + "!");
            }
            deltaTime = 0f;
        }
        if (damage > 0) {
            this.target.GetComponent<PlayerController> ().TakeDamage (damage);
        }
        deltaTime += Time.deltaTime;
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject == this.target) {
            this.targetInRange = true;
        }
    }

    void OnTriggerExit (Collider other) {
        if (other.gameObject == this.target) {
            this.targetInRange = false;
        }
    }
}
