using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ammo : MonoBehaviour
{

    #region "Variables"

    //General gun stats
    [Header("General")]
    [Tooltip("Prevents multiple bullets in a single input from consuming more than 1 unit of ammo, e.g. If set to 2, for every 2 bullets only 1 ammo is consumed")]
    [SerializeField] private bool multiBulletsPerShot = false;
    [Tooltip("Maximum ammo the gun can hold")]
    [SerializeField] protected int totalAmmo = 200;
    [Tooltip("Amount of bullets that can be shot before needing to be reloaded")]
    [SerializeField] protected int magazineSize = 20;

    [Tooltip("Whether the gun has infinite ammo or not")]
    [SerializeField] protected bool infiniteAmmo = false;

    protected int currentMag, ammoHeld;
    protected bool isReloading = false;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentMag = magazineSize;
        ammoHeld = totalAmmo;
    }

    protected void DecreaseAmmo()
    {
        currentMag--;
    }
}
