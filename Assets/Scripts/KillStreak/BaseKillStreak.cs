using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseKillStreak : MonoBehaviour
{
    public float duration;
    public float damage;
    public float fireRate;

    public abstract void Fire(Vector2 mousePosition);
}
