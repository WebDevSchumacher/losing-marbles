using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AoeEvent : UnityEvent<string>
{
};

public class AoeController : MonoBehaviour
{
    public AoeEvent touchEvent;
    public string aoeType;

    void Awake()
    {
        touchEvent = new AoeEvent();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            touchEvent.Invoke(aoeType);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            touchEvent.Invoke(aoeType);
        }
    }
}