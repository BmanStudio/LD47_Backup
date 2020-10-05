using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomManager : MonoBehaviour
{
    Dictionary<int, GameObject> rooms = new Dictionary<int, GameObject>();
    public GameObject[] repeatingRooms;
    public GameObject bossRoom;
    float room2Size = -1*(40+9.8f); //Room + Inbetween
    public int bossRoomNumber = 9;
    public int runTime = 1;
    internal void RegisterRoom(int index, GameObject gameObject)
    {
        rooms.Add(index, gameObject);
    }

    public void BetweenTrains(int index)
    {
        AddRoomAt(index + 2);
        AddRoomAt(index - 2);
    }
   public void MiddleOfTrain(int index)
    {
        RemoveRoomAt(index + 3);
        RemoveRoomAt(index - 3);
    }
    void AddRoomAt(int index)
    {
        if (index < 0 ) return;
      
        if (!rooms.ContainsKey(index))
        {
        Vector3 pos = new Vector3(index*room2Size, 0, 0);
            GameObject roomObj = null;
            if (index % bossRoomNumber == 0) roomObj = Instantiate(bossRoom, pos, Quaternion.identity);
            else roomObj = Instantiate(repeatingRooms[UnityEngine.Random.Range(0, repeatingRooms.Length)], pos, Quaternion.identity);
            // Added by Bman:
            //roomObj.isStatic = true;
            
            Room room = roomObj.GetComponent<Room>();
            room.roomManager = this;
            room.index = index;
           // rooms[index]= roomObj; no need, The room automatically registers itself upon execution

        }
    }
    void RemoveRoomAt(int index)
    {

        if (rooms.ContainsKey(index))
        {
            Destroy(rooms[index], 0);
            rooms.Remove(index); 
        }
    }

}
