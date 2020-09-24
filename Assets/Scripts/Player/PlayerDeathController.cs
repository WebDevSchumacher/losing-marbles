using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeathController : MonoBehaviour
{
    private PlayerHelper helper;
    GameManager gameManager;
    bool deathTriggered = false;

    void Start()
    {
        // initialize reference to helper script, providing commonly used functions for player management
        helper = GetComponent<PlayerHelper>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!helper.IsOnGround(5f) && transform.position.y < -5 && !deathTriggered){
            deathTriggered = true;
            gameManager.OnDeath();
        }
    }
}
