using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponScriptableObject : ScriptableObject
{
    public int weaponDamage;
    public float weaponRange;
    public float fireRate;
    public Sprite weaponSprite;

}
