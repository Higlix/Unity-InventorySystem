using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    [SerializeField, Range(0, 20)] public int slotCount; 

    [SerializeField] public Item[] items;

    void Start()
    {
        items = new Item[slotCount];
        ItemManager itemManager = GameObject.FindGameObjectWithTag("Item-Manager").GetComponent<ItemManager>();
        if (itemManager)
        {
            items[0] = itemManager.CreateItem("Item.Weapon.Deadalus");
            items[1] = itemManager.CreateItem("Item.Weapon.Pistol");
            items[7] = itemManager.CreateItem("Item.Weapon.Bomb");
            items[5] = itemManager.CreateItem("Item.Collectible.Key");
            items[6] = itemManager.CreateItem("Item.Weapon.Shotgun");
        }
        GameObject.FindGameObjectWithTag("Hotbar-Manager").GetComponent<HotbarManager>().LoadHotbarController(); 
    }
}
