using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    float cookTime;     //How long it takes to go off
    float maxDamage;    //Max amount of damage grenade can cause
    float aoe;          //Sets radius that can damages players
    float thrust;       //Force applied when thrown

    // Start is called before the first frame update
    void Start()
    {
        //thrust = 100;
        //this.GetComponent<Rigidbody>().AddForce(transform.forward * thrust);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
