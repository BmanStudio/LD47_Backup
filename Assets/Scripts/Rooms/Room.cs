using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomManager roomManager=null;
    public int index = 0;
    void Start()
    {
        roomManager.RegisterRoom(index,this.gameObject);
    }
}
