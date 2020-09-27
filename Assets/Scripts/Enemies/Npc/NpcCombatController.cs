using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcCombatController : MonoBehaviour
{
    public UnityEvent hit;

    void Awake()
    {
        hit = new UnityEvent();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject player = other.gameObject;
            player.GetComponent<PlayerHitController>().hit.Invoke();
            Rigidbody body = player.GetComponent<Rigidbody>();
            body.AddExplosionForce(400, transform.position, 20f, 0.5f);
        }
    }
}