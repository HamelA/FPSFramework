using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GunSystem : MonoBehaviour
{
    #region "Variables"

    //General gun stats
    [Header("General")]
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
    [Tooltip("Amount of bullets that can be shot before needing to be reloaded")]
    [SerializeField] private int magazineSize = 20;

    //Reload stats
    [Header("Reload")]
    [Tooltip("Time it takes to reload")]
    [SerializeField] private float reloadTime = 2f;
    [Tooltip("Amount of ammo reloaded into the magazine at one time.")]
    [SerializeField] private int reloadAmount = 20;

    //References to other objects
    [Header("References")]
    [Tooltip("Camera that the player is using")]
    [SerializeField] protected Camera fpsCam;
    [Tooltip("The point on the gun where the bullets are shot out of")]
    [SerializeField] protected Transform attackPoint;

    //Graphics
    [Tooltip("Effect that occurs on the muzzle when a bullet is shot")]
    [SerializeField] private GameObject muzzleFlash;

    [Tooltip("Text object to display ammo in magazine and ammo total")]
    [SerializeField] protected Text text;

    //Camera shake effect
    [Header("Camera Shake")]
    //public CamShake camShake;
    [Tooltip("Determines how sever the shaking is")]
    [SerializeField] private float camShakeMagnitude;
    [Tooltip("Determines how long the shaking lasts")]
    [SerializeField] private float camShakeDuration;

    //Gun stats
    int bulletsLeft, bulletsShot;

    //bools 
    bool shooting, readyToShoot, reloading;

    public bool isShooting;

    #endregion

    private void Awake()
    {
        bulletsLeft = magazineSize;     //Sets current magazine to full
        readyToShoot = true;
    }

    private void Update()
    {
        Debug.Log("Ready to shoot: " + readyToShoot);
        isShooting = readyToShoot;
    }

    protected void MyInput()
    {
        if (Input.GetKey(KeyCode.Mouse0)) shooting = true;

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }
        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            if (GetComponent<AttackCharge>())
            {
                if (GetComponent<AttackCharge>().ChargeShot())
                {
                    bulletsShot = bulletsPerTap;
                    Shoot();
                }
            }
            else
            {
                bulletsShot = bulletsPerTap;
                Shoot();
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)) shooting = false;
    }

    protected void TextManagement()
    {
        text.text = bulletsLeft + " / " + magazineSize;
    }

    #region "Shooting Logic"

    private void Shoot()
    {
        readyToShoot = false;

        StartCoroutine(ShotBurst());

        StartCoroutine(ResetShot());
    }

    IEnumerator ShotBurst()
    {
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

            //Debug.Log("X: " + x + "    Y:" + y);

            //Bullets handled by child class
            ShotType(x, y, z);

            //ShakeCamera
            //camShake.Shake(camShakeDuration, camShakeMagnitude);

            //Graphics
            //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

            if (!multiBulletsPerShot) bulletsLeft--;
            bulletsShot--;

            if (!multiBulletsPerShot && GetComponent<Cooldown>()) GetComponent<Cooldown>().IncreaseHeat();

        } while (bulletsShot > 0 && bulletsLeft > 0);

        if (multiBulletsPerShot) bulletsLeft--;
        if (multiBulletsPerShot && GetComponent<Cooldown>()) GetComponent<Cooldown>().IncreaseHeat();
    }

    protected abstract void ShotType(float x, float y, float z);

    IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(timeBetweenBursts + (timeBetweenBullets * bulletsPerTap));
        readyToShoot = true;
    }

    #endregion

    #region "Reload Logic"

    private void Reload()
    {
        reloading = true;
        StartCoroutine(ReloadFinished());
    }
    IEnumerator ReloadFinished()
    {
        do {
            yield return new WaitForSeconds(reloadTime);
            bulletsLeft += reloadAmount;
            if (bulletsLeft > magazineSize) bulletsLeft = magazineSize;     //Ensures the loop comes to an end
        } while (bulletsLeft != magazineSize);

        reloading = false;
    }

    #endregion

    public void SetReadyToShoot(bool value)
    {
        readyToShoot = value;
        Debug.Log(readyToShoot);
    }

}
