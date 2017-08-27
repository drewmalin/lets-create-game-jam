using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : EntityController {

    public int attemptedEntries = 0;

    protected override void Start() {
        base.Start ();
        this.totalHealth = 50f;
        this.health = totalHealth;
    }
	
    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Player") {
            attemptedEntries++;
            Debug.Log ("You have annoyed the bouncer " + attemptedEntries + " times.");
        }
    }
}
