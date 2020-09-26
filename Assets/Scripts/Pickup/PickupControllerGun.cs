using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupControllerGun : PickupControllerBase
{
    GameObject player;
    private bool attached;
    private Transform hand;
    public GameObject bulletObj;

    public override void Pickup()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().AttachObjectToPlayer(gameObject);
        attached = true;
        GetComponent<PickupRotate>().active = false;
        hand = transform.parent;
        transform.Rotate(Vector3.zero);
    }

    private void Start()
    {
        player = GameObject.Find("Player");
        attached = false;
    }

    private void Update()
    {
        if (attached && Input.GetMouseButtonDown(1))
        {
            FireBullet();
        }
    }

    void FireBullet()
    {
        if (!bulletObj)
        {
            return;
        }
        GameObject bullet = Instantiate(bulletObj, transform.position, Quaternion.Euler(transform.forward));
        bullet.GetComponent<BulletController>().FireBullet(gameObject, transform.forward);
    }
}