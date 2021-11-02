using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();
        AddItem(new Item { name = "Glock-17", itemType = Item.ItemType.Firearm, gameObject = 0, amount = 1 });
        AddItem(new Item { name = "Fiber-Wire", itemType = Item.ItemType.Tool, gameObject = 1, amount = 1 });
        AddItem(new Item { name = "Cyanide", itemType = Item.ItemType.Poison, gameObject = 1, amount = 2 });
        AddItem(new Item { name = "RatPoison", itemType = Item.ItemType.Poison, gameObject = 1, amount = 2 });
    }

    // Add a item to the list
    public void AddItem(Item item)
    {
        itemList.Add(item);
    }

    // Remove item based on index
    public void RemoveItem(int index)
    {
        itemList.RemoveAt(index);
    }

    public void SelectItem(int index)
    {
        Item item = itemList[index];
        RemoveItem(index);
        itemList.Insert(0, item);
    }

    public Item GetSelected()
    {
        return itemList[0];
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
