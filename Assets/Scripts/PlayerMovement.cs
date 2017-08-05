using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float playerSpeed = 5.0f;

    private Rigidbody playerRigidbody;
    private Vector3 movement;
    private int floorMask;

    void Awake() {
        this.floorMask = LayerMask.GetMask ("Floor");    
    }

    void Start () {
        this.playerRigidbody = GetComponent<Rigidbody> ();
    }

    void FixedUpdate () {
        float h = Input.GetAxisRaw ("Horizontal");
        float v = Input.GetAxisRaw ("Vertical");

        Move (h, v);
        Turn ();
    }

    void Move (float h, float v) {
        // Set the movement vector based on the axis input.
        this.movement.Set (h, 0f, v);

        // Normalize the movement vector and make it proportional to the speed per second.
        this.movement = this.movement.normalized * this.playerSpeed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        this.playerRigidbody.MovePosition (this.transform.position + this.movement);
    }

    void Turn () {
        Ray cameraRay = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit cameraRayFloorHit;
        if (Physics.Raycast (cameraRay, out cameraRayFloorHit, 100f, floorMask)) {
            Vector3 playerToFloorHit = cameraRayFloorHit.point - this.transform.position;
            playerToFloorHit.y = 0;

            Quaternion playerRotation = Quaternion.LookRotation (playerToFloorHit);
            this.playerRigidbody.MoveRotation (playerRotation);
        }
    }
}
