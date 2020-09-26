using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupControllerAmmo : PickupControllerBase
{
    public int amount;

    public override void Pickup(){
        GameObject.Find("GameManager").GetComponent<GameManager>().AddAmmo(amount);
        Destroy(gameObject);
    }
}
