using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] items;
    // private List<Item> items;

    void Awake()
    {
        items = Resources.LoadAll<Item>("Items/Starting-Items");
        // Item[] temp = Resources.LoadAll<Item>("Items/Starting-Items"); 
        // items = new List<Item>(temp);
        Debug.Log(items.Length);
    }

    public Item CreateItem(string ID)
    {
        if (items != null)
        {
            Debug.Log("Creating Item: " + ID);
            Debug.Log("Items.Length: " + items.Length);
            for (int i = 0; i < items.Length; i++)
            {
                Debug.Log("Input ID = " + "\"" + ID + "\"\n" +
                            "items[i].ID = " + "\"" + items[i].ID + "\"\n");
                if (items[i].ID == ID)
                {
                    return CloneItem(items[i]);

                }
            }
        }
        return (null);
    }

    public Item CloneItem(Item original)
    {
        Item clone = ScriptableObject.CreateInstance<Item>();
        clone.name = clone.name + original.itemName;
        clone.icon = original.icon;
        clone.ID = original.ID;
        clone.itemName = original.itemName;
        clone.description = original.description;
        clone.value = original.value;
        clone.type = original.type;
        return clone;
    }
}