using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomManager : MonoBehaviour
{
    Dictionary<int, GameObject> rooms = new Dictionary<int, GameObject>();
    public GameObject trainRepeat;
    float room2Size = 40;

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
        if (index < 0) return;

        Vector3 pos = new Vector3(index*room2Size, 0, 0);
        if (!rooms.ContainsKey(index))
        {
            
            GameObject roomObj = Instantiate(trainRepeat, pos, Quaternion.identity);
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
