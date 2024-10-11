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

    public GameObject springPrefab;

    private float minY = 0.5f;
    private float maxY = 1.5f;

    private float minMoveY = 0.1f;
    private float maxMoveY = 0.6f;

    private float minBreakY = 0.1f;
    private float maxBreakY = 0.5f;

    private float tileLenght = 0.250f;
    private float tileHeight = 0.130f; 

    private float screenWidth = 2.5f;

    // starts spawning at :
    private Vector3 SpawnPos = new Vector3(0,8f,0);
    private float spawnLimit = 3f;


    private void LateUpdate() {
        GameObject returnedTile;
        if(Doodle.transform.position.y > SpawnPos.y - spawnLimit){
            returnedTile = SpawnPlateform();
            if(Random.value < 0.3){
                    SpawnSpring(returnedTile);
                }

            if(Random.value <= 0.5){
                SpawnBreakingPlateform();
                
            }
            if(Random.value <=0.3){
                returnedTile = SpawnMovingPlateform();
                if(Random.value < 0.3){
                    SpawnSpring(returnedTile);
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


}
