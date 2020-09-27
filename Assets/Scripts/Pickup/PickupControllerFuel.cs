using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupControllerFuel : PickupControllerBase
{
    public float amount;

    public override void Pickup()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().Refuel(amount);
        Destroy(gameObject);
    }
}