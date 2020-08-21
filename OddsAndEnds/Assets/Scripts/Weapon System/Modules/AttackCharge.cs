using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCharge : MonoBehaviour
{
    [Header("Charge")]
    [Tooltip("If the weapon shoots once it passes the threshold or when the button is released")]
    [SerializeField] private bool onButtonRelease = false;
    [Tooltip("Time to charge weapon")]
    [SerializeField] private float chargeTime = 0f;
    [Tooltip("Minimum percentage the gun needs to fire")]
    [Range(0, 1)]
    [SerializeField] private float chargeThreshold = 0f;

    private float currentCharge = 0f;
    private float chargePercentage = 0f;         //Current charge percentage

    public bool ChargeShot()
    {
        currentCharge += Time.deltaTime;
        chargePercentage = currentCharge / chargeTime;

        if (onButtonRelease)
        {
            if (chargePercentage >= chargeThreshold && Input.GetKeyUp(KeyCode.Mouse0))
            {
                currentCharge = 0f;
                return true;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                currentCharge = 0f;
                return false;
            }
        }
        else
        {
            if (chargePercentage >= chargeThreshold)
            {
                currentCharge = 0f;
                return true;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                currentCharge = 0f;
                return false;
            }
        }
        return false;
    }

    public float GetChargePercentage()
    {
        return chargePercentage;
    }
}
