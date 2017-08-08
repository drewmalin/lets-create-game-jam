using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An item which may provide stats by being equipped into a particular
 * inventory slot.
 */
public class EquippableItem : Item {

    [SerializeField]
    private InventorySlot slot;
    [SerializeField]
    private List<StatValue> stats;

    /*
     * The stat values provided by this item.
     */
    public List<StatValue> GetStats() {
        return this.stats;
    }

    /*
     * The inventory slot into which this item may be equipped.
     */
    public InventorySlot GetSlot() {
        return this.slot;
    }
}
