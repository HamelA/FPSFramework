using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapons : GunSystem2
{
    [Header("Projectile")]
    [Tooltip("Prefab of the projectile")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootForce;
    [SerializeField] private float upwardForce;

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    protected override void ShotType(float x, float y, float z)
    {
        //Find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player

        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, z); //Just add spread to last direction

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(projectile, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        //Graphics
        //Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));          //SHOULD BE HANDLED ON BULLET PREFAB
    }
}
