using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Prefabs
    public GameObject tilePrefab;
    public GameObject movingTilePrefab;
    public GameObject breakingTilePrefab;
    public GameObject Doodle;
    public GameObject hatPrefab;
    public GameObject springPrefab;
    public GameObject jetPackPrefab;
    public GameObject monsterPrefab;
    public GameObject blackHolePrefab;
    public GameObject uiManager;
    public GameObject DoodleHat;
    public GameObject DoodleJetPack;


    //Max and min distance for tiles
    private float minY = 0.5f;
    private float maxY = 1.45f;

    private float minMoveY = 0.5f;
    private float maxMoveY = 1.3f;

    private float minBreakY = 0.5f;
    private float maxBreakY = 0.58f;

    private float minBH = 1.4f;
    private float maxBH = 1.55f;

    private float minMonster = 0.6f;
    private float maxMonster = 1.10f;

    //To have a random speed
    private float minSpeed = 0.7f;
    private float maxSpeed = 1.8f;

    private float tileLenght = 0.250f;
    private float tileHeight = 0.130f;

    private float screenWidth = 2.5f;

    //Black hole size
    private float blackHolewidth = 1.2f;
    private float blackHoleHeight = 1.8f;

    //Monster size
    private float monsterWidth = 1.20f;

    // starts spawning if doodle above SpawnPos.y-spawnLimit:
    private Vector3 SpawnPos = new Vector3(0, 7f, 0);
    private float spawnLimit = 4f;

    private bool isDead = false;


    // if Doodle is using the spring we don't spawn BlackHole or Monster
    private bool onSpring = false;
    private PlayerControl playerControl;

    // Adding difficulty when going up
    private float difficulty;
    private float maxDiff = 500f;
    private ScoreManager scoreManager;

    void Awake()
    {
        playerControl = Doodle.GetComponent<PlayerControl>();
        scoreManager = GetComponent<ScoreManager>();
        difficulty = Mathf.Min(1, scoreManager.gethighestPoint() / maxDiff);
    }


    private void Update()
    {
        if (!isDead)
        {
            GameObject returnedTile;
            if (Doodle.transform.position.y >= SpawnPos.y - spawnLimit)
            {
                difficulty = Mathf.Min(1, scoreManager.gethighestPoint() / maxDiff);
                returnedTile = SpawnPlateform();

                //Spawn Objects on tile depending on probability
                if (Random.value < 0.05){
                    SpawnObject(springPrefab,returnedTile);
                }else if (Random.value < 0.04){
                    SpawnObject(hatPrefab,returnedTile);
                }else if (Random.value <0.03){
                    SpawnObject(jetPackPrefab,returnedTile);
                }
            
                onSpring = playerControl.getUsedSpring();
                if (DoodleHat.activeSelf == false && DoodleJetPack.activeSelf == false && onSpring == false)
                {
                    if (Random.value < 0.1)
                    {
                        SpawnMonster();
                    }
                    if (Random.value < 0.1)
                    {
                        SpawnBlackHole();
                    }

                }


                if (Random.value <= 0.3)
                {
                    SpawnBreakingPlateform();
                }


                // Spawn Objects on tile depending on probability
                if (Random.value <= 0.2)
                {
                    returnedTile = SpawnMovingPlateform(difficulty);
                    if (Random.value < 0.05)
                    {
                        SpawnObject(springPrefab,returnedTile);
                    }
                    else if (Random.value < 0.05)
                    {
                        SpawnObject(hatPrefab,returnedTile);
                    }
                    else if (Random.value < 0.03)
                    {
                        SpawnObject(jetPackPrefab,returnedTile);
                    }
                }
            }
        }
    }



    private GameObject SpawnPlateform()
    {
        //Spawn between screen width and screnn height
        float xPos = Random.Range(-screenWidth, screenWidth);
        float yPos = Random.Range(minY, Mathf.Max(minY, maxY * difficulty));

        SpawnPos.x = xPos;
        SpawnPos.y += yPos;

        return Instantiate(tilePrefab, SpawnPos, Quaternion.identity);
    }




    private GameObject SpawnMovingPlateform(float difficulty)
    {
        float xPos = Random.Range(-screenWidth, screenWidth);
        float yPos = Random.Range(minMoveY, Mathf.Max(minMoveY, maxMoveY * difficulty));

        SpawnPos.x = xPos;
        SpawnPos.y += yPos;

        GameObject movingPlateform = Instantiate(movingTilePrefab, SpawnPos, Quaternion.identity);

        float speed = Random.Range(minSpeed, Mathf.Max(minSpeed,maxSpeed*difficulty));
        movingPlateform.GetComponent<MovingTile>().speed = speed;
        return movingPlateform;
    }




    private GameObject SpawnBreakingPlateform()
    {
        float xPos = Random.Range(-screenWidth, screenWidth);
        float yPos = Random.Range(minBreakY, Mathf.Max(minBreakY, maxBreakY * difficulty));
        SpawnPos.x = xPos;
        SpawnPos.y += yPos;
        return Instantiate(breakingTilePrefab, SpawnPos, Quaternion.identity);
    }




    private void SpawnObject(GameObject prefab, GameObject dad){
        float xPos = Random.Range(-tileLenght, tileLenght);

        SpawnPos.x += xPos;

        if(prefab.name == "PickableJetPack"){
            SpawnPos.y += tileHeight * 2f;
        }else{
            SpawnPos.y += tileHeight;
        }
        
        Instantiate(prefab, SpawnPos, Quaternion.identity, dad.transform);
    }



    private void SpawnBlackHole()
    {
        float xPos = Random.Range(-screenWidth + blackHolewidth, screenWidth - blackHolewidth);// +/- BlackHole width
        float yPos = Random.Range(minBH, maxBH);// +/- BlackHole Height

        SpawnPos.x = xPos;
        SpawnPos.y += minBH;

        GameObject bh = Instantiate(blackHolePrefab, SpawnPos, Quaternion.identity);

        //Spawn two tiles depending on the side of the blackHole
        if (SpawnPos.x >= 0)
        {
            Vector3 tilePos = new Vector3();
            xPos = Random.Range(-screenWidth, SpawnPos.x - blackHolewidth);
            tilePos.x = xPos;
            tilePos.y = SpawnPos.y - blackHoleHeight * 0.25f;
            Instantiate(tilePrefab, tilePos, Quaternion.identity);

            xPos = Random.Range(-screenWidth, SpawnPos.x - blackHolewidth);
            tilePos.x = xPos;
            tilePos.y = tilePos.y + blackHoleHeight * 0.5f;
            Instantiate(tilePrefab, tilePos, Quaternion.identity);
        }
        else{
            Vector3 tilePos= new Vector3();
            xPos = Random.Range(SpawnPos.x + blackHolewidth, screenWidth);
            tilePos.x = xPos;
            tilePos.y = SpawnPos.y - blackHoleHeight * 0.25f;
            Instantiate(tilePrefab, tilePos, Quaternion.identity);

            xPos = Random.Range(SpawnPos.x + blackHolewidth, screenWidth);
            tilePos.x = xPos;
            tilePos.y = tilePos.y + blackHoleHeight * 0.5f;
            Instantiate(tilePrefab, tilePos, Quaternion.identity);

        }

        SpawnPos.y += blackHoleHeight*0.25f;

    }



    private void SpawnMonster()
    {
        float xPos = Random.Range(-screenWidth + monsterWidth, screenWidth - monsterWidth);
        float yPos = Random.Range(minMonster, maxMonster);

        SpawnPos.x = xPos;
        SpawnPos.y += yPos;

        GameObject mst = Instantiate(monsterPrefab, SpawnPos, Quaternion.identity);

        //Spawn tile depending on the position of the monster
        if (SpawnPos.x >= 0)//Monster on the right, spawn the tile on the left
        {
            xPos = Random.Range(-screenWidth, SpawnPos.x - monsterWidth);
            SpawnPos.x = xPos;
            Instantiate(tilePrefab, SpawnPos, Quaternion.identity);
        }
        else
        {
            xPos = Random.Range(SpawnPos.x + monsterWidth, screenWidth);
            SpawnPos.x = xPos;
            Instantiate(tilePrefab, SpawnPos, Quaternion.identity);
        }

    }

    public void SetIsDead(bool isDead)
    {
        this.isDead = isDead;
    }

}