using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float health { get; }
    float maxHealth { get; }

    void TakeDamage(float damage);
}
