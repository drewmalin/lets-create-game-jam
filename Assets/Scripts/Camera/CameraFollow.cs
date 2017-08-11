using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Main camera movement behavior.
 * 
 * This class controls the automatic movement of the main camera, pinning its movement to the position of
 * a provided Transform. Slight movement lag may be introduced by adjusting the movement smoothing field.
 */
public class CameraFollow : MonoBehaviour {

    public Transform cameraTarget;
    public float movementSmoothing = 5f;

    Vector3 vectorToTarget;
	
    void Start() {
        vectorToTarget = this.transform.position - this.cameraTarget.position;
    }

	void Update () {
        Vector3 targetCameraPosition = this.cameraTarget.position + this.vectorToTarget;
        transform.position = Vector3.Lerp (transform.position, targetCameraPosition, movementSmoothing * Time.deltaTime);
	}

    /*
     * Returns a Vector3 representing the movement direction from the camera's current position to the
     * camera's target.
     */
    private Vector3 GetVectorToCameraTarget() {
        return this.transform.position - this.cameraTarget.position;
    }
}
