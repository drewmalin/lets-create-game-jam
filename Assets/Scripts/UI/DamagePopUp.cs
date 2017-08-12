using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour {

    [SerializeField]
    private Animator animator;
    private Text damageValue;

    void Awake() {
        // reset the popup
        AnimatorClipInfo[] animatorClipInfo = this.animator.GetCurrentAnimatorClipInfo (0);
        Destroy (this.gameObject, animatorClipInfo [0].clip.length);
        this.damageValue = this.animator.GetComponent<Text> ();
    }

    public void SetDamageValue(float value) {
        this.damageValue.text = string.Format ("{0:0.00}", value);
    }
}
