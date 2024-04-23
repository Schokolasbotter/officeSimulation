using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bathroom : Station
{
    public float stressIncreaseValue = 5f;
    public float funIncreaseValue = 5f;
    public float exhaustionDecreaseValue = 5f;
    
    public override void DoTask(Worker worker)
    {
        base.DoTask(worker);
        worker.IncreaseStress(stressIncreaseValue);
        worker.IncreaseFun(funIncreaseValue);
        worker.DecreaseExhaustion(exhaustionDecreaseValue);
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
