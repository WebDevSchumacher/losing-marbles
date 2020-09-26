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
        hit = true;
    }

    void Update()
    {
        
        if (!hit)
        {
            
        Vector3 pos = player.transform.position;
        // Vector3 pos = Vector3.Reflect(player.transform.position, player.transform.position);
        transform.LookAt(pos);
        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime);
        // float xForce = accelerationFactor * Time.deltaTime;
        // float zForce = accelerationFactor * Time.deltaTime;
        // Vector3 norm = pos.normalized;
        // Vector3 force = new Vector3(norm.x*xForce, 0, norm.z*zForce);
        // body.AddForce(force);
        }
        
    }
    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("hit!");
            hit = true;
        }
    }
}
