using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : EntityController {

    private int floorMask;

    private PlayerInventory inventory;

    void Awake() {
        this.floorMask = LayerMask.GetMask ("Floor");
    }

    protected override void Start () {
        base.Start();
        this.inventory = GetComponent<PlayerInventory> ();
        totalHealth = 100.0f;
        health = totalHealth;
    }

    void FixedUpdate () {
        float h = Input.GetAxisRaw ("Horizontal");
        float v = Input.GetAxisRaw ("Vertical");

        if(!fixedFacing) { 
            Turn (); 
        }
        if (!immobile && (h != 0 || v != 0)) { 
            Move (h, v);
        } else {
            this.anim.SetBool ("playIdle", true);
            this.anim.SetBool ("runForward", false);
            this.anim.SetBool ("runBackward", false);
            this.anim.SetBool ("runLeft", false);
            this.anim.SetBool ("runRight", false);
        }
    }

    protected override void Update() {
        base.Update ();
        if (Input.GetMouseButtonDown (1)) {
            Interact ();
        }
        if (Input.GetMouseButtonDown (0) && IsWeaponEquipped()) {
            SwingWeapon();
        }
        if (Input.GetKeyDown (KeyCode.I)) {
            LogInventory ();
        }
        if (Input.GetKeyDown (KeyCode.U)) {
            LogEntityStats ();
        }
    }

    private bool IsWeaponEquipped () {
        EquippableItem rightHandItem = this.inventory.GetItemInSlot(InventorySlot.RightHand);
        EquippableItem leftHandItem = this.inventory.GetItemInSlot (InventorySlot.LeftHand);
        return (rightHandItem && rightHandItem is Weapon) || (leftHandItem && leftHandItem is Weapon);
    }

    private void SwingWeapon() {
        Weapon weapon = this.inventory.GetItemInSlot (InventorySlot.RightHand) as Weapon;
        weapon.Attack ();
    }

    private void Turn () {
        RaycastHit cameraRayFloorHit;
        if (RaycastCameraToMouse(out cameraRayFloorHit, this.floorMask)) {
            RotatePlayerToPosition (cameraRayFloorHit.point);
        }
    }

    private void Interact() {
        RaycastHit cameraRayObjectHit;
        if (RaycastCameraToMouse(out cameraRayObjectHit)) {
            ItemContainer container = cameraRayObjectHit.collider.gameObject.GetComponent<ItemContainer> ();
            if (container) {
                InteractWithItemContainer (container);
            }
        }
    }

    private void InteractWithItemContainer(ItemContainer itemContainer) {
        if (!itemContainer.IsEmpty() && 
            Vector3.Distance(this.transform.position, itemContainer.transform.position) <= itemContainer.GetInteractionRadius()) {

            itemContainer.OpenUI ((item) => {
                this.inventory.AddItem (item);
                if (item is EquippableItem) {
                    this.inventory.Equip (item as EquippableItem);
                }
            });
        }
    }

    /*
     * Rotates the player rigidbody towards the intended target position Vector3.
     */
    private void RotatePlayerToPosition(Vector3 targetPosition) {
        facing = targetPosition - this.transform.position;

        // do not pitch the player position up/down
        facing.y = 0;
        facing.Normalize ();

        Quaternion playerRotationQuaternion = Quaternion.LookRotation (facing);
        this.charRigidbody.MoveRotation (playerRotationQuaternion);
    }

    /*
     * Returns true if the ray cast from the camera through the mouse position intersects
     * a game object.
     */
    private bool RaycastCameraToMouse(out RaycastHit raycastHit) {
        Ray cameraRay = Camera.main.ScreenPointToRay (Input.mousePosition);
        return Physics.Raycast (cameraRay, out raycastHit, Mathf.Infinity);
    }

    /*
     * Returns true if the ray cast from the camera through the mouse position intersects
     * with the specified layer mask.
     */
    private bool RaycastCameraToMouse(out RaycastHit raycastHit, int layerMask) {
        Ray cameraRay = Camera.main.ScreenPointToRay (Input.mousePosition);
        return Physics.Raycast (cameraRay, out raycastHit, Mathf.Infinity, layerMask);
    }

    private void LogInventory() {
        System.Text.StringBuilder sb = new System.Text.StringBuilder ();
        sb.AppendLine ("Inventory (equipped): ");
        foreach (EquippableItem item in this.inventory.GetEquippedItems()) {
            sb.AppendLine (item.GetName() + " (" + item.GetSlot() + ")");
            LogItemStats (item, sb);
        }
        Debug.Log (sb.ToString ());
    }

    private void LogItemStats(EquippableItem item, System.Text.StringBuilder sb) {
        foreach (StatValue statValue in item.GetStats()) {
            string modifier = statValue.GetValue() < 0f ? "-" : "+";
            sb.AppendLine("    " + modifier + statValue.GetValue() + " " + statValue.GetStat());
        }
    }
}
