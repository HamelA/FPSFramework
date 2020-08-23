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
    [Tooltip("Whether the magazine needs to be reloaded or not")]
    [SerializeField] private bool bottomlessClip = false;

    private GunSystem gs;
    [HideInInspector] public bool reloading;

    private Text text;

    private Coroutine reload;

    // Start is called before the first frame update
    void Start()
    {
        gs = GetComponent<GunSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        {
            text.text = currentMag + " / " + magazineSize + " / " + ammoHeld;
        }

        if (Input.GetKeyDown(KeyCode.R) && currentMag < magazineSize && !isReloading)
        {
            reload = StartCoroutine(RefillMagazine());
        }

        /*if (isReloading && gs.isShooting  || isMelee || isjumping || isThrowingGrenade)
        {
            Debug.Log("Stopping coroutine: Reload");
            StopCoroutine(reload);
            isReloading = false;
        }*/
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

            //SET TEXT HERE
            text.text = currentMag + " / " + magazineSize + " / " + ammoHeld;

            Debug.Log(currentMag);
        }

        isReloading = false;
        Debug.Log("Ended successfully");
    }






    /*public void Reloading()
    {
        gs.SetReadyToShoot(false);
        reloading = true;
        StartCoroutine(ReloadFinished());
    }
    IEnumerator ReloadFinished()
    {
        do
        {
            yield return new WaitForSeconds(reloadTime);
            gs.SetBulletsLeft(gs.GetBulletsLeft() + reloadAmount);
            if (gs.GetBulletsLeft() > gs.GetMagazineSize()) gs.SetBulletsLeft(gs.GetMagazineSize());     //Ensures the loop comes to an end
        } while (gs.GetBulletsLeft() != gs.GetMagazineSize());

        reloading = false;
        gs.SetReadyToShoot(true);
    }*/
}
