using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InceptionController : MonoBehaviour {

    private float speed = 5f;
    private Rigidbody charRigidbody;
	// Use this for initialization
	void Start () {
        charRigidbody = GetComponent<Rigidbody> ();
	}
	
    void FixedUpdate () {
        float h = Input.GetAxisRaw ("Horizontal");
        float v = Input.GetAxisRaw ("Vertical");
        Vector3 movement = speed * Time.deltaTime * new Vector3(h, 0f, v);
        charRigidbody.MovePosition (this.transform.position + movement);
    }
}
