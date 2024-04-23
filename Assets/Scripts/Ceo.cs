using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ceo : Station
{
    public float motivationIncreaseValue = 5f;
    public float funDecreaseValue = 5f;
    private int _taskCount = 0;

    private void FixedUpdate()
    {
        
    }

    public override void DoTask(Worker worker)
    {
        if (worker.GetAssignedIndex() == 0)
        {
            if (_taskCount == 10)
            {
                _taskCount = 0;
            }
            _taskCount++;
            base.DoTask(worker);
            worker.IncreaseMotivation(motivationIncreaseValue);
            worker.DecreaseFun(funDecreaseValue);
        }
        
        if (_taskCount == 10)
        {
            //End task with index 0
            if (worker.GetAssignedIndex() == 0)
            {
                worker.StopTask();
                worker.GoToDesk();
            }
            //Update Worker
            else
            {
                worker.UpdateWorker(worker.GetAssignedIndex() - 1);
            }
        }
    }
    
    public override int GetIndex()
    {
        for (int i = 0; i < isOccupied.Length; i++)
        {
            if (isOccupied[i] == false)
            {
                return i;
            }
        }

        return -1;
    }
    
}
