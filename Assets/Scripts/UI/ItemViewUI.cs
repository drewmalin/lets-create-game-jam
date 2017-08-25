using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * The line-level single item view of a greater ItemContainerUI. Contains basic information regarding a specific item. Instances
 * of this class are automatically instantiated from within ItemContainerUI.cs. The GameObject on which this script is attached
 * is expected to include a button, allowing the entire object to be clickable.
 */
public class ItemViewUI : MonoBehaviour {

    public Item item;

    private Text itemNameText;

    public delegate void OnItemSelectionFunction(Item item);
    public OnItemSelectionFunction onItemSelectionFunction;

    public void Awake() {
        this.itemNameText = this.transform.Find ("ItemName").GetComponent<Text> ();
    }

    public void SetItem(Item item) {
        this.item = item;
        this.itemNameText.text = item.GetName ();
    }

    /**
     * Note: this method is not referenced via another script, but is instead referenced via the Inspector. See the prefab
     * at Resources/UI/ItemView.
     */
    public void OnItemSelection() {
        this.onItemSelectionFunction (this.item);
    }
}
