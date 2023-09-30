using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] RectTransform background;
    [SerializeField] RectTransform items;
    [SerializeField] Button removeItemButton;
    [SerializeField] GameObject itemTemplatePrefab;
    [SerializeField] GameObject inventorySlotPrefab;
    [SerializeField] Vector2Int inventorySize;
    [SerializeField] float gap = 200;
    public Transform[,] inventorySlots;
    Inventory inventory;
    bool inventoryDisplayed = false;
    private void Start()
    {
        GameManager.instance.inventoryUI = this;
        player.OnPlayerDied += () => { if (inventoryDisplayed) SwitchDisplaying(); };
        CreateBackground();
    }
    void CreateBackground()
    {
        inventorySlots = new Transform[inventorySize.x, inventorySize.y];
        float avgGapX = background.sizeDelta.x/inventorySize.x;
        float avgGapY = background.sizeDelta.y/inventorySize.y;
        gap = avgGapX; //(avgGapX + avgGapY) / 2;
        for(int i = 0; i < inventorySize.x; i++)
        {
            for(int j = 0; j < inventorySize.y; j++)
            {
                GameObject go = Instantiate(inventorySlotPrefab);
                go.transform.SetParent(background, false);
                go.GetComponent<RectTransform>().anchoredPosition = new Vector3(gap/2 + gap * i, -(gap/2 + gap * j), 0);
                inventorySlots[i, j] = go.transform;
            }
        }
    }
    public void RefreshItems()
    {
        if (!inventoryDisplayed)
            return;
        for (int i = 0; i < items.childCount; i++)
            Destroy(items.GetChild(i).gameObject);

        inventory = player.GetInventory();
        int x = 0;
        int y = 0;
        foreach (var item in inventory.GetItems())
        {
            GameObject go = Instantiate(itemTemplatePrefab);
            go.transform.SetParent(items, false);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector3(gap/2 + gap * x, -(gap/2 + gap * y), 0);
            ItemUI itemUI = go.GetComponent<ItemUI>();
            itemUI.item = item;
            itemUI.x = x;
            itemUI._y = y;
            x++;
            if(x > inventorySize.x - 1)
            {
                x = 0;
                y++;
            }
            if (x > inventorySize.x || y > inventorySize.y)
                break;
        }
    }

    public void SwitchDisplaying()
    {
        ClearSelection();
        background.gameObject.SetActive(!inventoryDisplayed);
        items.gameObject.SetActive(!inventoryDisplayed);
        inventoryDisplayed = !inventoryDisplayed;
        if (inventoryDisplayed)
            RefreshItems();
        else
            for (int i = 0; i < items.childCount; i++)
                Destroy(items.GetChild(i).gameObject);

        removeItemButton.gameObject.SetActive(false);
    }

    public void RemoveItemButtonChangeItem(UnityEngine.Events.UnityAction action)
    {
        removeItemButton.gameObject.SetActive(true);
        removeItemButton.onClick.RemoveAllListeners();
        removeItemButton.onClick.AddListener(action);
        removeItemButton.onClick.AddListener(() => { removeItemButton.gameObject.SetActive(false); });
    }

    public void ClearSelection()
    {
        foreach(Transform t in inventorySlots)
        {
            t.gameObject.GetComponent<Image>().color = ItemUI.defaultSlotColor;
        }
    }
}
