using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<GameObject> inventory;
    private GameObject leftHand;
    private GameObject rightHand;
    private GameObject back;
    private GameObject player;
    public GameObject equipment;
    public GameObject item;
    void Awake()
    {
        player = GameObject.Find("Player");
        inventory = new List<GameObject>();
        leftHand = GameObject.Find("LeftHand");
        rightHand = GameObject.Find("RightHand");
        back = GameObject.Find("Back");
    }

    public void Add(GameObject obj)
    {
        switch (obj.name)
        {
            case "KnifePickup":
                obj.transform.position = leftHand.transform.position;
                obj.transform.parent = leftHand.transform;
                obj.transform.localEulerAngles = new Vector3(90,0,0);
                Destroy(obj.GetComponent<SphereCollider>());
                break;
            case "GunPickup":
                obj.transform.position = rightHand.transform.position;
                obj.transform.parent = rightHand.transform;
                // obj.transform.rotation = Quaternion.identity;
                obj.transform.rotation = rightHand.transform.parent.rotation;
                Destroy(obj.GetComponent<SphereCollider>());
                break;
            case "JumppackPickup":
                obj.transform.position = back.transform.position;
                obj.transform.parent = back.transform;
                obj.transform.rotation = Quaternion.identity;
                Destroy(obj.GetComponent<SphereCollider>());
                break;
        }
        inventory.Add(obj);
    }
}
