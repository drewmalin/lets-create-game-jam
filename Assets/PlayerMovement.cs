using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

  public float playerSpeed = 5.0f;

  private Rigidbody playerRigidbody;
  private Vector3 movement;

	// Use this for initialization
	void Start () {
    playerRigidbody = GetComponent<Rigidbody> ();
	}
	
	void FixedUpdate () {
    float h = Input.GetAxisRaw ("Horizontal");
    float v = Input.GetAxisRaw ("Vertical");

    Move (h, v);
	}

  void Move (float h, float v)
  {
    // TODO: Support rotation of the camera
    // Set the movement vector based on the axis input.
    movement.Set (h, 0f, v);

    // Normalise the movement vector and make it proportional to the speed per second.
    movement = movement.normalized * playerSpeed * Time.deltaTime;

    // Move the player to it's current position plus the movement.
    playerRigidbody.MovePosition (transform.position + movement);
  }
}
