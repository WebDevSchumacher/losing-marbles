using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldCreator : MonoBehaviour
{
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
    public GameObject objectPhysical;
    GameObject startZone;
    static GameObject finishZone;
    Dictionary<string, float> finishBoundries = new Dictionary<string, float>();

    float levelReach;
    Vector3 goalCenter;
    direction orientationX;
    direction orientationZ;

    public static bool levelCreated;
    private enum direction {North, East, South, West};
    List<GameObject> instantiated = new List<GameObject>();
    
    void Awake()
    {
        
        
    }

    void Start()
    {
        sceneController = GameObject.Find("GameManager").GetComponent<GameManager>().sceneController;
        startZone = GameObject.Find("StartZone");
        if(!levelCreated){
            CreateFinishZone();
            CreatePathToTarget(GetStartingDirection());
            levelCreated = true;
        }
    }

    private void CreateFinishZone(){
        int goalX = Random.Range(10, 21) * (Random.Range(0,2)*2-1); // Distance: between 10 and 20; Direction: randomly multiplied by 1 or -1
        int goalZ = Random.Range(10, 21) * (Random.Range(0,2)*2-1);
        int goalY = Random.Range(-5, 6);
        goalCenter = new Vector3(Random.Range(10, 21) * (Random.Range(0,2)*2-1), 0, Random.Range(10, 21) * (Random.Range(0,2)*2-1));

        orientationX = (goalCenter.x > 0) ? direction.North : direction.South;
        orientationZ = (goalCenter.z > 0) ? direction.East : direction.West;

        finishZone = Instantiate(finishZonePrefab, goalCenter, Quaternion.identity);
        DontDestroyOnLoad(finishZone);
        instantiated.Add(finishZone);
        Vector3 finishSize = finishZone.GetComponent<Renderer>().bounds.size;
        Vector3 pathSize = pathPrefab.GetComponent<Renderer>().bounds.size;
        Vector3 northEastCorner = new Vector3(goalCenter.x + finishSize.x/2 + pathSize.x/2, 0, goalCenter.z + finishSize.z/2 + pathSize.z/2);
        Vector3 southWestCorner = new Vector3(goalCenter.x - finishSize.x/2 - pathSize.x/2, 0, goalCenter.z - finishSize.z/2 - pathSize.z/2);
        finishBoundries.Add("min_x", southWestCorner.x);
        finishBoundries.Add("max_x", northEastCorner.x);
        finishBoundries.Add("min_z", southWestCorner.z);
        finishBoundries.Add("max_z", northEastCorner.z);

        levelReach = Vector3.Distance(startZone.GetComponent<Renderer>().bounds.center, goalCenter);
        if(sceneController.CurrentScene() == SceneController.sceneLevel03){
            PlaceFire();
        }

        if(sceneController.CurrentScene() == SceneController.sceneLevel02){
            PlaceBrick();
        }

        if(sceneController.CurrentScene() == SceneController.sceneLevel01){
            PlaceIce();
        }
    }

    private direction GetStartingDirection(){
        direction startingDirection;
        if(Mathf.Abs(goalCenter.x) >= Mathf.Abs(goalCenter.z)){ //maximum distance in x direction
            if(goalCenter.x > 0){ // forward direction from stating perspective
                startingDirection = direction.North;
            } else {
                startingDirection = direction.South;
            }
        } else {
            if(goalCenter.z > 0){ // righthand direction from stating perspective
                startingDirection = direction.East;
            } else {
                startingDirection = direction.West;
            }
        }
        return startingDirection;
    }

    private void CreatePathToTarget(direction startingDirection){
        Renderer startRenderer = startZone.GetComponent<Renderer>();
        Vector3 startCenter = startRenderer.bounds.center;
        Vector3 pathSize = pathPrefab.GetComponent<Renderer>().bounds.size;

        float startZoneOffset = startRenderer.bounds.extents.x - pathSize.x / 2;

        direction currentDirection = startingDirection;
        Vector3 tileCenter = new Vector3(0,0,0);
        Vector3 targetDirection = new Vector3(0,0,0);
        int i = 0;
        bool switchDirection = false;
        do{
            switch(currentDirection){
            case direction.North:
                // targetDirection = new Vector3(100, 0, 0);
                tileCenter.x += pathSize.x+startZoneOffset;
                break;
            case direction.East:
                // targetDirection = new Vector3(0, 0, 100);
                tileCenter.z += pathSize.x+startZoneOffset;
                break;
            case direction.South:
                tileCenter.x -= pathSize.x+startZoneOffset;
                // targetDirection = new Vector3(-100, 0, 0);
                break;
            case direction.West:
                tileCenter.z -= pathSize.x+startZoneOffset;
                // targetDirection = new Vector3(0, 0, -100);
                break;
            }
            // tileCenter = Vector3.MoveTowards(tileCenter, targetDirection, pathPrefab.transform.localScale.x+startZoneOffset);
            if(Vector3.Distance(startCenter, tileCenter) > levelReach){
                currentDirection = GetRandomDirection(currentDirection, tileCenter);
                continue;
            }

            GameObject tile = Instantiate(pathPrefab, tileCenter, Quaternion.identity);
            DontDestroyOnLoad(tile);
            instantiated.Add(tile);
            int chance = Random.Range(0,10);
            if(chance <= 1){
                PlacePickup(tileCenter);
            }

            startZoneOffset = 0.0f;
            if(switchDirection){
                currentDirection = GetRandomDirection(currentDirection, tileCenter);
            }
            switchDirection = !switchDirection; // switch placement direction every 2nd iteration
            i++;
        }while(!FinishZoneReached(tileCenter) && i < 30);
    }

    private direction GetRandomDirection(direction current, Vector3 position){ // returns a random direction except the opposite of 'current', we don't want to go back where we came from
        bool isOpositeDirection = true;
        orientationX = (goalCenter.x > position.x) ? direction.North : direction.South;
        orientationZ = (goalCenter.z > position.z) ? direction.East : direction.West;
        direction newDirection = current;

        do{
            newDirection = Random.Range(0, 2) > 0 ? orientationX : orientationZ;
            // if(newDirection != orientationX && newDirection != orientationZ){
            //     continue;
            // }

            switch(current){
                case direction.North:
                    isOpositeDirection = (newDirection == direction.South);
                    break;
                case direction.East:
                    isOpositeDirection = (newDirection == direction.West);
                    break;
                case direction.South:
                    isOpositeDirection = (newDirection == direction.North);
                    break;
                case direction.West:
                    isOpositeDirection = (newDirection == direction.East);
                    break;
            }
        }while(isOpositeDirection);
        return newDirection;
    }

    private bool FinishZoneReached(Vector3 tileCenter){
        bool reached = false;
        reached = (tileCenter.x >= finishBoundries["min_x"] && tileCenter.x <= finishBoundries["max_x"]) && (tileCenter.z >= finishBoundries["min_z"] && tileCenter.z <= finishBoundries["max_z"]);
        return reached;
    }

    public void ClearLevel(){
        foreach(GameObject obj in instantiated){
            Destroy(obj);
        }
        levelCreated = false;
    }

    void PlaceTurret(){
        Vector3 location = new Vector3(goalCenter.x, goalCenter.y, goalCenter.z + 8);
        GameObject turret = Instantiate(enemyTurret, location, Quaternion.identity);
        DontDestroyOnLoad(turret);
    }

    void PlaceBrick(){
        Vector3 location = new Vector3(goalCenter.x, goalCenter.y+0.5f, goalCenter.z);
        GameObject brick = Instantiate(enemyBrick, location, Quaternion.identity);
        DontDestroyOnLoad(brick);
    }

    void PlaceIce(){
        Vector3 location = new Vector3(goalCenter.x, goalCenter.y, goalCenter.z);
        GameObject ice = Instantiate(objectSurface, location, Quaternion.identity);
        DontDestroyOnLoad(ice);
    }

    void PlaceFire(){
        Vector3 location1 = new Vector3(goalCenter.x+1f, goalCenter.y, goalCenter.z);
        Vector3 location2 = new Vector3(goalCenter.x-1f, goalCenter.y, goalCenter.z);
        Vector3 location3 = new Vector3(goalCenter.x, goalCenter.y, goalCenter.z+1f);
        Vector3 location4 = new Vector3(goalCenter.x, goalCenter.y, goalCenter.z-1f);
        GameObject ice = Instantiate(enemyAoe, location1, Quaternion.identity);
        DontDestroyOnLoad(ice);
        ice = Instantiate(enemyAoe, location2, Quaternion.identity);
        DontDestroyOnLoad(ice);
        ice = Instantiate(enemyAoe, location3, Quaternion.identity);
        DontDestroyOnLoad(ice);
        ice = Instantiate(enemyAoe, location4, Quaternion.identity);
        DontDestroyOnLoad(ice);
    }

    void PlacePickup(Vector3 tileCenter){
        Vector3 location = new Vector3(tileCenter.x, tileCenter.y+0.2f, tileCenter.z);

        GameObject instance;
        if(Random.Range(0,2) < 1){
            instance = pickupFuel;
        } else {
            instance = pickupHealth;
        }
        GameObject pickup = Instantiate(instance, location, Quaternion.identity);
        DontDestroyOnLoad(pickup);
    }
}
