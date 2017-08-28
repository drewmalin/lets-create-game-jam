using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public List<Item> GetAllItems() {
        return this.items;
    }

    public void OpenUI(System.Action<Item> onTakeItemAction) {
        ItemContainerUI itemContainerUI = GameObject.Find ("ItemContainerUI").GetComponent<ItemContainerUI> ();

        if (itemContainerUI.IsOpen ()) {
            return;
        }

        itemContainerUI.Open (); // must be made active prior to all subsequent calls
        itemContainerUI.AddItems (GetAllItems ());
        itemContainerUI.SetItemSelectionAction ((item) => {

            // by default, remove the item from the container and UI
            TakeItem (item);
            itemContainerUI.RemoveItem (item);

            if (IsEmpty()) {
                itemContainerUI.Close();
            }

            // custom behavior
            onTakeItemAction.Invoke(item);
        });
    }
}
