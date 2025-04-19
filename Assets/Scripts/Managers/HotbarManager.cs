using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HotbarManager : MonoBehaviour
{
    public struct HotbarController
    {
        public Item[] items;
        public GameObject[] slots;
    };


    [SerializeField] public GameObject itemPrefab;
    private GameObject[] hotbarSlots;
    private HotbarController hotbarController;


    public void LoadHotbarController()
    {
        Hotbar hotbar = GameObject.FindGameObjectWithTag("Hotbar").GetComponent<Hotbar>();
        hotbarSlots = GameObject.FindGameObjectsWithTag("Hotbar-Slot");
        hotbarSlots = OrderHotbarSlots(hotbarSlots);

        Debug.Log("Hotbar Length ->" + hotbarSlots.Length);
        Debug.Log("Item Length -> "+ hotbar.items.Length);

        hotbarController.items = hotbar.items;
        hotbarController.slots = hotbarSlots;

        for (int i = 0; i < hotbar.slotCount; i++)
        {
            HotbarSlot slot = hotbarSlots[i].GetComponent<HotbarSlot>();
            if (hotbar.items[slot.slotIndex])
            {
                Debug.Log("Item --> " + slot.slotIndex);
                GameObject draggable = Instantiate(itemPrefab);
                draggable.GetComponent<Draggable>().Initialize(hotbar.items[slot.slotIndex], slot);
                slot.PlaceItem(draggable);
            }
        }
    }

    public void ChangeHotbarOrder(int s1, int s2)
    {
        HotbarSlot firstSlot = hotbarController.slots[s1].GetComponent<HotbarSlot>();
        HotbarSlot secondSlot = hotbarController.slots[s2].GetComponent<HotbarSlot>();

        GameObject firstDraggable = firstSlot.Item;
        GameObject secondDraggable = secondSlot.Item;
        if (firstDraggable)
        {
            firstSlot.RemoveItem();
        }
        if (secondDraggable)
        {
            secondSlot.RemoveItem();
        }

        firstSlot.PlaceItem(secondDraggable);
        secondSlot.PlaceItem(firstDraggable);
        Item tmp = hotbarController.items[s1];
        hotbarController.items[s1] = hotbarController.items[s2];
        hotbarController.items[s2] = tmp;
   }

    public void UpdateHotbar()
    {
        Debug.Log("HotbarUpdate");


        Hotbar hotbar = GameObject.FindGameObjectWithTag("Hotbar").GetComponent<Hotbar>();
        hotbarSlots = GameObject.FindGameObjectsWithTag("Hotbar-Slot");
        hotbarSlots = OrderHotbarSlots(hotbarSlots);
        

        for (int i = 0; i < hotbar.slotCount; i++)
        {
            HotbarSlot slot = hotbarSlots[i].GetComponent<HotbarSlot>();
            if (hotbar.items[i] && slot.Item && slot.Item != hotbar.items[i])
            {
                Debug.Log("Hot bar Update " + i);
                Draggable draggable = slot.Item.GetComponent<Draggable>();
                slot.RemoveItem();
                // slot.PlaceItem();

                // Debug.Log("Item --> " + slot.slotIndex);
                // GameObject draggable = Instantiate(itemPrefab);
                // draggable.GetComponent<Draggable>().Initialize(hotbar.items[i], slot);
                // slot.PlaceItem(draggable);
            }
            else if (!slot.Item)
            {

            }
        }
    }

    private GameObject[] OrderHotbarSlots(GameObject[] hotbarSlots)
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            HotbarSlot outerSlot = hotbarSlots[i].GetComponent<HotbarSlot>();
            for (int j = 0; j < i; j++)
            {
                HotbarSlot innerSlot = hotbarSlots[j].GetComponent<HotbarSlot>();
                if (innerSlot.slotIndex >= outerSlot.slotIndex)
                {
                    GameObject tmp = hotbarSlots[j];
                    hotbarSlots[j] = hotbarSlots[i];
                    hotbarSlots[i] = tmp;
                }
            }
        }
        return hotbarSlots;
    }
}