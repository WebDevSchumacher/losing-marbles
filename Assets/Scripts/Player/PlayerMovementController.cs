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
    private Rigidbody body;
    public Vector3 viewDirection;

    public Vector3 direction;
    void Awake()
    {
        viewDirection = Vector3.one;
        body = GetComponent<Rigidbody>();
        helper = GetComponent<PlayerHelper>();
        moveEvent = new MovementEvent();
        canSteer = true;
    }

    void Update()
    {
        if(helper.IsOnGround(.01f) && helper.CanMove()){
            float xForce = accelerationFactor * Input.GetAxis("Horizontal") * Time.deltaTime;
            float zForce = accelerationFactor * Input.GetAxis("Vertical") * Time.deltaTime;
            Vector3 norm = viewDirection.normalized;
            Vector3 force = new Vector3(norm.x*xForce, 0, norm.z*zForce);
            if (Input.GetKeyDown("space"))
            {
                body.AddForce(0, jumpForceFactor, 0);
                moveEvent.Invoke("jump");
            }
            if(canSteer){
                body.AddForce(force);
                // body.AddForce(xForce, 0, zForce);
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
