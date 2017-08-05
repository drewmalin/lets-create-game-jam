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

    private void Move (float h, float v) {
        // Set the movement vector based on the axis input.
        this.movement.Set (h, 0f, v);

        // Normalize the movement vector and make it proportional to the speed per second.
        this.movement = this.movement.normalized * this.playerSpeed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        this.playerRigidbody.MovePosition (this.transform.position + this.movement);
    }

    private void Turn () {
        RaycastHit cameraRayFloorHit;
        if (isMouseOverFloor(out cameraRayFloorHit)) {
            RotatePlayerToPosition (cameraRayFloorHit.point);
        }
    }

    /*
     * Rotates the player rigidbody towards the intended target position Vector3.
     */
    private void RotatePlayerToPosition(Vector3 targetPosition) {
        Vector3 playerRotationVector = targetPosition - this.transform.position;

        // do not pitch the player position up/down
        playerRotationVector.y = 0;

        Quaternion playerRotationQuaternion = Quaternion.LookRotation (playerRotationVector);
        this.playerRigidbody.MoveRotation (playerRotationQuaternion);
    }

    /*
     * Returns true if the ray cast from the camera through the mouse position intersects
     * the "Floor" layer.
     */
    private bool isMouseOverFloor(out RaycastHit raycastHit) {
        Ray cameraRay = Camera.main.ScreenPointToRay (Input.mousePosition);
        return Physics.Raycast (cameraRay, out raycastHit, 100f, this.floorMask);
    }
}
