using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{

    #region "Variables"

    //General gun stats
    [Header("General")]
    [Tooltip("Prevents multiple bullets in a single input from consuming more than 1 unit of ammo, e.g. If set to 2, for every 2 bullets only 1 ammo is consumed")]
    [SerializeField] private bool multiBulletsPerShot = false;
    [Tooltip("Maximum ammo the gun can hold")]
    [SerializeField] private int totalAmmo = 200;
    [Tooltip("Amount of bullets that can be shot before needing to be reloaded")]
    [SerializeField] private int magazineSize = 20;

    [Tooltip("Time it takes to reload")]
    [SerializeField] private float reloadTime = 2f;
    [Tooltip("Amount of ammo reloaded into the magazine at one time.")]
    [SerializeField] private int reloadAmount = 20;

    [Tooltip("Whether the magazine drops unused ammo when reloaded")]
    [SerializeField] private bool dropMagazine = false;
    [Tooltip("Whether the gun has infinite ammo or not")]
    [SerializeField] private bool infiniteAmmo = false;
    [Tooltip("Whether the magazine needs to be reloaded or not")]
    [SerializeField] private bool bottomlessClip = false;

    private int currentMag, ammoHeld;
    private bool isReloading = false;

    public Text text;

    private Coroutine reload;

    private GunSystem gs;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentMag = magazineSize;
        ammoHeld = totalAmmo;

        if (GetComponent<GunSystem>()) gs = GetComponent<GunSystem>();
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
}
