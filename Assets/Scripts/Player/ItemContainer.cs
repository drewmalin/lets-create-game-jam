using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A GameObject which contains items.
 */
public class ItemContainer : MonoBehaviour {

    [SerializeField]
    private float interactionRadius;
    [SerializeField]
    private List<Item> items;

    public List<Item> GetItems() {
        return this.items;
    }

    public void TakeItem(Item item) {
        if (item == null || !this.items.Contains (item)) {
            return;
        }
        this.items.Remove (item);
    }

    public void PutItem(Item item) {
        if (item == null) {
            return;
        }
        this.items.Add (item);
    }

    public float GetInteractionRadius() {
        return this.interactionRadius;
    }

    public bool IsEmpty() {
        return this.items.Count == 0;
    }
}
