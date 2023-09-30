using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System.Linq;

public class DataManager : MonoBehaviour
{
    Player player;
    PlayerData playerData = null;
    string dataPath;

    private void Start()
    {
        dataPath = Path.Combine(Application.persistentDataPath, "PlayerData.xml");
        player = GameManager.instance.GetPlayer();
        
        playerData = LoadPlayerData();
        if (playerData == null || playerData.health <= 0)
        {
            playerData = new PlayerData();
            player.InitNewData(playerData);
        }
        else
        {
            player.LoadData(playerData);
        }

        player.OnPlayerDied += () => { playerData.health = 0; };

    }

    private void FixedUpdate()
    {
        if (player == null)
            return;
        playerData.position = player.gameObject.transform.position;
        playerData.health = player.health;
    }

    private void OnApplicationQuit()
    {
        SavePlayerData();
    }

    void SavePlayerData()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
        using (FileStream stream = new FileStream(dataPath, FileMode.Create))
        {
            serializer.Serialize(stream, playerData);
        }
    }

    PlayerData LoadPlayerData()
    {
        if (File.Exists(dataPath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
            using (FileStream stream = new FileStream(dataPath, FileMode.Open))
            {
                return (PlayerData)serializer.Deserialize(stream);
            }
        }
        else
        {
            Debug.LogWarning("Player data file not found.");
            return null;
        }
    }
}
