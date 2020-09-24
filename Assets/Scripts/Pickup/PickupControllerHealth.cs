using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupControllerHealth : PickupControllerBase
{
    public float amount;

    void Start()
    {
    }

    public override void Pickup(){
        GameObject.Find("GameManager").GetComponent<GameManager>().Heal(amount);
        Destroy(gameObject);
    }
}
