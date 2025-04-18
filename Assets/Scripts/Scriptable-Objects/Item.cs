using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    [TextArea(3, 5)] public string description;
    public int value;
    public ItemType type;
    public enum ItemType { None, Weapon, Potion, Collectible }
    public int ID;
}