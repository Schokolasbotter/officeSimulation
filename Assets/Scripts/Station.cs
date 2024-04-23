using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Station : MonoBehaviour
{
    public bool isFull = false;
    public bool[] isOccupied;
    public Worker[] assignedWorkers;
    public Transform[] workPositions;

    private void Update()
    {
        //Check if I am full
        isFull = true;
        foreach (bool occupiedPosition in isOccupied)
        {
            isFull &= occupiedPosition;
        }
    }

    //Set the station as occupied
    public void SetOccupancy(Worker worker, int index, bool newState)
    {
        isOccupied[index] = newState;
        assignedWorkers[index] = worker;
    }
    
    //Returns the position of the assigned Index
    public Vector3 GetWorkPosition(int index)
    {
        return workPositions[index].position;
    }
    
    //Overwrite for different type of stations
    public virtual void DoTask(Worker worker){}

    public virtual int GetIndex()
    {
        return 0;
    }
}
