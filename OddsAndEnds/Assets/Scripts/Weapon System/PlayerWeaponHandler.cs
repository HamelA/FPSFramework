using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    //General gun stats
    [Header("General")]
    [Tooltip("How many guns the player can hold at a time")]
    [SerializeField] private int maxGuns = 2;

    private Transform weaponHolder;
    private GunSystem2[] gunList;
    private List<GunSystem2> weaponList;
    private int currentWeapon;


    // Start is called before the first frame update
    void Start()
    {
        weaponHolder = transform.GetChild(0).GetChild(0);
        gunList = new GunSystem2[maxGuns];

        InitWeaponList();

        currentWeapon = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (gunList[currentWeapon].GetIsAutomatic())
            gunList[currentWeapon].SetShooting(Input.GetKey(KeyCode.Mouse0));
        else
            gunList[currentWeapon].SetShooting(Input.GetKeyDown(KeyCode.Mouse0));
    }

    private void InitWeaponList()
    {
        //for (int i = 0; i < weaponHolder.childCount; i++)
            //weaponList.Add(weaponHolder.GetChild(i).gameObject.GetComponent<GunSystem2>());
        gunList = weaponHolder.GetComponentsInChildren<GunSystem2>(true);
    }
}
