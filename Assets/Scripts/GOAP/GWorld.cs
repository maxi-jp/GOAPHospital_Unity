using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld
{
    private static readonly GWorld instance = new GWorld();
    private static WorldStates world;
    private static Queue<GameObject> patientsWaitingInTheWaitingRoom;

    static GWorld()
    {
        world = new WorldStates();
        patientsWaitingInTheWaitingRoom = new Queue<GameObject>();
    }

    public static GWorld Instance
    {
        get { return instance; }
    }

    public WorldStates GetWorld()
    {
        return world;
    }

    public void AddPatientToTheWaitingRoom(GameObject p)
    {
        patientsWaitingInTheWaitingRoom.Enqueue(p);
    }

    public GameObject RemovePatientFromTheWaitingRoom()
    {
        return patientsWaitingInTheWaitingRoom.Count > 0 ? patientsWaitingInTheWaitingRoom.Dequeue() : null;
    }

}
