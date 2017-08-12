using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {

    public float health { get; set; }
    public float totalHealth { get; set; }

    public bool immobile { get; set; }
    public bool fixedFacing { get; set; }
    public bool silent { get; set; }
    public bool cantAttack { get; set; }

    public float endStun;
    public float endFreeze;

    public IDictionary<Stat, float> baseStats;
    public List<Effect> currentEffects;
    public IDictionary<Stat, List<Modifier>> activeModifiers;
    public IDictionary<Stat, float> effectiveStats;

    protected Rigidbody charRigidbody;
    protected Vector3 movement;

    // Use this for initialization
    protected virtual void Start () {
        this.charRigidbody = GetComponent<Rigidbody> ();
        this.immobile = false;
        this.fixedFacing = false;
        this.silent = false;
        this.cantAttack = false;
        this.currentEffects = new List<Effect> ();
        this.baseStats = new Dictionary<Stat, float> ();
        this.effectiveStats = new Dictionary<Stat, float> ();
        this.activeModifiers = new Dictionary<Stat, List<Modifier>> ();
        foreach (Stat s in System.Enum.GetValues(typeof(Stat))) {
            baseStats [s] = 0f;
            effectiveStats [s] = 0f;
            activeModifiers [s] = new List<Modifier> ();
        }
    }

    // Update is called once per frame
    protected virtual void Update () {
        // Remove expired Effects and their associated Modifiers
        for (int i = currentEffects.Count - 1; i >= 0; i--) {
            var e = currentEffects [i];
            if (Time.time - e.start_time > e.duration) {
                activeModifiers [e.modifier.stat].Remove (e.modifier);
                currentEffects.RemoveAt (i);
            }
        }
        // Calculate effective Stats from base and Modifiers
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
                effectiveStats [pair.Key] = baseStats [pair.Key];
            }
        }
        // Check if dead
        if (health == 0f) {
            SetStun (true);
            // temporary...
            Quaternion deathRotationQuaternion = Quaternion.LookRotation (new Vector3(0f, -1f, 0f));
            this.charRigidbody.MoveRotation (deathRotationQuaternion);
        }
        if (endStun != 0f) {
            if (Time.time > endStun) {
                SetStun (false);
                endStun = 0f;
            }
        }
        if (endFreeze != 0f) {
            if (Time.time > endFreeze) {
                SetFreeze (false);
                endFreeze = 0f;
            }
        }

    }

    public void ApplyEffect(Effect e) {
        currentEffects.Add (e);
        activeModifiers [e.modifier.stat].Add (e.modifier);
    }

    public void TakeDamage(float damage) {
        health = Mathf.Max (health - damage, 0f);
    }

    protected void Move (float h, float v) {
        // Set the movement vector
        this.movement.Set (h, 0f, v);

        // Normalize the movement vector and make it proportional to the speed per second.
        this.movement = this.movement.normalized * effectiveStats[Stat.Speed] * Time.deltaTime;

        // Move the character to it's current position plus the movement.
        this.charRigidbody.MovePosition (this.transform.position + this.movement);
    }

    /*
     * Short convenience functions for combination state changes
     */

    /*
    * set character cannot turn, move, or act
    */
    public void SetStun(bool isStunned) {
        this.fixedFacing = isStunned;
        this.immobile = isStunned;
        this.silent = isStunned;
        this.cantAttack = isStunned;
    }

    /*
     * set character cannot turn or move
     */
    public void SetFreeze(bool isFrozen) {
        this.fixedFacing = isFrozen;
        this.immobile = isFrozen;
        this.cantAttack = isFrozen;
    }

    /*
     * Debug utilities
     */
    protected void LogEntityStats() {
        System.Text.StringBuilder sb = new System.Text.StringBuilder ();
        sb.AppendLine ("Entity Stats: ");
        foreach (KeyValuePair<Stat, float> s in this.baseStats) {
            sb.AppendLine (s.Key.ToString () + " (" + s.Value.ToString () + ")");
        }
        foreach (Stat s in System.Enum.GetValues(typeof(Stat))) {
            sb.AppendLine (s.ToString ());
        }
        Debug.Log (sb.ToString ());
    }
}
