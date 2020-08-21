using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : Ammo
{
    //Reload stats
    [Header("Reload")]
    [Tooltip("Time it takes to reload")]
    [SerializeField] private float reloadTime = 2f;
    [Tooltip("Amount of ammo reloaded into the magazine at one time")]
    [SerializeField] private int reloadAmount = 20;

    private GunSystem gs;
    [HideInInspector] public bool reloading;

    // Start is called before the first frame update
    void Start()
    {
        gs = GetComponent<GunSystem>();
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
