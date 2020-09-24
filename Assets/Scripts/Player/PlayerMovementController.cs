using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementEvent : UnityEvent<string>{};
public class PlayerMovementController : MonoBehaviour
{
    public float accelerationFactor = 200;
    public float jumpForceFactor = 200;
    private PlayerHelper helper;
    public MovementEvent moveEvent;
    bool canSteer;

    public Vector3 direction;
    void Awake()
    {
        helper = GetComponent<PlayerHelper>();
        moveEvent = new MovementEvent();
        canSteer = true;
    }

    void Update()
    {
        if(helper.IsOnGround(.01f) && helper.CanMove()){
            float xForce = accelerationFactor * Input.GetAxis("Horizontal") * Time.deltaTime;
            Debug.Log(transform.forward);
            float zForce = accelerationFactor * Input.GetAxis("Vertical") * Time.deltaTime;
            Rigidbody body = GetComponent<Rigidbody>();
            if (Input.GetKeyDown("space"))
            {
                body.AddForce(0, jumpForceFactor, 0);
                moveEvent.Invoke("jump");
            }
            if(canSteer){
                body.AddForce(xForce, 0, zForce);
                if(xForce != 0) {
                    moveEvent.Invoke("move");
                }
                if(zForce != 0) {
                    moveEvent.Invoke("move");
                }
            }
        }
    }

    public void Stop(){
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
    }

    public void ToogleSteering(){
        canSteer = !canSteer;
        GetComponent<Rigidbody>().angularDrag = GetComponent<Rigidbody>().angularDrag > 0 ? 0 : 1;
    }

    
}
