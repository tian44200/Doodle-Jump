using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    public GameObject endPageManager; // Add reference to EndPageManager


    private float minY = 0.5f;
    private float maxY = 1.5f;

    private float minMoveY = 0.1f;
    private float maxMoveY = 0.6f;

    private float minBreakY = 0.1f;
    private float maxBreakY = 0.5f;

    private float tileLenght = 0.250f;
    private float tileHeight = 0.130f; 

    private float screenWidth = 2.5f;


    private float blackHolewidth = 0.68f;
    private float blackHoleHeight = 1f;

    private float monsterWidth = 0.75f;
    private float monsterHeight = 0.27f;

    // starts spawning at :
    private Vector3 SpawnPos = new Vector3(0,8f,0);
    private float spawnLimit = 3f;


    private void LateUpdate() {
        GameObject returnedTile;
        if(Doodle.transform.position.y > SpawnPos.y - spawnLimit){
            returnedTile = SpawnPlateform();
            if(Random.value < 0.15){
                SpawnSpring(returnedTile);
            }else if(Random.value < 0.15){
                SpawnHat(returnedTile);
            }else if(Random.value < 0.15){
                SpawnJetPack(returnedTile);
            }
            
            
            if(Random.value < 0.10){
                SpawnBlackHole();
            }

            if(Random.value < 0.10){
                SpawnMonster();
            }


            if(Random.value <= 0.5){
                SpawnBreakingPlateform();
            }
            if(Random.value <=0.2){
                returnedTile = SpawnMovingPlateform();
                if(Random.value < 0.15){
                    SpawnSpring(returnedTile);
                }else if(Random.value < 0.15){
                    SpawnHat(returnedTile);
                }else if(Random.value < 0.15){
                    SpawnJetPack(returnedTile);
                }
            }
        }
    }

    private GameObject SpawnPlateform(){
        //Spawn between screen width and screnn height
        float xPos = Random.Range(-screenWidth,screenWidth);
        float yPos = Random.Range(minY,maxY);

        SpawnPos.x = xPos;
        SpawnPos.y += yPos; 

        return Instantiate(tilePrefab, SpawnPos,Quaternion.identity);
    }

    private GameObject SpawnMovingPlateform(){
        float xPos = Random.Range(-screenWidth,screenWidth);
        float yPos = Random.Range(minMoveY,maxMoveY);

        SpawnPos.x = xPos;
        SpawnPos.y += yPos; 
        return Instantiate(movingTilePrefab, SpawnPos,Quaternion.identity);
    }

    private GameObject SpawnBreakingPlateform(){
        float xPos = Random.Range(-screenWidth,screenWidth);
        float yPos = Random.Range(minBreakY,maxBreakY);

        SpawnPos.x = xPos;
        SpawnPos.y += yPos; 
        return Instantiate(breakingTilePrefab, SpawnPos,Quaternion.identity);
    }

    private void SpawnSpring(GameObject dad){
        float xPos = Random.Range(-tileLenght,tileLenght);

        SpawnPos.x += xPos;
        SpawnPos.y += tileHeight; 
        Instantiate(springPrefab, SpawnPos,Quaternion.identity, dad.transform);
    }

    private void SpawnHat(GameObject dad){
        float xPos = Random.Range(-tileLenght,tileLenght);

        SpawnPos.x += xPos;
        SpawnPos.y += tileHeight; 
        Instantiate(hatPrefab, SpawnPos,Quaternion.identity, dad.transform);
    }

    private void SpawnJetPack(GameObject dad){
        float xPos = Random.Range(-tileLenght,tileLenght);

        SpawnPos.x += xPos;
        SpawnPos.y += tileHeight*2f; 
        Instantiate(jetPackPrefab, SpawnPos,Quaternion.identity, dad.transform);
    }

    private void SpawnBlackHole(){
        float xPos = Random.Range(-screenWidth+blackHolewidth,screenWidth-blackHolewidth);// +/- BlackHole width
        float yPos = Random.Range(minBreakY,maxBreakY-blackHoleHeight);// +/- BlackHole Height

        SpawnPos.x = xPos;
        SpawnPos.y += yPos+blackHoleHeight;

        GameObject bh = Instantiate(blackHolePrefab, SpawnPos,Quaternion.identity);
        bh.GetComponent<LoseCondition>().SetUIManager(uiManager);
        if(SpawnPos.x >= 0){
            xPos = Random.Range(-screenWidth,SpawnPos.x-blackHolewidth);
            SpawnPos.x = xPos;
            Instantiate(tilePrefab, SpawnPos,Quaternion.identity);
        }else{
            xPos = Random.Range(SpawnPos.x+blackHolewidth,screenWidth);
            SpawnPos.x = xPos;
            Instantiate(tilePrefab, SpawnPos,Quaternion.identity);
        }

        SpawnPos.y += blackHoleHeight;
    }

    private void SpawnMonster(){
        float xPos = Random.Range(-screenWidth+monsterWidth,screenWidth-monsterWidth);// +/- BlackHole width
        float yPos = Random.Range(minBreakY,maxBreakY-monsterHeight);// +/- BlackHole Height

        SpawnPos.x = xPos;
        SpawnPos.y += yPos+monsterHeight;

        GameObject bh = Instantiate(monsterPrefab, SpawnPos,Quaternion.identity);
        bh.GetComponent<LoseCondition>().SetUIManager(uiManager);
        if(SpawnPos.x >= 0){
            xPos = Random.Range(-screenWidth,SpawnPos.x-monsterWidth);
            SpawnPos.x = xPos;
            Instantiate(tilePrefab, SpawnPos,Quaternion.identity);
        }else{
            xPos = Random.Range(SpawnPos.x+monsterWidth,screenWidth);
            SpawnPos.x = xPos;
            Instantiate(tilePrefab, SpawnPos,Quaternion.identity);
        }

        SpawnPos.y += monsterHeight;
    }

}
