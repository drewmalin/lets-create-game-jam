using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilTreatPedestal : ItemContainer {

    private bool devilTreatExists = true;

    void Update () {
        if (devilTreatExists && IsEmpty ()) {
            GameObject.Find ("PedestalDevilTreatModel").SetActive(false);
            devilTreatExists = false;
        }
    }
}
