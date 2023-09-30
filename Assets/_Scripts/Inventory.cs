using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory
{
    public List<Item> items = new List<Item>();

    public event System.Action OnInventoryChanged = () => { GameManager.instance.inventoryUI?.RefreshItems(); };

    public Inventory(){ }
    public Inventory(Vector2Int[] itemsIdCount)
    {
        if(itemsIdCount == null)
           return;

        foreach (var item in itemsIdCount)
        {
           AddItemFixedCount(GameManager.instance.GetItemDatabase().ItemById[item.x], item.y);
        }
    }

    public void AddItem(Item item)
    {
        if(item.isStackable && items.Exists((Item inInventory) => { return inInventory.id == item.id; }))
        {
            items.Find((Item inInventory) => { return inInventory.id == item.id; }).count += item.count;
        }
        else
            items.Add(item.Copy());
        OnInventoryChanged?.Invoke();
    }

    public void AddItemFixedCount(Item item, int count)
    {
        Item copy = item.Copy();
        copy.count = count;
        AddItem(copy);
    }

    public void RemoveItem(Item item) 
    {
        items.Remove(items.Find((Item inInventory) => { return inInventory.id == item.id; }));
        OnInventoryChanged?.Invoke();
    }

    public void DecreaseCountByOne(Item item)
    {
        Item inInventory = items.Find((Item inInventory) => { return inInventory.id == item.id; });
        inInventory.count--;
        if (inInventory.count < 1)
            items.Remove(inInventory);

        OnInventoryChanged?.Invoke();
    }

    public bool Contains(Item item)
    {
        return items.Find((Item inInventory) => { return inInventory.id == item.id; }) != null;
    }

    public List<Item> GetItems()
    {
        return items;
    }
}
