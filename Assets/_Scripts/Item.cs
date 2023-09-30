using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    public int id;
    public new string name;
    public Sprite sprite;
    public bool isStackable;
    public int count = 1;

    public Item Copy()
    {
        Item newItem = CreateInstance<Item>();
        newItem.id = id;
        newItem.name = name;
        newItem.sprite = sprite;
        newItem.isStackable = isStackable;
        newItem.count = count;

        return newItem;
    }
}
