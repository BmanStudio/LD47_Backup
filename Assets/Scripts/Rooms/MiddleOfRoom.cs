using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleOfRoom : MonoBehaviour
{
    public Room room;
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
            room.roomManager.MiddleOfTrain(room.index);
    }
}
