using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown : Ammo
{

    //Cooldown stats
    [Header("Cooldown")]
    [Tooltip("Total heat units the gun can handle at one time")]
    [SerializeField] private float maxHeat = 10f;
    [Tooltip("How many heat units each bullet creates")]
    [SerializeField] private float heatPerBullet = 0.5f;
    [Tooltip("How many heat units dissipate per second")]
    [SerializeField] private float heatDissipationRate = 2f;
    [Tooltip("How long overheating lasts")]
    [SerializeField] private float overheatTime = 5f;
    [Tooltip("Determines whether the gun maintains its overheated state or if it lets the gun cooldown to 0 heat units")]
    [SerializeField] private bool retainHeat = false;

    [Header("References")]
    public Text ammoText;
    public Text cooldownText;

    private float currentHeat;
    private bool overheating, isRunning;

    private float overheatCounter;

    private GunSystem2 gs2;

    private void Start()
    {
        SetupInitialValues();

        currentHeat = 0f;
        overheatCounter = 0f;
        overheating = false;
        isRunning = false;

        gs2 = GetComponent<GunSystem2>();
    }

    private void Update()
    {
        
        if (overheating && !isRunning)
        {
            StartCoroutine(OverheatWait());
        }
        else if (!overheating || !retainHeat)
        {
            DecreaseHeat();
        }

        TextManagement();
    }

    protected void TextManagement()
    {
        ammoText.text = currentMag + " / " + ammoHeld;
        cooldownText.text = currentHeat + " / " + maxHeat;
    }

    IEnumerator OverheatWait()
    {
        isRunning = true;
        gs2.InterruptResetShot();
        gs2.SetReadyToShoot(false);

        while (overheatTime > overheatCounter)
        {
            overheatCounter += Time.deltaTime;

            yield return null;
        }

        overheatCounter = 0;
        overheating = false;

        gs2.SetReadyToShoot(true);
        isRunning = false;
    }

    public void IncreaseHeat()
    {
        currentHeat += heatPerBullet;
        if (currentHeat >= maxHeat)
        {
            currentHeat = maxHeat;
            overheating = true;
        }
    }

    private void DecreaseHeat()
    {
        if (currentHeat > 0) currentHeat -= (Time.deltaTime * heatDissipationRate);
        else if (currentHeat < 0) currentHeat = 0f;
    }

    public float CurrentHeatPercentage()
    {
        float heatPercentage = currentHeat / maxHeat;
        return heatPercentage;
    }

    public bool GetOverheating()
    {
        return overheating;
    }
}
