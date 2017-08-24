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

    public StatValue strength;
    public StatValue dexterity;
    public StatValue constitution;
    public StatValue speed;
    public StatValue damage; 

    public EntityStats myStats;
    public List<Effect> currentEffects;

    public Animator anim;
    protected Rigidbody charRigidbody;
    protected Vector3 movement;
    protected Vector3 facing;

    public Collider damageArea;

    // Use this for initialization
    protected virtual void Start () {
        this.charRigidbody = GetComponent<Rigidbody> ();
        this.anim = GetComponentInChildren<Animator> ();
        this.immobile = false;
        this.fixedFacing = false;
        this.silent = false;
        this.cantAttack = false;
        this.facing = new Vector3 (0, 0, -1f);
        this.currentEffects = new List<Effect> ();
        this.myStats = new EntityStats ();
        this.myStats.SetBaseStat (strength.GetStat (), strength.GetValue ());
        this.myStats.SetBaseStat (dexterity.GetStat (), dexterity.GetValue ());
        this.myStats.SetBaseStat (constitution.GetStat (), constitution.GetValue ());
        this.myStats.SetBaseStat (speed.GetStat (), speed.GetValue ());
        this.myStats.SetBaseStat (damage.GetStat (), damage.GetValue ());
    }

    // Update is called once per frame
    protected virtual void Update () {
        RemoveExpiredEffects();
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
        myStats.AddModifier(e.modifier);
    }

    public void TakeDamage(float damage) {
        DamagePopUpController.CreateDamagePopUp (damage, this.transform);
        health = Mathf.Max (health - damage, 0f);
    }

    protected void Move (float h, float v) {
        // Set the movement vector
        this.movement.Set (h, 0f, v);
        Vector3 moveUnitVec = Vector3.Normalize (movement);
        float dotProduct = Vector3.Dot (moveUnitVec, facing);
        Vector3 crossProduct = Vector3.Cross (moveUnitVec, facing);
        if (this.anim != null) {
            this.anim.SetBool ("playIdle", false);
            if (dotProduct > 0.6) {
                this.anim.SetBool ("runBackward", false);
                this.anim.SetBool ("runForward", true);
                this.anim.SetBool ("runRight", false);
                this.anim.SetBool ("runLeft", false);
            } else if (dotProduct < 0) {
                this.anim.SetBool ("runBackward", true);
                this.anim.SetBool ("runForward", false);
                this.anim.SetBool ("runRight", false);
                this.anim.SetBool ("runLeft", false);
            } else if (Vector3.Dot (crossProduct, Vector3.up) > 0) {
                this.anim.SetBool ("runBackward", false);
                this.anim.SetBool ("runForward", false);
                this.anim.SetBool ("runRight", false);
                this.anim.SetBool ("runLeft", true);
            } else {
                this.anim.SetBool ("runBackward", false);
                this.anim.SetBool ("runForward", false);
                this.anim.SetBool ("runRight", true);
                this.anim.SetBool ("runLeft", false);
            }
        }
        // If the player is moving and facing in different directions, penalize movement speed
        float strafingPenalty = .75f + .25f * dotProduct;

        // Calculate the effective movement vector and make it proportional to the speed per second.
        this.movement = moveUnitVec
                        * myStats.GetEffectiveStat(Stat.Speed) 
                        * strafingPenalty
                        * Time.deltaTime;

        // Move the character to it's current position plus the movement.
        this.charRigidbody.MovePosition (this.transform.position + this.movement);
    }

    private void RemoveExpiredEffects() {
        for (int i = currentEffects.Count - 1; i >= 0; i--) {
            var e = currentEffects [i];
            if (e.HasExpired()) {
                myStats.RemoveModifier(e.modifier);
                currentEffects.RemoveAt (i);
            }
        }
    }

    public void SetDamageArea(Collider area) {
        GameObject damageAreaGO = this.transform.Find ("DamageArea").gameObject;
        // TODO: This cannot be the right way to do this. 
        // Maybe transfer attributes of area to DamageArea's collider
        // Destroy (damageAreaGO.GetComponent<Collider>());
        // Collider justAdded = damageAreaGO.AddComponent<Collider> () as Collider;
        this.damageArea = area;
    }

    /*
     * Activates the DamageArea currently attached to the Entity
     */
    protected void ActivateDamageArea(float damageAmount) {
        GetComponentInChildren<DamageArea> ().damage = damageAmount;
        this.damageArea.enabled = true;
    }

    protected void DisableDamageArea() {
        this.damageArea.enabled = false;
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
        foreach (KeyValuePair<Stat, float> s in myStats.baseStats) {
            sb.AppendLine (s.Key.ToString () + " (" + s.Value.ToString () + ")");
        }
        foreach (Stat s in System.Enum.GetValues(typeof(Stat))) {
            sb.AppendLine (s.ToString ());
        }
        Debug.Log (sb.ToString ());
    }
}
