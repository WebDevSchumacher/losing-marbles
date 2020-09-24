using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupControllerBase : MonoBehaviour
{
    public abstract void Pickup();

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player"){
            Pickup();
        }
    }
}
