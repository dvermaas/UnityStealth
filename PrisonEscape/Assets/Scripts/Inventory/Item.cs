using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        Firearm,
        Tool,
        Poison,
        Explosive,
        Misc,
    }

    public string name;
    public ItemType itemType;
    public int gameObject;
    public int amount;
}
