using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUpController : MonoBehaviour {

    private static DamagePopUp damagePopUp;
    private static GameObject canvas;

    public static void Initialize(DamagePopUp popUp) {
        canvas = GameObject.Find ("Canvas");
        damagePopUp = popUp;
    }

    public static void CreateDamagePopUp(float damageValue, Transform location) {
        DamagePopUp damagePopUpInstance = Instantiate (damagePopUp);
        float x = location.position.x + Random.Range(-.5f, .5f);
        float y = location.position.y + Random.Range(-.2f, .2f);
        Vector2 worldToScreenPosition = Camera.main.WorldToScreenPoint (new Vector3(x, y, location.position.z));

        damagePopUpInstance.transform.SetParent (canvas.transform);
        damagePopUpInstance.transform.position = worldToScreenPosition;
        damagePopUpInstance.SetDamageValue (damageValue);
    }
}
