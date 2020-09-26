﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;

public class ValueChangedEvent : UnityEvent<string, float>
{
};

public class GameManager : MonoBehaviour
{
    public SceneController sceneController;
    private GameObject player;
    static int lives = 3;
    static float health = 100f;
    float fuel;
    private int ammo;
    private bool hasGun;
    public ValueChangedEvent valueChanged;
    public UnityEvent holdGame;
    static int totalDuration = 0;
    static float totalFuelSpent = 0;
    static int totalLivesLost = 0;

    private float dotDuration;

    DateTime startTime;
    

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        ammo = 0;
        hasGun = false;
        fuel = 1000f;
        dotDuration = 0.0f;
        valueChanged = new ValueChangedEvent();
        holdGame = new UnityEvent();
        sceneController = GetComponent<SceneController>();
    }

    void Start()
    {
        if (sceneController.CurrentScene() > 0 && sceneController.CurrentScene() < 4)
        {
            player.GetComponent<PlayerMovementController>().moveEvent.AddListener(Move);
            player.GetComponent<PlayerHitController>().hit.AddListener(Hit);
            GameObject.FindWithTag("Hud").transform.Find("MenuFinish").gameObject.SetActive(false);
            GameObject.FindWithTag("Hud").transform.Find("MenuOutOfFuel").gameObject.SetActive(false);
            startTime = DateTime.Now;
        }
        else if (sceneController.CurrentScene() == 4)
        {
            GameObject.Find("DurationDisplay").GetComponent<TextMeshProUGUI>().text =
                totalDuration.ToString() + " seconds played";
            GameObject.Find("FuelSpent").GetComponent<TextMeshProUGUI>().text =
                totalFuelSpent.ToString() + " fuel spent";
            GameObject.Find("LivesLost").GetComponent<TextMeshProUGUI>().text =
                totalLivesLost.ToString() + " lives lost";
        }
        else if (sceneController.CurrentScene() == 5)
        {
            GameObject.FindWithTag("Hud").transform.Find("MenuFinish").gameObject.SetActive(false);
            GameObject.FindWithTag("Hud").transform.Find("MenuOutOfFuel").gameObject.SetActive(false);
            PlayerMovementController pController =
                GameObject.FindWithTag("Player").GetComponent<PlayerMovementController>();
            pController.accelerationFactor = 400;
            pController.jumpForceFactor = 300;
        }
    }

    void Update()
    {
        if (dotDuration > 0)
        {
            dotDuration -= Time.deltaTime;
        }
    }

    public void WorldBuilt()
    {
        GameObject.FindWithTag("Finish").GetComponent<FinishController>().finishEvent.AddListener(ReachedTarget);
        GameObject[] aoeGrounds = GameObject.FindGameObjectsWithTag("AoeGround");
        foreach (GameObject aoeGround in aoeGrounds)
        {
            Debug.Log(aoeGround.GetComponent<AoeController>());
            aoeGround.GetComponent<AoeController>().touchEvent.AddListener(TouchAoe);
        }
    }

    public void OnDeath()
    {
        lives--;
        totalLivesLost++;
        totalFuelSpent += (1000f - fuel);
        totalDuration += DateTime.Now.Subtract(startTime).Seconds;
        valueChanged.Invoke("lives", lives);
        DisplayFailureMenu();
        holdGame.Invoke();
    }

    public void UseFuel(float amount)
    {
        fuel -= amount;
        if (fuel <= 0)
        {
            OnDeath();
        }

        valueChanged.Invoke("fuel", fuel);
    }

    public void DisplayFailureMenu()
    {
        player.GetComponent<PlayerHelper>().Movable(false);
        player.GetComponent<PlayerMovementController>().Stop();
        GameObject menu = GameObject.FindWithTag("Hud").transform.Find("MenuOutOfFuel").gameObject;
        menu.SetActive(true);
        string msg = "";
        if (fuel <= 0)
        {
            msg = "Out of fuel";
        }
        else if (health <= 0)
        {
            msg = "Died";
        }
        else
        {
            msg = "Fallen off a cliff";
        }

        menu.transform.Find("Status").GetComponent<TextMeshProUGUI>().text = msg;
        if (lives == 0)
        {
            Debug.Log(lives);
            menu.transform.Find("ReloadButton").gameObject.SetActive(false);
        }

    }

    public float GetValue(string name)
    {
        switch (name)
        {
            case "lives":
                return GetLives();
            case "fuel":
                return GetFuelAmount();
            case "health":
                return GetHealth();
            default:
                return 0;
        }
    }

    public void Move(string type)
    {
        switch (type)
        {
            case "move":
                UseFuel(.2f);
                break;
            case "jump":
                UseFuel(5);
                break;
        }
    }

    public void TouchAoe(string type)
    {
        switch (type)
        {
            case "poison":
                dotDuration += 2;
                break;
            case "fire":
                InflictDamage(5);
                break;
            case "ice":
                player.GetComponent<PlayerMovementController>().ToogleSteering();
                break;
        }
    }

    public void InflictDamage(float amount)
    {
        health -= amount;
        valueChanged.Invoke("health", health);
        if (health <= 0)
        {
            OnDeath();
        }
    }

    public void Hit()
    {
        InflictDamage(5);
    }

    public int GetLives()
    {
        return lives;
    }

    public float GetFuelAmount()
    {
        return fuel;
    }

    public float GetHealth()
    {
        return health;
    }

    public void Heal(float amount)
    {
        InflictDamage(-amount);
    }

    public void Refuel(float amount)
    {
        UseFuel(-amount);
    }

    public void ReachedTarget()
    {
        holdGame.Invoke();
        player.GetComponent<PlayerHelper>().Movable(false);
        player.GetComponent<PlayerMovementController>().Stop();
        GameObject menu = GameObject.FindWithTag("Hud").transform.Find("MenuFinish").gameObject;
        menu.SetActive(true);
        int duration = DateTime.Now.Subtract(startTime).Seconds;
        totalDuration += duration;
        float fuelSpent = 1000 - GetFuelAmount();
        totalFuelSpent += fuelSpent;
        menu.transform.Find("DurationDisplay").GetComponent<TextMeshProUGUI>().text = duration.ToString() + " seconds";
        menu.transform.Find("FuelSpent").GetComponent<TextMeshProUGUI>().text = fuelSpent.ToString() + " fuel spent";
    }

    public void AddAmmo(int amount)
    {
        ammo += amount;
    }

    public void AttachObjectToPlayer(GameObject obj)
    {
        player.GetComponent<PlayerInventory>().Add(obj);
    }

    public void ResetValues()
    {
        lives = 3;
        health = 100f;    
    }

}