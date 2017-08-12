using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float playerSpeed = 5.0f;

    private Rigidbody playerRigidbody;
    private Vector3 movement;
    private int floorMask;

    private PlayerInventory inventory;

    void Awake() {
        this.floorMask = LayerMask.GetMask ("Floor");
    }

    void Start () {
        this.playerRigidbody = GetComponent<Rigidbody> ();
        this.inventory = GetComponent<PlayerInventory> ();
    }

    void FixedUpdate () {
        float h = Input.GetAxisRaw ("Horizontal");
        float v = Input.GetAxisRaw ("Vertical");

        Move (h, v);
        Turn ();
    }

    void Update() {
        if (Input.GetMouseButton (1)) {
            Interact ();
        }
        if (Input.GetKeyDown (KeyCode.I)) {
            LogInventory ();
        }
    }

    public void TakeDamage(float damageValue) {
        DamagePopUpController.CreateDamagePopUp (damageValue, this.transform);
    }

    private void Move (float h, float v) {
        // Set the movement vector based on the axis input.
        this.movement.Set (h, 0f, v);

        // Normalize the movement vector and make it proportional to the speed per second.
        this.movement = this.movement.normalized * this.playerSpeed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        this.playerRigidbody.MovePosition (this.transform.position + this.movement);
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

            EquippableItem item = itemContainer.GetItems()[0] as EquippableItem;
            if (item) {
                itemContainer.TakeItem (item);
                this.inventory.AddItem (item);
                this.inventory.Equip (item);
            }
        }
    }

    /*
     * Rotates the player rigidbody towards the intended target position Vector3.
     */
    private void RotatePlayerToPosition(Vector3 targetPosition) {
        Vector3 playerRotationVector = targetPosition - this.transform.position;

        // do not pitch the player position up/down
        playerRotationVector.y = 0;

        Quaternion playerRotationQuaternion = Quaternion.LookRotation (playerRotationVector);
        this.playerRigidbody.MoveRotation (playerRotationQuaternion);
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
