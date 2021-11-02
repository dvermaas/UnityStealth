using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private Button currentItem;

    private void Awake()
    {
        itemSlotContainer = transform.Find("inventory_container");
        itemSlotTemplate = itemSlotContainer.Find("inventory_template");
        currentItem = transform.Find("inventory_selected").GetComponent<Button>();
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        // Update held item on startup, kinda hacky
        currentItem.GetComponentInChildren<Text>().text = inventory.GetSelected().name;
    }

    // add->end insert->front
    public void CheckInventory()
    {
        // Destroy the old inventory
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        // Make new inventory
        int y = 0;
        itemSlotContainer.gameObject.SetActive(true);
        currentItem.Select();
        foreach (Item item in inventory.GetItemList().Skip(1))
        {
            float itemSlotCellSize = 56.7f;
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.gameObject.GetComponentInChildren<Text>().text = item.name;
            // If this is not done all buttons will return max y val, kinda cringe (you get pointers like python lists).
            int x = y + 1;
            itemSlotRectTransform.gameObject.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(x));
            itemSlotRectTransform.anchoredPosition = new Vector2(0, y * itemSlotCellSize);
            y++;
        }
    }

    public void ButtonClicked(int buttonNo)
    {
        inventory.SelectItem(buttonNo);
        currentItem.GetComponentInChildren<Text>().text = inventory.GetSelected().name;
    }
}
