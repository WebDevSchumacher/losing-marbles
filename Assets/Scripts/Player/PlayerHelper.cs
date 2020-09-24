using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelper : MonoBehaviour
{
    private bool canMove;
    void Awake()
    {
        canMove = true;
    }
    public bool IsOnGround(float threshold) {
        SphereCollider collider = GetComponent<SphereCollider>();
        return Physics.Raycast(collider.bounds.center, Vector3.down, collider.bounds.extents.y + threshold);
    }

    public void ResetPosition(){
        transform.position = new Vector3(0, .15f, 0);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public bool CanMove(){
        return canMove;
    }

    public void Movable(bool isMovable){
        canMove = isMovable;
    }
}
