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

    private float minY = 0.5f;
    private float maxY = 1.5f;

    private float minMoveY = 0.1f;
    private float maxMoveY = 0.6f;

    private float minBreakY = 0.1f;
    private float maxBreakY = 0.5f;


    private float screenWidth = 2.5f;

    // starts spawning at :
    private Vector3 SpawnPos = new Vector3(0,8f,0);
    private float spawnLimit = 3f;


    private void LateUpdate() {
        if(Doodle.transform.position.y > SpawnPos.y - spawnLimit){
            SpawnPlateform();
            
            if(Random.Range(0,1) <= 0.5){
                SpawnBreakingPlateform();
            }
            if(Random.Range(0,1) <=0.3){
                SpawnMovingPlateform();
            }
        }
    }

    private void SpawnPlateform(){
        //Spawn between screen width and screnn height
        float xPos = Random.Range(-screenWidth,screenWidth);
        float yPos = Random.Range(minY,maxY);

        SpawnPos.x = xPos;
        SpawnPos.y += yPos; 

        Instantiate(tilePrefab, SpawnPos,Quaternion.identity);
    }

    private void SpawnMovingPlateform(){
        float xPos = Random.Range(-screenWidth,screenWidth);
        float yPos = Random.Range(minMoveY,maxMoveY);

        SpawnPos.x = xPos;
        SpawnPos.y += yPos; 
        Instantiate(movingTilePrefab, SpawnPos,Quaternion.identity);
    }

    private void SpawnBreakingPlateform(){
        float xPos = Random.Range(-screenWidth,screenWidth);
        float yPos = Random.Range(minBreakY,maxBreakY);

        SpawnPos.x = xPos;
        SpawnPos.y += yPos; 
        Instantiate(breakingTilePrefab, SpawnPos,Quaternion.identity);
    }


}
