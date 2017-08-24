using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour {
    public float damage;

    public void OnTriggerEnter(Collider other) {
        EntityController victim = other.GetComponentInParent<EntityController> ();
        if (victim != null) {
            victim.TakeDamage (damage);
        }
    }
}
