using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRotate : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.Rotate(0, 10.0f, 0, Space.World);
    }
}
