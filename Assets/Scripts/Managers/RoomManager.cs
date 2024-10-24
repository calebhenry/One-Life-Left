using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.ParticleSystem;

public class RoomManager : MonoBehaviour
{
    public int enemyAmount;
    public double spawnInterval;
    [SerializeField] GameObject enemy;
  
    // Start is called before the first frame update
    [SerializeField] GameObject ExitRoom;
    [SerializeField] GameObject ExitDoor;
    [SerializeField] GameObject Exit;
    [SerializeField] GameObject ExitCollision;
    public void EnableNextRoom()
    {
        ExitRoom.SetActive(true);
        ExitCollision.SetActive(true);
        ExitDoor.SetActive(false);
        Exit.SetActive(true);
    }
}
