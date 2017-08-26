using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/**
 * The details section of a greater ItemContainerUI. Contains detailed information about a specific item, and houses a button
 * which, when clicked, will invoke custom behavior.
 */
public class SelectedItemDetailsUI : MonoBehaviour {

    public Button selectItemButton;
    public Text itemNameText;
    public Text itemDescriptionText;

    private Item item;

    public void SetItem(Item item) {
        this.item = item;
        this.itemNameText.text = this.item.GetName ();
        this.itemDescriptionText.text = this.item.GetDescription ();
    }

    public void SetItemSelectionAction(System.Action<Item> onItemSelectionAction) {
        this.selectItemButton.onClick.AddListener (() => onItemSelectionAction.Invoke(this.item));
    }

    public void ClearItem() {
        this.item = null;
        this.itemNameText.text = "";
        this.itemDescriptionText.text = "";
    }

    public void Clear() {
        ClearItem ();
        this.selectItemButton.onClick.RemoveAllListeners();
    }
}
