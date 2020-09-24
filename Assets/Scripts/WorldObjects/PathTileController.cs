using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTileController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(this.GetInstanceID() + " created");
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        // Debug.Log(other.gameObject.GetInstanceID() + " collided with " + this.GetInstanceID());
    }
}
