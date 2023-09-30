using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] Player player;
    [SerializeField] ItemDatabase itemDatabase;
    [SerializeField] List<GameObject> enemyPrefabs;
    public InventoryUI inventoryUI;
    public Bounds levelBounds;
    public static Transform enemyAnchor;
    public static Transform itemAnchor;

    private void Awake()
    {
        instance = this;
        itemDatabase.InitDictionary();
        enemyAnchor = new GameObject("Enemy Anchor").transform;
        itemAnchor = new GameObject("Item Anchor").transform;
        SpawnEnemies(3);
    }

    void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++) 
        {
            GameObject go = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)]);
            go.transform.position = new Vector3(
                Random.Range(levelBounds.min.x, levelBounds.max.x),
                Random.Range(levelBounds.min.y, levelBounds.max.y),
                0);
            go.transform.SetParent(enemyAnchor, true);
        }
    }

    public Player GetPlayer() 
    {
        return player;
    }

    public ItemDatabase GetItemDatabase()
    {
        return itemDatabase;
    }
}
