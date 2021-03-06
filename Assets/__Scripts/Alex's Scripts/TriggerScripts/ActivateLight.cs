﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLight : MonoBehaviour
{
    public bool inZone = false;
    public bool ePressed = false;
    public EnableLight[] lights;
    Main manager;
    public AudioSource source;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("heartbeat").GetComponent<Main>();
    }


    
    void Update()
    {
        if (inZone == true)
        {
            if (Input.GetKeyDown("e"))
            {
                //// P1 Torch
                //lights[0].myLight.enabled = !lights[0].myLight.enabled;
                //// P1 WallLight
                //lights[1].myLight.enabled = !lights[1].myLight.enabled;

                //// P2 LightColors
                //lights[2].myLight.enabled = !lights[2].myLight.enabled;
                //// P2 WallLight
                //lights[3].myLight.enabled = !lights[3].myLight.enabled;
                manager.RequestLight(this.gameObject.tag);
            }
        }
    } 

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            inZone = true;
        }
    }

    void OnTriggerExit()
    {
        inZone = false;
    }
}
