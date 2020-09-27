using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject pickupHealth;
    public GameObject pickupAmmo;
    public GameObject pickupFuel;
    public GameObject npcPrefab;
    public int pickupCountdownDuration;
    public int enemyCountdownDuration;
    private float pickupCountdown;
    private float enemyCountdown;
    private Vector3[,] fields;
    private int xSize;
    private int zSize;
    private GameManager gameManager;
    public List<GameObject> enemies = new List<GameObject>();
    private bool active;
    private bool started;

    void Start()
    {
        started = false;
        active = true;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.holdGame.AddListener(Clear);
        GameObject arena = GameObject.Find("Arena");
        Renderer arenaRenderer = arena.GetComponent<Renderer>();
        Bounds arenaBounds = arenaRenderer.bounds;
        xSize = (int) arenaBounds.extents.x;
        zSize = (int) arenaBounds.extents.z;
        fields = new Vector3[xSize * 2, zSize * 2];
        for (int x = 0; x < xSize * 2; x++)
        {
            for (int z = 0; z < zSize * 2; z++)
            {
                fields[x, z] = new Vector3(x - 10, 0.2f, z - 10);
            }
        }

        pickupCountdown = pickupCountdownDuration;
        enemyCountdown = enemyCountdownDuration;
        GameObject.Find("Button").GetComponent<ButtonController>().buttonPressed.AddListener(StartArena);
    }

    void Update()
    {
        if (started && active)
        {
            pickupCountdown -= Time.deltaTime;
            enemyCountdown -= Time.deltaTime;
            if (pickupCountdown <= 0)
            {
                PlacePickup();
                pickupCountdown = pickupCountdownDuration;
            }

            if (enemyCountdown <= 0)
            {
                PlaceNpc();
                enemyCountdown = enemyCountdownDuration;
            }
        }
    }

    void PlacePickup()
    {
        GameObject instance;
        switch (Random.Range(0, 2))
        {
            case 0:
                instance = pickupHealth;
                PlaceObject(instance, GetField());
                break;
            case 1:
                instance = pickupAmmo;
                PlaceObject(instance, GetField());
                break;
            case 2:
                instance = pickupFuel;
                PlaceObject(instance, GetField());
                break;
        }
    }

    void PlaceNpc()
    {
        GameObject instance = Instantiate(npcPrefab, GetField(), Quaternion.identity);
        instance.GetComponent<NpcCombatController>().hit.AddListener(gameManager.EnemyHit);
        enemies.Add(instance);
    }

    private void PlaceObject(GameObject obj, Vector3 location)
    {
        GameObject instance = Instantiate(obj, location, Quaternion.identity);
    }

    Vector3 GetField()
    {
        int x = Random.Range(0, xSize);
        int z = Random.Range(0, zSize);
        return fields[x, z];
    }

    void Clear()
    {
        active = false;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Npc"))
        {
            Destroy(enemy);
        }
    }

    void StartArena()
    {
        started = true;
    }
}