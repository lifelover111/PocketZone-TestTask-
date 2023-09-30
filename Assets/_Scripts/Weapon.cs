using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    float damage = 1;
    public float range = 3.5f;
    public void Shoot(Enemy enemy = null)
    {
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}
