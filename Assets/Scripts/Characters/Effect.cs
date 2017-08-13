using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * An effect is a modification to a character's state and a duration
 */
public class Effect {
    public float start_time;
    public float duration;
    public Modifier modifier;

    public Effect(Modifier mod, float duration) {
        this.start_time = Time.time;
        this.modifier = mod;
        this.duration = duration;
    }

    public bool HasExpired() {
        return Time.time - this.start_time > this.duration;
    }
}
