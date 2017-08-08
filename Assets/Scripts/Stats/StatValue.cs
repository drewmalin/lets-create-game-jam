using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A tuple pairing a single Stat with a numeric value.
 */
[System.Serializable]
public class StatValue {

    [SerializeField]
    private Stat stat;
    [SerializeField]
    private float value;

    public StatValue(Stat stat, float value) {
        this.stat = stat;
        this.value = value;
    }

    public Stat GetStat() {
        return this.stat;
    }

    public float GetValue() {
        return this.value;
    }
}
