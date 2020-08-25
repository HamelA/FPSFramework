using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reload : Ammo
{
    //Reload stats
    [Header("Reload")]
    [Tooltip("Time it takes to reload")]
    [SerializeField] private float reloadTime = 2f;
    [Tooltip("Amount of ammo reloaded into the magazine at one time")]
    [SerializeField] private int reloadAmount = 20;

    [Header("Ammo Modifiers")]
    [Tooltip("Whether the magazine drops unused ammo when reloaded")]
    [SerializeField] private bool dropMagazine = false;
    [Tooltip("Whether the gun has infinite ammo or not. Still requires reload.")]
    [SerializeField] protected bool infiniteAmmo = false;

    [Header("References")]
    public Text text;

    private bool isReloading = false;

    private GunSystem gs;

    private Coroutine reload;

    // Start is called before the first frame update
    void Start()
    {
        gs = GetComponent<GunSystem>();

        SetupInitialValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentMag < magazineSize && !isReloading)
        {
            reload = StartCoroutine(RefillMagazine());
        }

        /*if (isReloading && gs.isShooting || isMelee || isjumping || isSprinting || isThrowingGrenade)
        {
            Debug.Log("Stopping coroutine: Reload");
            StopCoroutine(reload);
            isReloading = false;
        }*/

        TextManagement();
    }

    IEnumerator RefillMagazine()
    {
        Debug.Log("Starting coroutine: Reload");

        isReloading = true;
        bool calculated = false;

        if (infiniteAmmo)
        {
            yield return new WaitForSeconds(reloadTime);
            currentMag = magazineSize;
        }
        else
        {
            while (currentMag < magazineSize && ammoHeld > 0)
            {
                yield return new WaitForSeconds(reloadTime);

                if (dropMagazine && !calculated)
                {
                    currentMag = 0;
                    calculated = true;
                }

                ammoHeld -= reloadAmount;

                if (ammoHeld < 0)
                {
                    currentMag += reloadAmount - Math.Abs(ammoHeld);
                    ammoHeld = 0;
                }
                else
                {
                    currentMag += reloadAmount;
                }

                if (currentMag > magazineSize)
                {
                    ammoHeld += (currentMag - magazineSize);
                    currentMag = magazineSize;
                    if (ammoHeld > totalAmmo) ammoHeld = totalAmmo;
                }
            }
        }

        isReloading = false;
        Debug.Log("Ended successfully");
    }

    protected void TextManagement()
    {
        text.text = currentMag + " / " + ammoHeld;
    }
}
