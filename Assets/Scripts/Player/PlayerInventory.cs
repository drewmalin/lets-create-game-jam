using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * The controller for the player's inventory and equipped items.
 */
public class PlayerInventory : MonoBehaviour {

    public Transform headInventorySlot;
    public Transform rightHandInventorySlot;
    public Transform leftHandInventorySlot;
    public Transform chestInventorySlot;

    private HashSet<Item> items;
    private Dictionary<InventorySlot, EquippableItem> equipped;

    public PlayerInventory() {
        this.items = new HashSet<Item> ();
        this.equipped = new Dictionary<InventorySlot, EquippableItem> ();
    }

    public List<EquippableItem> GetEquippedItems() {
        return this.equipped.Values.ToList();
    }

    /*
     * Adds the provided item to the player inventory. No-ops if the item is null.
     */
    public void AddItem(Item item) {
        if (item == null) {
            return;
        }
        this.items.Add (item);
    }

    /*
     * Removes the provided item from the player inventory. No-ops if the item is null or if
     * the item does not exist in the inventory.
     */
    public void RemoveItem(Item item) {
        if (item == null) {
            return;
        }
        this.items.Remove (item);
    }

    /*
     * Equips the provided equippable item. No-ops if the item is null or does not exist in
     * the inventory.
     */
    public void Equip(EquippableItem equippableItem) {
        if (equippableItem == null || !this.items.Contains (equippableItem)) {
            return;
        }
        this.equipped.Add (equippableItem.GetSlot (), equippableItem);
        InstantiateItem (equippableItem);
    }

    /*
     * Unequips the provided equippable item. No-ops if the item is null, does not exist in
     * the inventory, or is not equipped.
     */
    public void Unequip(EquippableItem equippableItem) {
        if (equippableItem == null || !this.items.Contains (equippableItem) || !this.equipped.ContainsValue(equippableItem)) {
            return;
        }
        this.equipped.Remove (equippableItem.GetSlot ());
        DestroyItem (equippableItem);
    }

    private void InstantiateItem (EquippableItem equippableItem) {
        Transform itemTransform = GetSlotTransform (equippableItem.GetSlot ());
        Quaternion itemRotation = itemTransform.rotation * equippableItem.transform.rotation;
        Instantiate (equippableItem, itemTransform.position, itemRotation, this.transform);
    }

    private void DestroyItem(EquippableItem equippableItem) {
        Destroy (equippableItem);
    }

    private Transform GetSlotTransform(InventorySlot slot) {
        switch (slot) {
            case InventorySlot.Head: {
                return this.headInventorySlot;
            }
            case InventorySlot.RightHand: {
                return this.rightHandInventorySlot;
            }
            case InventorySlot.LeftHand: {
                return this.leftHandInventorySlot;
            }
            case InventorySlot.Chest: {
                return this.chestInventorySlot;
            }
        }
        throw new System.ArgumentException ("Unknown slot: " + slot);
    }
}
