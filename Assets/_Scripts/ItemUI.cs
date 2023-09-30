using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Item item;
    public int x;
    public int _y;
    public static Color defaultSlotColor = new Color(0.472f, 0.472f, 0.472f, 0.510f);
    [SerializeField] Color chosenSlotColor = new Color(0.6f, 0.6f, 0.6f);
    private void Start()
    {
        GetComponent<Image>().sprite = item.sprite;
        if (item.isStackable && item.count > 1)
        {
            GameObject counter = transform.GetChild(0).gameObject;
            counter.SetActive(true);
            counter.GetComponent<TMP_Text>().text = item.count.ToString();
        }
    }
    public void RemoveItem()
    {
        GameObject backgroundSlot = GameManager.instance.inventoryUI.inventorySlots[x, _y].gameObject;
        GameManager.instance.inventoryUI.ClearSelection();
        backgroundSlot.GetComponent<Image>().color = chosenSlotColor;
        GameManager.instance.inventoryUI.RemoveItemButtonChangeItem(() =>
        {
            backgroundSlot.GetComponent<Image>().color = defaultSlotColor;
            GameManager.instance.GetPlayer().GetInventory().RemoveItem(item);
            Destroy(gameObject);
        });
    }
}
