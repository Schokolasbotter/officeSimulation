using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalMoney = 0;
    public int currentMoney = 0;
    public GameObject workerPrefab;
    public Transform workerContainer;
    public int workerPrice = 1000;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI workerCountText;
    

    // Update is called once per frame
    void Update()
    {
        
        //Spawn new worker
        if (currentMoney > workerPrice)
        {
            Instantiate(workerPrefab, new Vector3(0f, 0f, -9.32f), workerPrefab.transform.rotation, workerContainer);
            currentMoney -= workerPrice;
            workerPrice += 100;
        }
        //Update UI
        moneyText.text = "Total Money:      " + totalMoney + "\nCurrent Money:      " + currentMoney;
        workerCountText.text = "Worker:        " + workerContainer.childCount;

    }
    
    //This function increases money
    public void increaseMoney()
    {
        totalMoney += 100;
        currentMoney += 100;
    }
}
 