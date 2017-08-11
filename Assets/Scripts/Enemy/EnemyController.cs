using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    private Transform target;
    private NavMeshAgent agent;

	void Start () {
        this.target = GameObject.FindGameObjectWithTag ("Player").transform;
        this.agent = GetComponent<NavMeshAgent> ();
	}
	
	void Update () {
        this.agent.SetDestination (this.target.position);
	}
}
