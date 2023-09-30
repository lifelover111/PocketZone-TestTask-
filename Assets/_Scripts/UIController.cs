using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] Transform shootButton;
    [SerializeField] Transform inventoryButton;
    void Start()
    {
        GameManager.instance.GetPlayer().OnPlayerDied += () => { 
            shootButton.gameObject.SetActive(false);  
            inventoryButton.gameObject.SetActive(false);
        };
    }
}
