using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject pickupHealth;
    public GameObject npcPrefab;
    private float countdown;
    private Vector3[,] fields;
    private int xSize;
    private int zSize;

    void Start()
    {
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

        countdown = 10;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    PlaceNpc();
                    break;
                case 1:
                    PlacePickup();
                    break;
            }

            countdown = 10;
        }
    }

    void PlacePickup()
    {
        GameObject instance;
        if (Random.Range(0, 2) < 1)
        {
            instance = pickupHealth;
            PlaceObject(instance, GetField());
        }
    }

    void PlaceNpc()
    {
        GameObject instance = Instantiate(npcPrefab, GetField(), Quaternion.identity);
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
}