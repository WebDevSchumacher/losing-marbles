using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHitController : MonoBehaviour
{
    public UnityEvent hit;

    void Awake()
    {
        hit = new UnityEvent();
    }
}
