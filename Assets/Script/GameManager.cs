using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject Doodle;

    private float minY = 0.5f;
    private float maxY = 2f;

    private float screenWidth = 2.75f;

    // starts spawning at :
    private Vector3 SpawnPos = new Vector3(0,8f,0);


    private float spawnLimit = 3f;

    private void LateUpdate() {
        if(Doodle.transform.position.y > SpawnPos.y - spawnLimit){
            SpawnPlaterform();
        }
    }

    private void SpawnPlaterform(){
        //Spawn between screen width and screnn height
        float xPos = Random.Range(-screenWidth,screenWidth);
        float yPos = Random.Range(minY,maxY);

        SpawnPos.x = xPos;
        SpawnPos.y += yPos; 

        Instantiate(tilePrefab, SpawnPos,Quaternion.identity);
    }


}
