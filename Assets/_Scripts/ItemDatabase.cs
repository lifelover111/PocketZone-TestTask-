using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] List<Item> items;

    public Dictionary<string, Item> ItemByName = new Dictionary<string, Item>();
    public Dictionary<int, Item> ItemById = new Dictionary<int, Item>();

    public void InitDictionary()
    {
        foreach (var item in items)
        {
            ItemById.Add(item.id, item.Copy());
            ItemByName.Add(item.name, item.Copy());
        }
    }
}
