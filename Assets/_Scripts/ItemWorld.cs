using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public Item item;
    private void Start()
    {
        if (item == null)
            return;
        GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
}
