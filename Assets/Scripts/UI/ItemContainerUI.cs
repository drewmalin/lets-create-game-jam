using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * The ItemContainerUI encompasses the full UI object which will allow for the item browsing and selection. The container
 * includes a primary view panel, a scroll view which houses multiple single-item selections, and a item description panel
 * which will display information about a selected item.
 */
public class ItemContainerUI : MonoBehaviour {

    public RectTransform mainItemContainerPanel;
    public SelectedItemDetailsUI selectedItemDetailsPanel;
    public RectTransform scrollableItemsPanel;

    private ItemViewUI singleItemViewPanel;

    private bool isActive;
    private Dictionary<Item, ItemViewUI> itemAndItemViewPairs;

	void Awake () {
        this.itemAndItemViewPairs = new Dictionary<Item, ItemViewUI> ();
        this.singleItemViewPanel = Resources.Load<ItemViewUI> ("UI/ItemView");
        this.mainItemContainerPanel.gameObject.SetActive (false);
	}

    void Update () {
        if (Input.GetKey (KeyCode.Escape)) {
            Close ();
        }
        this.mainItemContainerPanel.gameObject.SetActive (this.isActive);
	}

    public void Close() {
        Clear ();
        this.isActive = false;
    }

    public void Open() {
        this.isActive = true;
    }

    public bool IsOpen() {
        return this.isActive;
    }

    public void AddItems (List<Item> items) {
        foreach (Item item in items) {
            AddItem (item);
        }
    }

    public void AddItem(Item item) {
        ItemViewUI newItemView = Instantiate (this.singleItemViewPanel);
        newItemView.SetItem (item);
        newItemView.transform.SetParent (this.scrollableItemsPanel);
        newItemView.onItemSelectionFunction += SetOnItemSelectionFunction;
        this.itemAndItemViewPairs.Add (item, newItemView);
    }
        
    public void RemoveItem(Item item) {
        ItemViewUI itemViewUi;
        if (this.itemAndItemViewPairs.TryGetValue (item, out itemViewUi)) {
            itemViewUi.onItemSelectionFunction -= SetOnItemSelectionFunction;
            Destroy (itemViewUi.gameObject);
            this.itemAndItemViewPairs.Remove (item);
            this.selectedItemDetailsPanel.ClearItem ();
        }
    }

    public void SetItemSelectionAction(System.Action<Item> onItemSelectionAction) {
        this.selectedItemDetailsPanel.SetItemSelectionAction (onItemSelectionAction);
    }

    public void Clear() {
        List<Item> items = new List<Item>(this.itemAndItemViewPairs.Keys);
        for (int i = 0; i< items.Count; i++) {
            RemoveItem (items[i]);
        }
        this.itemAndItemViewPairs.Clear ();
        this.selectedItemDetailsPanel.Clear ();
    }

    /*
     * Default callback provided to every ItemViewPanel -- upon selection, populate the details panel
     * with information specific to the selected item.
     */
    private void SetOnItemSelectionFunction(Item item) {
        this.selectedItemDetailsPanel.SetItem (item);
    }
}
