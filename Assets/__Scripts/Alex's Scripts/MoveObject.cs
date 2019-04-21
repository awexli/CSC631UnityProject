﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{

    Vector3 objectPos;
    public float distance;

    public GameObject item;
    public GameObject tempParent;
    public bool isHolding = false;

    // Start is called before the first frame update
    void Start()
    {
        item.GetComponent<Rigidbody>().useGravity = true;
    }

    void Update()
    {
        distance = Vector3.Distance(item.transform.position, tempParent.transform.position);
        
        if (distance >= 1f)
        {
            isHolding = false;
        }

        if (isHolding == true)
        {
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);
        }
        else
        {
            objectPos = item.transform.position;
            item.transform.SetParent(null);
            item.GetComponent<Rigidbody>().useGravity = true;
            item.transform.position = objectPos;
        }
    }

    void OnMouseDown()
    {
        if (distance <= 1f)
        {
            isHolding = true;
            item.GetComponent<Rigidbody>().useGravity = false;
            item.GetComponent<Rigidbody>().detectCollisions = true;
        }
    }

    void OnMouseUp()
    {
        isHolding = false;
    }
}
