﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool free;
    Vector3 rotation;
    float distanceToFocal = 10.0f;

    public float moveSensitivity = 4.0f;
    public float perspectiveChangeSpeed = 5.0f;
    Quaternion initialRotation;

    void Start()
    {
        free = false;
        transform.LookAt(transform.parent.position);
        initialRotation = transform.parent.rotation;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C)){
            if(free){
                free = false;
                transform.parent.rotation = initialRotation;
            } else {
                free = true;
            }
        }
    }

    void LateUpdate()
    {
        if(free){
            if(Input.GetAxis("Mouse X") != 0){
                rotation.x += Input.GetAxis("Mouse X") * moveSensitivity;
                rotation.y = Mathf.Clamp(rotation.y, 0.0f, 90.0f);
            }
            Quaternion angle = Quaternion.Euler(rotation.y, rotation.x, 0);
            transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, angle, perspectiveChangeSpeed * Time.deltaTime);
            
        }
    }
}