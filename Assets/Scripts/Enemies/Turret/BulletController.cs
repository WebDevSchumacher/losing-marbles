﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    bool isFired;
    Vector3 aimDirection;
    public float range = 10;
    public int speed;

    void Update()
    {
        if (isFired)
        {
            Vector3 distance = speed * Time.deltaTime * aimDirection;
            transform.position += distance;
            range -= distance.magnitude;
            if (range <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void FireBullet(GameObject weapon, GameObject target)
    {
        if (weapon && target)
        {
            aimDirection = (target.transform.position - weapon.transform.position).normalized;
            isFired = true;
        }
    }

    public void FireBullet(GameObject weapon, Vector3 direction)
    {
        if (weapon && !direction.Equals(Vector3.zero))
        {
            transform.rotation = weapon.transform.rotation;
            aimDirection = direction;
            isFired = true;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject player = other.gameObject;
            player.GetComponent<PlayerHitController>().hit.Invoke();
            Destroy(gameObject);
        }
    }
}