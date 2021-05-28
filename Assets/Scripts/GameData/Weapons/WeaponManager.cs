using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<GameObject> go_Weapons;

    public GameObject go_CurrentWeapon;

    public Transform weaponSlot;

    public Transform playerTransform;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EquipWeapon(GameObject weapon) {
        // Check if we already is carrying a weapon
        if (go_CurrentWeapon != null) {
            // hide the weapon                
            go_CurrentWeapon.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        // Add our new weapon
        go_CurrentWeapon = weapon;

        // Show our new weapon
        go_CurrentWeapon.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        go_CurrentWeapon.gameObject.GetComponent<Weapon>().bCanShoot = true;
    }

    void SelectWeapon(int index) {

        // Ensure we have a weapon in the wanted 'slot'
        if (go_Weapons.Count > index && go_Weapons[index] != null) {

            // Check if we already is carrying a weapon
            if (go_CurrentWeapon != null) {
                // hide the weapon                
                go_CurrentWeapon.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }

            // Add our new weapon
            go_CurrentWeapon = go_Weapons[index];

            // Show our new weapon
            go_CurrentWeapon.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void PickUpWeapon(GameObject weapon) {
        for(int i = 0; i < go_Weapons.Count; i++) {
            if(weapon.name != go_Weapons[i].name) {
                go_Weapons.Add(weapon);
                weapon.transform.parent = playerTransform;
                weapon.transform.position = weaponSlot.position;
            }
		}
    }

    public void DropWeapon(GameObject weapon) {
        go_Weapons.Remove(weapon);
        go_CurrentWeapon = null;
    }
}
