using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class WorldCreator : MonoBehaviour
{
    private class Field
    {
        public Vector3 center;
        public Vector2 coordinate;
        public Vector3 originDirection;

        public Field(Vector3 center, Vector2 coordinate, Vector3 origin)
        {
            this.center = center;
            this.coordinate = coordinate;
            this.originDirection = origin;
        }
    }
    
    SceneController sceneController;
    public GameObject pathPrefab;
    public GameObject finishZonePrefab;
    public GameObject enemyTurret;
    public GameObject enemyBrick;
    public GameObject enemyAoe;
    public GameObject pickupHealth;
    public GameObject pickupFuel;
    public GameObject objectSwitch;
    public GameObject objectSurface;
    public GameObject objectSurfaceSmall;
    public GameObject objectPhysical;
    public int width;
    public int height;
    public int fieldAmount;
    GameObject startZone;
    public static Vector3 finishZoneLocation;
    Field[,] fields;
    public static bool levelCreated;
    static List<GameObject> instantiated = new List<GameObject>();
    private GameObject[] obstaclePool;
    public static UnityEvent reload = new UnityEvent();

    void Start()
    {
        fields = new Field[width, height];
        sceneController = GameObject.Find("GameManager").GetComponent<GameManager>().sceneController;
        startZone = GameObject.Find("StartZone");
        obstaclePool = new [] {objectSurfaceSmall, enemyBrick, enemyAoe};
        if (!levelCreated)
        {
            CreatePath();
            levelCreated = true;
        }
        CreateFinishZone(finishZoneLocation);
        reload.Invoke();
        GameObject.Find("GameManager").GetComponent<GameManager>().WorldBuilt();
    }

    public void CreateFinishZone(Vector3 location)
    {
        Instantiate(finishZonePrefab, location, Quaternion.identity);

        switch (sceneController.CurrentScene())
        {
            case SceneController.sceneLevel01:
                PlaceIce(location);
                break;
            case SceneController.sceneLevel02:
                PlaceBrick(location);
                break;
            case SceneController.sceneLevel03:
                PlaceFire(location);
                break;
        }
    }

    private void CreatePath()
    {
        Renderer startRenderer = startZone.GetComponent<Renderer>();
        Vector3 pathSize = pathPrefab.GetComponent<Renderer>().bounds.size;
        Vector3 startZoneOffset = new Vector3(0, 0, startRenderer.bounds.extents.x - pathSize.x / 2);
        Vector3 currentDirection = Vector3.forward;
        Vector3 tileCenter = Vector3.zero;

        int i = 0;
        bool isStart = true;
        Vector2 coordinate = new Vector2(width / 2 - 1, 0);
        bool switchDirection = false;
        do
        {
            tileCenter += currentDirection;
            if (isStart)
            {
                tileCenter += startZoneOffset;
                isStart = false;
            }

            PlaceObject(pathPrefab, tileCenter);
            int chance = Random.Range(0, 20);
            if (chance <= 1)
            {
                PlacePickup(tileCenter);
            }

            fields[(int) coordinate.x, (int) coordinate.y] = new Field(tileCenter, coordinate, currentDirection);

            if (switchDirection)
            {
                currentDirection = GetNewDirection(currentDirection, coordinate);
            }

            switchDirection = !switchDirection; // switch placement direction every 2nd iteration

            // if no tile can be placed, i.e. if it would create loops or reach out-of-bounds:
            // backtrack through 
            while (currentDirection.Equals(Vector3.zero))
            {
                Field currentField = fields[(int) coordinate.x, (int) coordinate.y];
                int x = (int) coordinate.x - (int) currentField.originDirection.x;
                int y = (int) coordinate.y - (int) currentField.originDirection.z;
                coordinate = new Vector2(x, y);
                currentDirection = GetNewDirection(Vector3.Reflect(currentDirection, currentDirection), coordinate);
            }

            coordinate = new Vector2(coordinate.x + currentDirection.x, coordinate.y + currentDirection.z);

            i++;
            if (i % 5 == 0)
            {
                PlaceObstacle(tileCenter);
            }
        } while (i < fieldAmount);

        Vector3 finishLineDirection = GetOutwardFacingDirection(coordinate);
        for (int j = 0; j < 5; j++)
        {
            if (coordinate.x < 0 || coordinate.y < 0 || coordinate.x >= width || coordinate.y >= height)
            {
                break;
            }
            tileCenter += finishLineDirection;
            PlaceObject(pathPrefab, tileCenter);
            fields[(int) coordinate.x, (int) coordinate.y] = new Field(tileCenter, coordinate, currentDirection);
            coordinate = new Vector2(coordinate.x + currentDirection.x, coordinate.y + currentDirection.z);
        }
        finishZoneLocation = tileCenter;
    }

    void PlaceObstacle(Vector3 location)
    {
        int obstacleCap = 0;
        switch (sceneController.CurrentScene())
        {
            case SceneController.sceneLevel01:
                obstacleCap = 1;
                break;
            case SceneController.sceneLevel02:
                obstacleCap = 2;
                break;
            case SceneController.sceneLevel03:
                obstacleCap = 3;
                break;
        }
        GameObject obj = obstaclePool[Random.Range(0, obstacleCap)];
        GameObject obstacle = PlaceObject(obj, location);
    }

    private Vector3 GetNewDirection(Vector3 currentDirection, Vector2 coordinate)
    {
        // all directions, excluding going back
        List<Vector3> directions = new List<Vector3>();
        if (!Vector3.forward.Equals(currentDirection))
        {
            directions.Add(Vector3.back);
        }

        if (!Vector3.back.Equals(currentDirection))
        {
            directions.Add(Vector3.forward);
        }

        if (!Vector3.left.Equals(currentDirection))
        {
            directions.Add(Vector3.right);
        }

        if (!Vector3.right.Equals(currentDirection))
        {
            directions.Add(Vector3.left);
        }

        // remove already occupied or out-of-bounds fields from possibilities
        for (int n = directions.Count - 1; n >= 0; n--)
        {
            int x1 = (int) coordinate.x + (int) directions[n].x;
            int y1 = (int) coordinate.y + (int) directions[n].z;
            int x2 = (int) coordinate.x + (int) directions[n].x * 2;
            int y2 = (int) coordinate.y + (int) directions[n].z * 2;
            if (x1 < 0 || x1 >= width || y1 < 0 || y1 >= height || fields[x1, y1] != null ||
                x2 < 0 || x2 >= width || y2 < 0 || y2 >= height || fields[x2, y2] != null)
            {
                directions.RemoveAt(n);
            }
        }

        Vector3 newDirection = Vector3.zero;

        if (directions.Count > 0)
        {
            newDirection = directions[Random.Range(0, directions.Count)];
            int startX = width / 2 - 1;
            bool undesirableDirection = newDirection == Vector3.back || //backwards towards start area
                                        (coordinate.x < startX && newDirection == Vector3.right) || // inwards from left
                                        (coordinate.x > startX && newDirection == Vector3.left); // inwards from right
            if (undesirableDirection && Random.Range(0, 2) > 0)
            {
                // 50% chance of re-roll on undesirable direction
                newDirection = directions[Random.Range(0, directions.Count)];
            }
        }

        return newDirection;
    }

    private Vector3 GetOutwardFacingDirection(Vector2 coordinate)
    {
        Vector2 lowestX = Vector2.zero;
        for (int x = 0; x < (width / 2 - 1); x++)
        {
            for (int y = height - 1; y > 0; y--)
            {
                if (fields[x, y] != null)
                {
                    lowestX = fields[x, y].coordinate;
                    break;
                }
            }

            if (lowestX != Vector2.zero)
            {
                break;
            }
        }

        Vector2 highestX = Vector2.zero;
        for (int x = width - 1; x > (width / 2 - 1); x--)
        {
            for (int y = height - 1; y > 0; y--)
            {
                if (fields[x, y] != null)
                {
                    highestX = fields[x, y].coordinate;
                    break;
                }
            }

            if (highestX != Vector2.zero)
            {
                break;
            }
        }

        Vector2 highestY = Vector2.zero;
        for (int y = height - 1; y > 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                if (fields[x, y] != null)
                {
                    highestY = fields[x, y].coordinate;
                    break;
                }
            }

            if (highestY != Vector2.zero)
            {
                break;
            }
        }

        Vector3 direction = Vector3.right;
        Vector2 deltaX = highestX;
        if (Vector2.Distance(Vector2.zero, lowestX) > Vector2.Distance(Vector2.zero, highestX))
        {
            deltaX = lowestX;
            direction = Vector3.left;
        }

        if (Vector2.Distance(Vector2.zero, deltaX) > Vector2.Distance(Vector2.zero, highestY))
        {
            direction = Vector3.forward;
        }

        return direction;
    }

    public void ClearLevel()
    {
        foreach (GameObject obj in instantiated)
        {
            Destroy(obj);
        }
        instantiated = new List<GameObject>();
        levelCreated = false;
    }

    void PlaceTurret(Vector3 finishLocation)
    {
        Vector3 location = new Vector3(finishLocation.x + 8, finishLocation.y, finishLocation.z + 8);
        Instantiate(enemyTurret, location, Quaternion.identity);
    }

    void PlaceBrick(Vector3 location)
    {
        Instantiate(enemyBrick, location, Quaternion.identity);
    }

    void PlaceIce(Vector3 location)
    {
        Instantiate(objectSurface, location, Quaternion.identity);
    }

    void PlaceFire(Vector3 location)
    {
        Vector3 placement = new Vector3(location.x + 1f, location.y, location.z);
        Instantiate(enemyAoe, placement, Quaternion.identity);
        placement = new Vector3(location.x - 1f, location.y, location.z);
        Instantiate(enemyAoe, placement, Quaternion.identity);
        placement = new Vector3(location.x, location.y, location.z + 1f);
        Instantiate(enemyAoe, placement, Quaternion.identity);
        placement = new Vector3(location.x, location.y, location.z - 1f);
        Instantiate(enemyAoe, placement, Quaternion.identity);
    }

    void PlacePickup(Vector3 tileCenter)
    {
        Vector3 location = new Vector3(tileCenter.x, tileCenter.y + 0.2f, tileCenter.z);

        GameObject instance;
        if (Random.Range(0, 2) < 1)
        {
            instance = pickupFuel;
        }
        else
        {
            instance = pickupHealth;
        }

        PlaceObject(instance, location);
    }

    private GameObject PlaceObject(GameObject obj, Vector3 location)
    {
        GameObject instance = Instantiate(obj, location, Quaternion.identity);
        DontDestroyOnLoad(instance);
        instantiated.Add(instance);
        return instance;
    }
}