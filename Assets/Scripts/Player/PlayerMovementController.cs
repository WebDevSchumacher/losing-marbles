using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementEvent : UnityEvent<string>
{
};

public class PlayerMovementController : MonoBehaviour
{
    public float accelerationFactor = 200;
    public float jumpForceFactor = 200;
    private PlayerHelper helper;
    public MovementEvent moveEvent;
    bool canSteer;
    private Rigidbody body;
    public bool inArena;
    public float turnSensitivity = 2.0f;
    private GameObject cam;
    private CameraController camController;
    public GameObject equipment;
    bool canTurn;

    void Awake()
    {
        equipment = GameObject.Find("Equipment");
        cam = GameObject.Find("Main Camera");
        camController = cam.GetComponent<CameraController>();
        body = GetComponent<Rigidbody>();
        helper = GetComponent<PlayerHelper>();
        moveEvent = new MovementEvent();
        canSteer = true;
        canTurn = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().holdGame.AddListener(Stop);
    }

    void Update()
    {
        Vector3 dir = transform.position - cam.transform.position;
        dir.y = 0;
        dir = Vector3.Normalize(dir);
        if (helper.IsOnGround(.01f) && helper.CanMove())
        {
            float xForce = accelerationFactor * Input.GetAxis("Horizontal") * Time.deltaTime;
            float zForce = accelerationFactor * Input.GetAxis("Vertical") * Time.deltaTime;
            if (Input.GetKeyDown("space"))
            {
                body.AddForce(0, jumpForceFactor, 0);
                moveEvent.Invoke("jump");
            }

            if (canSteer)
            {
                body.AddForce(Vector3.Normalize(dir) * zForce);
                body.AddForce(Vector3.Cross(dir, Vector3.down) * xForce);

                if (xForce != 0)
                {
                    moveEvent.Invoke("move");
                }

                if (zForce != 0)
                {
                    moveEvent.Invoke("move");
                }
            }
        }

        if (inArena && canTurn && Input.GetAxis("Mouse X") != 0)
        {
            GameObject container = new GameObject("turnContainer");
            container.transform.position = body.transform.position;
            body.transform.parent = container.transform;
            equipment.transform.parent = container.transform;
            container.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * turnSensitivity, 0));
            container.transform.DetachChildren();
            Destroy(container);
        }
    }

    public void Stop()
    {
        helper.Movable(false);
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.constraints = RigidbodyConstraints.FreezePosition;
    }

    public void ToogleSteering()
    {
        canSteer = !canSteer;
        body.angularDrag = body.angularDrag > 0 ? 0 : 1;
    }

    public void ToggleTurning()
    {
        canTurn = !canTurn;
        camController.ToggleTurning();
    }
}