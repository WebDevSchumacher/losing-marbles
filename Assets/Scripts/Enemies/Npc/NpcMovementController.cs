using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovementController : MonoBehaviour
{
    private GameObject player;
    private Rigidbody body;
    public float accelerationFactor = 10;
    private bool hit;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        body = GetComponent<Rigidbody>();
        hit = false;
    }

    void Update()
    {
        if (!hit)
        {
            Vector3 pos = player.transform.position;
            transform.LookAt(pos);
            transform.position = Vector3.MoveTowards(transform.position, pos, 2 * Time.deltaTime);
        }
    }
}
