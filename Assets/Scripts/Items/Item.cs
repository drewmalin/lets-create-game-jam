using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The base interface for in-game items.
 */
public class Item : MonoBehaviour {

    [SerializeField]
    private string itemName;
    [SerializeField]
    private string itemDescription;

    public string GetDescription() {
        return this.itemDescription;
    }

    public string GetName() {
        return this.itemName;
    }
}
