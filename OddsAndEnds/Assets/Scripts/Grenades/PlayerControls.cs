using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject grenade;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(grenade, cam.transform.position + cam.transform.forward, cam.transform.rotation);
        }
    }
}
