using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishController : MonoBehaviour
{
    public UnityEvent finishEvent;
    void Awake()
    {
        finishEvent = new UnityEvent();
    }
    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player"){
            finishEvent.Invoke();
        }
    }
}
