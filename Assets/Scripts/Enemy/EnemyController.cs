using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private float hitChance;

    private GameObject target;
    private NavMeshAgent agent;
    private bool targetInRange = false;
    private float deltaTime;

	void Start () {
        this.target = GameObject.FindGameObjectWithTag ("Player");
        this.agent = GetComponent<NavMeshAgent> ();
	}
	
	void Update () {
        this.agent.SetDestination (this.target.transform.position);
        if (this.targetInRange) {
            Attack ();
        }
	}

    void Attack () {
        if (deltaTime == 0f || deltaTime >= this.weapon.GetAttackSpeed()) {
            float hitRoll = Random.Range (0f, 1f);
            if (hitRoll == 1f) {
                // critical!
                float damage = Random.Range (this.weapon.GetMinDamage(), this.weapon.GetMaxDamage()) * 2;
                Debug.Log(this.gameObject + " performs a critical hit on " + this.target + "for " + damage + " damage!");
            }
            else if (hitRoll <= this.hitChance) {
                // hit!
                float damage = Random.Range (this.weapon.GetMinDamage(), this.weapon.GetMaxDamage());
                Debug.Log(this.gameObject + " hits " + this.target + " for " + damage + " damage!");
            }
            else {
                // miss!
                Debug.Log(this.gameObject + " misses " + this.target + "!");
            }
            deltaTime = 0f;
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
