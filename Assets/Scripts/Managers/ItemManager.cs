using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private List<Item> items;

    void Awake()
    {
        Item[] temp = Resources.LoadAll<Item>("Items/Starting-Items"); 
        items = new List<Item>(temp);
    }

    public Item CreateItem(int ID)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == ID)
                return (CloneItem(items[i]));
        }
        return (null);
    }

    public Item CloneItem(Item original)
    {
        Item clone = new Item();
        clone.icon = original.icon;
        clone.ID = original.ID;
        clone.itemName = original.itemName;
        clone.description = original.description;
        clone.value = original.value;
        clone.type = original.type;
        return (clone);
    }
}