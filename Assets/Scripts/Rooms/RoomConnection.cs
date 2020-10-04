using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnection : MonoBehaviour
{
    public Room room;
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>() !=null)
        room.roomManager.BetweenTrains(room.index);
    }
}
