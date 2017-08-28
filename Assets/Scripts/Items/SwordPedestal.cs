using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPedestal : ItemContainer {
	
    private bool swordExists = true;

	void Update () {
        if (swordExists && IsEmpty ()) {
            GameObject.Find ("PedestalSwordModel").SetActive(false);
            swordExists = false;
        }
	}
}
