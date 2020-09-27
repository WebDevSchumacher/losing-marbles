using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRotate : MonoBehaviour
{
    public bool active;

    private void Start()
    {
        active = true;
    }

    void Update()
    {
        if (active)
        {
            gameObject.transform.Rotate(0, 10.0f, 0, Space.World);
        }
    }
}