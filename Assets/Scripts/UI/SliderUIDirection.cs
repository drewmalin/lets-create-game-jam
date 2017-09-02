using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Behavior which, when added to the Slider UI, will fix the initial
 * rotation for the object's lifetime, ignoring parent rotation.
 */
public class SliderUIDirection : MonoBehaviour {

    private Quaternion rotation;

	void Start () {
        this.rotation = this.transform.parent.localRotation;
	}
	
	void Update () {
        this.transform.rotation = this.rotation;
	}
}
