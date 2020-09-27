using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BrickSmash : MonoBehaviour
{
    Vector3 hover;
    Vector3 rise;
    Vector3 fall;
    float zenith;
    float nadir;
    bool active;

    void Start()
    {
        zenith = gameObject.GetComponent<Renderer>().bounds.size.y * 2;
        nadir = gameObject.GetComponent<Renderer>().bounds.size.y / 2;
        rise = new Vector3(0, 0.4f * Time.deltaTime, 0);
        fall = new Vector3(0, -4.0f * Time.deltaTime, 0);
        hover = rise;
        GameObject.Find("GameManager").GetComponent<GameManager>().holdGame.AddListener(Stop);
        active = true;
        WorldCreator.reload.AddListener(Start);
    }

    void Update()
    {
        if (active)
        {
            gameObject.transform.position += hover;
            if (gameObject.transform.position.y >= zenith)
            {
                hover = fall;
            }
            else if (gameObject.transform.position.y <= nadir)
            {
                hover = rise;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameManager.crushed = true;
            gameManager.OnDeath();
        }
    }

    void Stop()
    {
        active = false;
    }
}