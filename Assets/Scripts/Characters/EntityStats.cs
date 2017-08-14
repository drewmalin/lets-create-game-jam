using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats {

    public IDictionary<Stat, float> baseStats;
    public IDictionary<Stat, List<Modifier>> activeModifiers;
    public IDictionary<Stat, float> effectiveStats;

    public EntityStats () {
        this.baseStats = new Dictionary<Stat, float> ();
        this.effectiveStats = new Dictionary<Stat, float> ();
        this.activeModifiers = new Dictionary<Stat, List<Modifier>> ();
	}

    public void SetBaseStat(Stat s, float v) {
        baseStats [s] = v;
        CalculateEffectiveStats ();
    }

    public float GetEffectiveStat(Stat s) {
        return effectiveStats [s];
    }

    public void AddModifier(Modifier m) {
        activeModifiers [m.stat].Add (m);
        CalculateEffectiveStats ();
    }

    public void RemoveModifier(Modifier m) {
        activeModifiers [m.stat].Remove (m);
        CalculateEffectiveStats ();
    }

    private void CalculateEffectiveStats() {
        foreach (KeyValuePair<Stat, float> pair in baseStats) {
            if (activeModifiers.ContainsKey (pair.Key)) {
                float additive = 0f;
                float multiplicative = 1f;
                foreach (Modifier m in activeModifiers[pair.Key]) {
                    additive += m.additive;
                    multiplicative += m.multiplicative;
                }
                effectiveStats [pair.Key] = (pair.Value + additive) * multiplicative;
            } else {
                effectiveStats [pair.Key] = pair.Value;
            }
        }
    }
}
