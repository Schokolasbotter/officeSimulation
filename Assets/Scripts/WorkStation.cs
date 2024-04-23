using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : Station
{
    public float stressDecreaseValue = 5f;
    public float motivationDecreaseValue = 5f;
    public float funDecreaseValue = 5f;
    public float exhaustionDecreaseValue = 5f;
    
    public override void DoTask(Worker worker)
    {
        base.DoTask(worker);
        worker.DecreaseStress(stressDecreaseValue);
        worker.DecreaseMotivation(motivationDecreaseValue);
        worker.DecreaseFun(funDecreaseValue);
        worker.DecreaseExhaustion(exhaustionDecreaseValue);
    }

    public override int GetIndex()
    {
        if (isOccupied[0] == false)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }
}
