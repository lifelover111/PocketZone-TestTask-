using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class PlayerData
{
    [XmlElement] public Vector2 position;
    [XmlElement] public float health;
    [XmlElement] public Vector2Int[] itemsInInventory_Id_Count;
}
