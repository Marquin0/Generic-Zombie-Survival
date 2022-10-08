using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private List<Weapon> weapons = new List<Weapon>();
    public List<Weapon> InitWeapons = new List<Weapon>();

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Weapon weapon in InitWeapons)
        {
            var newWeapon = Instantiate(weapon);
            newWeapon.Owner = gameObject;
            weapons.Add(newWeapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.Update();
        }
    }
}
