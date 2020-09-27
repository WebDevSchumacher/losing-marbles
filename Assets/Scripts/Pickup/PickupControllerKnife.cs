using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupControllerKnife : PickupControllerBase
{
    GameObject player;
    private bool attached;
    private Transform hand;
    private Transform equipment;
    private Vector3 thrustAngle;
    private bool thrusting;
    private bool drawingBack;
    private Quaternion thrustStart;
    public int thrustSpeed = 700;

    public override void Pickup()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().AttachObjectToPlayer(gameObject);
        attached = true;
        thrusting = false;
        drawingBack = false;
        GetComponent<PickupRotate>().active = false;
        hand = transform.parent;
        equipment = hand.parent;
        thrustAngle = new Vector3(0, 50, 0);
    }

    private void Start()
    {
        player = GameObject.Find("Player");
        attached = false;
    }

    private void Update()
    {
        if (attached)
        {
            if (Input.GetMouseButtonDown(0) && !drawingBack)
            {
                thrusting = true;
                thrustStart = equipment.transform.rotation;
                player.GetComponent<PlayerMovementController>().ToggleTurning();
            }

            if (Input.GetMouseButtonUp(0))
            {
                thrusting = false;
                drawingBack = true;
            }

            if (drawingBack && equipment.transform.rotation.Equals(thrustStart))
            {
                drawingBack = false;
                player.GetComponent<PlayerMovementController>().ToggleTurning();
            }

            if (thrusting)
            {
                equipment.transform.rotation = Quaternion.RotateTowards(equipment.transform.rotation,
                    Quaternion.Euler(thrustStart.eulerAngles + thrustAngle), thrustSpeed * Time.deltaTime);
            }

            if (drawingBack)
            {
                equipment.transform.rotation = Quaternion.RotateTowards(equipment.transform.rotation,
                    thrustStart, thrustSpeed * Time.deltaTime);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (thrusting)
        {
            if (other.gameObject.CompareTag("Npc"))
            {
                GameObject npc = other.gameObject;
                npc.GetComponent<NpcCombatController>().hit.Invoke();
                Destroy(npc);
                GameObject.Find("GameManager").GetComponent<GameManager>().hitEvent.Invoke(gameObject);
            }
        }
    }
}