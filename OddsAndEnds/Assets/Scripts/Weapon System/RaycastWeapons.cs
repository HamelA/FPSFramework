using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapons : GunSystem2
{
    //Gun stats
    [Header("Raycast")]
    [Tooltip("Damage each shot deals to the target")]
    [SerializeField] private int damage;
    [Tooltip("How far the shot can hit")]
    [SerializeField] private float range = 100f;
    [Tooltip("Identifies what can be affected by the shot")]
    [SerializeField] private LayerMask whatIsEnemy;
    [Tooltip("Decal left where the shot landed")]
    [SerializeField] private GameObject bulletHoleGraphic;

    //Reference
    public RaycastHit rayHit;

    // Update is called once per frame
    void Update()
    {
        TriggerPull();
    }

    protected override void ShotType(float x, float y, float z)
    {
        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, z);

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            //Debug.Log(rayHit.collider.name);

            //if (rayHit.collider.CompareTag("Enemy"))
            //    rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
        }

        //Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.FromToRotation(Vector3.forward, rayHit.normal));
    }
}
