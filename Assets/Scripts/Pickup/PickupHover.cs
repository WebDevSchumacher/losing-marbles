using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHover : MonoBehaviour
{
    Vector3 hover;
    float zenith;
    float nadir;
    public float range;

    void Start()
    {
        zenith = gameObject.transform.position.y * (1f + range);
        nadir = gameObject.transform.position.y * (1f - range);
        hover = new Vector3(0, .1f * Time.deltaTime, 0);
    }

    void Update()
    {
        gameObject.transform.position += hover;
        if(gameObject.transform.position.y > zenith || gameObject.transform.position.y < nadir){
            hover = -hover;
        }
    }
}
