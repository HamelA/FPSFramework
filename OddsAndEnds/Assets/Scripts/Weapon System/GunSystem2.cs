using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GunSystem2 : MonoBehaviour
{
    #region "Variables"

    //General gun stats
    [Header("General")]
    [Tooltip("Whether the gun allows the trigger to be held or not")]
    [SerializeField] private bool isAutomatic = false;
    [Tooltip("Time between bursts")]
    [SerializeField] private float timeBetweenBursts = 0.2f;
    [Tooltip("Time between each bullet in a burst")]
    [SerializeField] private float timeBetweenBullets = 0f;
    [Tooltip("Amount of bullets in each burst")]
    [SerializeField] private int bulletsPerTap = 1;
    [Tooltip("Prevents multiple bullets in a single input from consuming more than 1 unit of ammo, e.g. If set to 2, for every 2 bullets only 1 ammo is consumed")]
    [SerializeField] private bool multiBulletsPerShot = false;
    [Tooltip("Random shifts in the direction a bullet travels")]
    [SerializeField] private float spread = 0.01f;

    //References to other objects
    [Header("References")]
    [Tooltip("Camera that the player is using")]
    [SerializeField] protected Camera fpsCam;
    [Tooltip("The point on the gun where the bullets are shot out of")]
    [SerializeField] protected Transform attackPoint;

    //Graphics
    [Tooltip("Effect that occurs on the muzzle when a bullet is shot")]
    [SerializeField] private GameObject muzzleFlash;

    //Camera shake effect
    [Header("Camera Shake")]
    //public CamShake camShake;
    [Tooltip("Determines how sever the shaking is")]
    [SerializeField] private float camShakeMagnitude;
    [Tooltip("Determines how long the shaking lasts")]
    [SerializeField] private float camShakeDuration;

    //Gun stats
    int bulletsShot;

    //bools 
    bool shooting, readyToShoot, reloading;

    public bool isShooting;

    //Script References
    private Ammo ammo;
    private Reload reload;
    private Cooldown cooldown;
    private AttackCharge attackCharge;

    private Coroutine resetShot;

    #endregion

    private void Awake()
    {
        readyToShoot = true;
    }

    private void Start()
    {
        ammo = GetComponent<Ammo>();
        if (GetComponent<Reload>()) reload = GetComponent<Reload>();
        if (GetComponent<Cooldown>()) cooldown = GetComponent<Cooldown>();
        if (GetComponent<AttackCharge>()) attackCharge = GetComponent<AttackCharge>();
    }

    private void Update()
    {
        isShooting = !readyToShoot;
    }

    protected void TriggerPull()
    {
        //Shoot
        if (readyToShoot && shooting && /*!reloading &&*/  ammo.GetCurrentMag() > 0)
        {
            if (attackCharge)
            {
                if (attackCharge.ChargeShot())
                {
                    Shoot();
                }
            }
            else
            {
                Shoot();
            }
        }
    }

    #region "Shooting Logic"

    private void Shoot()
    {
        readyToShoot = false;

        StartCoroutine(ShotBurst());

        resetShot = StartCoroutine(ResetShot());
    }

    IEnumerator ShotBurst()
    {
        bulletsShot = bulletsPerTap;

        do
        {
            if (bulletsShot <= bulletsPerTap)
            {
                yield return new WaitForSeconds(timeBetweenBullets);
            }

            //Spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);
            float z = Random.Range(-spread, spread);

            //Bullets handled by child class
            ShotType(x, y, z);

            //ShakeCamera
            //camShake.Shake(camShakeDuration, camShakeMagnitude);

            //Graphics
            //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

            if (!multiBulletsPerShot)
            {
                ammo.DecreaseAmmo();
                if (cooldown) cooldown.IncreaseHeat();
            }
            bulletsShot--;

        } while (bulletsShot > 0 && ammo.GetCurrentMag() > 0);

        if (multiBulletsPerShot)
        {
            ammo.DecreaseAmmo();
            if (cooldown) cooldown.IncreaseHeat();
        }
    }

    protected abstract void ShotType(float x, float y, float z);

    IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(timeBetweenBursts + (timeBetweenBullets * bulletsPerTap));
        readyToShoot = true;
    }

    #endregion
    

    public void InterruptResetShot()
    {
        StopCoroutine(resetShot);
        readyToShoot = true;
    }

    public bool GetIsAutomatic()
    {
        return isAutomatic;
    }

    public void SetReadyToShoot(bool value)
    {
        readyToShoot = value;
    }

    public void SetShooting(bool value)
    {
        shooting = value;
    }

}
