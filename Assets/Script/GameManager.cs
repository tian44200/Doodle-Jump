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

    private float tileLenght = 0.250f;
    private float tileHeight = 0.130f;

    private float screenWidth = 2.5f;

    //Black hole size
    private float blackHolewidth = 0.68f;
    private float blackHoleHeight = 2f;

    //Monster size
    private float monsterWidth = 0.80f;
    private float monsterHeight = 0.27f;

    // starts spawning if doodle above SpawnPos.y-spawnLimit:
    private Vector3 SpawnPos = new Vector3(0, 8f, 0);
    private float spawnLimit = 4f;

    private bool isDead = false;


    // if Doodle is using the spring we don't spawn BlackHole or Monster
    private bool onSpring = false;
    private PlayerControl playerControl;

    // Adding difficulty when going up
    private float difficulty;
    private ScoreManager scoreManager;

    void Awake()
    {
        playerControl = Doodle.GetComponent<PlayerControl>();
        scoreManager = GetComponent<ScoreManager>();
        difficulty = Mathf.Min(1,scoreManager.gethighestPoint()/500f);
    }


    private void Update()
    {
        if (!isDead)
        {
            GameObject returnedTile;
            if (Doodle.transform.position.y >= SpawnPos.y - spawnLimit)
            {
                difficulty = Mathf.Min(1,scoreManager.gethighestPoint()/500f);
                
                returnedTile = SpawnPlateform();
                if (Random.value < 0.10)
                {
                    SpawnSpring(returnedTile);
                }
                else if (Random.value < 0.1)
                {
                    SpawnHat(returnedTile);
                }
                else if (Random.value < 0.1)
                {
                    SpawnJetPack(returnedTile);
                }

                onSpring = playerControl.getUsedSpring();
                // if(DoodleHat.activeSelf == false && DoodleJetPack.activeSelf == false && onSpring == false){
                //     if (Random.value < 0.1)
                //     {
                //         SpawnBlackHole();
                //     }

                //     if (Random.value < 0.1)
                //     {
                //         SpawnMonster();
                //     }
                // }


                if (Random.value <= 0.5)
                {
                    SpawnBreakingPlateform();
                }
                if (Random.value <= 0.2)
                {
                    returnedTile = SpawnMovingPlateform();
                    if (Random.value < 0.1)
                    {
                        SpawnSpring(returnedTile);
                    }
                    else if (Random.value < 0.1)
                    {
                        SpawnHat(returnedTile);
                    }
                    else if (Random.value < 0.1)
                    {
                        SpawnJetPack(returnedTile);
                    }
                }
            }
        }
    }



    private GameObject SpawnPlateform()
    {
        //Spawn between screen width and screnn height
        float xPos = Random.Range(-screenWidth, screenWidth);
        float yPos = Random.Range(minY, Mathf.Max(0.5f,maxY*difficulty));
        Debug.Log(yPos + "nomaar");

        SpawnPos.x = xPos;
        SpawnPos.y += yPos;

        return Instantiate(tilePrefab, SpawnPos, Quaternion.identity);
    }




    private GameObject SpawnMovingPlateform()
    {
        float xPos = Random.Range(-screenWidth, screenWidth);
        float yPos = Random.Range(minMoveY, Mathf.Max(0.5f,maxMoveY*difficulty));
        Debug.Log(yPos + "move");

        SpawnPos.x = xPos;
        SpawnPos.y += yPos;
        return Instantiate(movingTilePrefab, SpawnPos, Quaternion.identity);
    }




    private GameObject SpawnBreakingPlateform()
    {
        float xPos = Random.Range(-screenWidth, screenWidth);
        float yPos = Random.Range(minBreakY, Mathf.Max(0.5f,maxBreakY*difficulty));
        Debug.Log(yPos + "break");

        SpawnPos.x = xPos;
        SpawnPos.y += yPos;
        return Instantiate(breakingTilePrefab, SpawnPos, Quaternion.identity);
    }




    private void SpawnSpring(GameObject dad)
    {
        float xPos = Random.Range(-tileLenght, tileLenght);

        SpawnPos.x += xPos;
        SpawnPos.y += tileHeight;
        Instantiate(springPrefab, SpawnPos, Quaternion.identity, dad.transform);
    }




    private void SpawnHat(GameObject dad)
    {
        float xPos = Random.Range(-tileLenght, tileLenght);

        SpawnPos.x += xPos;
        SpawnPos.y += tileHeight;
        Instantiate(hatPrefab, SpawnPos, Quaternion.identity, dad.transform);
    }




    private void SpawnJetPack(GameObject dad)
    {
        float xPos = Random.Range(-tileLenght, tileLenght);

        SpawnPos.x += xPos;
        SpawnPos.y += tileHeight * 2f;
        Instantiate(jetPackPrefab, SpawnPos, Quaternion.identity, dad.transform);
    }





    private void SpawnBlackHole()
    {
        float xPos = Random.Range(-screenWidth + blackHolewidth, screenWidth - blackHolewidth);// +/- BlackHole width
        float yPos = Random.Range(minBreakY, maxBreakY - blackHoleHeight);// +/- BlackHole Height

        SpawnPos.x = xPos;
        SpawnPos.y += yPos + blackHoleHeight;

        GameObject bh = Instantiate(blackHolePrefab, SpawnPos, Quaternion.identity);
        bh.GetComponent<LoseCondition>().SetUIManager(uiManager);
        if (SpawnPos.x >= 0)
        {
            xPos = Random.Range(-screenWidth, SpawnPos.x - blackHolewidth);
            SpawnPos.x = xPos;
            SpawnPos.y = SpawnPos.y - blackHoleHeight*0.25f; 
            Instantiate(tilePrefab, SpawnPos, Quaternion.identity);
            SpawnPos.y = SpawnPos.y + blackHoleHeight*0.5f; 
            Instantiate(tilePrefab, SpawnPos, Quaternion.identity);
        }
        else
        {
            xPos = Random.Range(SpawnPos.x + blackHolewidth, screenWidth);
            SpawnPos.x = xPos;
            SpawnPos.y = SpawnPos.y - blackHoleHeight*0.25f; 
            Instantiate(tilePrefab, SpawnPos, Quaternion.identity);
            SpawnPos.y = SpawnPos.y + blackHoleHeight*0.5f; 
            Instantiate(tilePrefab, SpawnPos, Quaternion.identity);

        }

    }





    private void SpawnMonster()
    {
        float xPos = Random.Range(-screenWidth + monsterWidth, screenWidth - monsterWidth);// +/- BlackHole width
        float yPos = Random.Range(minBreakY, maxBreakY - monsterHeight);// +/- BlackHole Height

        SpawnPos.x = xPos;
        SpawnPos.y += yPos + monsterHeight;

        GameObject bh = Instantiate(monsterPrefab, SpawnPos, Quaternion.identity);
        bh.GetComponent<LoseCondition>().SetUIManager(uiManager);
        if (SpawnPos.x >= 0)
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
