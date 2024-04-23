using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Worker : MonoBehaviour
{

    public float motivation = 100f;
    public float motivationThreshold = 0f;
    public float fun = 100f;
    public float funThreshold = 0f;
    public float exhaustion = 100f;
    public float exhaustionThreshold = 0f;
    public float stress = 100f;
    public float stressThreshold = 0f;
    public float TickRate = 1f;
    public int EvaluateAfterTaskAmmount = 5;

    private GameManager _gameManager;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Station _myAssignedStation;
    private int _myAssignedIndex;
    private bool _isWorking;
    private float _timer;
    private List<int> _priorityOrder = new List<int>();
    private int _taskCounter = 0;
    private Material _workerMaterial;
    
    //Temporary!!!!
    public Station targetStation;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _workerMaterial = transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
        FindSuitableStation(StationTypes.Desk);
        _timer = TickRate;

        motivationThreshold = Mathf.Ceil(Random.Range(10, 100));
        funThreshold = Mathf.Ceil(Random.Range(10, 100));
        exhaustionThreshold = Mathf.Ceil(Random.Range(10, 100));
        stressThreshold = Mathf.Ceil(Random.Range(10, 100));

        _priorityOrder = CreatePriorityOrder();
    }

    // Update is called once per frame
    void Update()
    {
        //Animation
        if (_navMeshAgent.velocity.magnitude != 0)
        {
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }
        
        //Shader
        _workerMaterial.SetFloat("_stressLevel", stress);
        _workerMaterial.SetFloat("_motivationLevel", motivation);
        _workerMaterial.SetFloat("_funLevel", fun);
        _workerMaterial.SetFloat("_exhaustionLevel", exhaustion);
        
        //Start task when arrived at destination
        if (_navMeshAgent.destination == transform.position)
        {
            StartTask(_myAssignedStation);
        }
        
        //Repeat Actions on Tick
        if (_timer <= 0)
        {
            //Reset Timer
            _timer = TickRate;
            //Execute Task
            if (_isWorking)
            {
                ExecuteTask();
                if (_myAssignedStation is WorkStation)
                {
                    _gameManager.increaseMoney();
                }
                _taskCounter++;
            }
            //Evaluate stats
            if (_taskCounter == EvaluateAfterTaskAmmount)
            {
                _taskCounter = 0;
                StopTask();
                StationTypes type = evaluateSituation();
                FindSuitableStation(type);
            }
        }

        _timer -= Time.deltaTime;
    }
    
    //Enum for station
    enum StationTypes
    {
        Desk,
        Bathroom,
        Ceo,
        BreakRoom,
        Talking
    };

    enum Stats
    {
        Stress,
        Motivation,
        Fun,
        Exhaustion
    };
    
    //Evaluate which work to do
    private StationTypes evaluateSituation()
    {
        foreach (int priorityNumber in _priorityOrder)
        {
            //Check Stats
            Stats statToCheck = (Stats)priorityNumber;
            float randomNumber = Random.value;
            switch (statToCheck)
            {
                case Stats.Stress:
                    if (stress < stressThreshold)
                    {
                        switch (randomNumber)
                        {
                            case <0.5f:
                                return StationTypes.Bathroom;
                            case <= 1f:
                                return StationTypes.BreakRoom;
                        }
                    }
                    break;
                case Stats.Motivation:
                    if (motivation < motivationThreshold)
                    {
                        switch (randomNumber)
                        {
                            case <=1f:
                                return StationTypes.Ceo;
                        }
                    }
                    break;
                case Stats.Fun:
                    if (fun < funThreshold)
                    {
                        switch (randomNumber)
                        {
                            case <0.5f:
                                return StationTypes.BreakRoom;
                            case <=1f:
                                return StationTypes.Bathroom;
                        }
                    }
                    break;
                case Stats.Exhaustion:
                    if (exhaustion < exhaustionThreshold)
                    {
                        switch (randomNumber)
                        {
                            case <=1f:
                                return StationTypes.BreakRoom;
                        }
                    }
                    break;
                default:
                    continue;
            }
        }
        return StationTypes.Desk;
    }

    //This function creates a list of randomly arranged numbers from 1 to 3
    //This translates to this workers stat priority
    private List<int> CreatePriorityOrder()
    {
        List<int> order = new List<int>();
        List<int> pool = new List<int>()
        {
            0,
            1,
            2,
            3
        };
        for (int i = 0; i < 4; i++)
        {
            int poolIndex = Random.Range(0, pool.Count);
            order.Add(pool[poolIndex]);
            pool.RemoveAt(poolIndex);
        }
        return order;
    }
    
    //This function finds all available stations and assigns a free one to the worker
    private void FindSuitableStation(StationTypes type)
    {
        List<Station> stationList = new List<Station>();
        switch (type)
        {
            case StationTypes.Desk:
                stationList.AddRange(FindObjectsOfType<WorkStation>());
                break;
            case StationTypes.Bathroom:
                stationList.AddRange(FindObjectsOfType<Bathroom>());
                break;
            case StationTypes.Ceo:
                stationList.AddRange(FindObjectsOfType<Ceo>());
                break;
            case StationTypes.BreakRoom:
                stationList.AddRange(FindObjectsOfType<Breakroom>());
                break;
            case StationTypes.Talking:
                //stationList.AddRange(FindObjectsOfType<Worker>());
                break;
        }

        int maxAttempts = 10;
        int attempts = 0;
        bool canWork = false;
        Station stationToCheck = null;
        while (!canWork && attempts < maxAttempts)
        {
            stationToCheck = stationList[Random.Range(0, stationList.Count)];
            if (!stationToCheck.isFull)
            {
                canWork = true;
            }
            attempts++;
        }
    
        if (canWork)
        {
            GoToTask(stationToCheck);
        }
    }

    public void GoToDesk()
    {
        List<Station> stationList = new List<Station>();
        stationList.AddRange(FindObjectsOfType<WorkStation>());
        foreach (Station station in stationList)
        {
            if (!station.isFull)
            {
                GoToTask(station);
            }
        }
    }

    //This sets the destination for the NavAgent and assigns the station
    private void GoToTask(Station station)
    {
        _myAssignedStation = station;
        _myAssignedIndex = station.GetIndex();
        if (_myAssignedIndex == -1)
        {
            return;
        }
        _navMeshAgent.SetDestination(station.GetWorkPosition(_myAssignedIndex));
        station.SetOccupancy(this, _myAssignedIndex, true);
    }

    //This sets the occupancy of the station and personal working status
    //Also calls the function to rotate
    private void StartTask(Station station)
    {
        _isWorking = true;
        LookAtWorkStation();
    }

    public void StopTask()
    {
        if (_myAssignedIndex != -1)
        {
            _myAssignedStation.SetOccupancy(this, _myAssignedIndex, false);
        }
        _isWorking = false;
    } 
    
    //Update work
    //This order comes from the station to move to the next position (CEO)
    public void UpdateWorker(int newIndex)
    {
        _myAssignedIndex = newIndex;
        _navMeshAgent.SetDestination(_myAssignedStation.GetWorkPosition(_myAssignedIndex));
    }

    //This function rotates the worker to look at his workstation
    //Turns the local forward Vector to the Station position
    private void LookAtWorkStation()
    {
        Vector3 stationPosition = _myAssignedStation.transform.position;
        stationPosition.y = transform.position.y;
        Vector3 toStationDirection =  (stationPosition - transform.position);
        transform.rotation = Quaternion.LookRotation(toStationDirection, Vector3.up);
    }

    //Function To be overwritten by different type of workers
    public void ExecuteTask()
    {
        _myAssignedStation.DoTask(this);
    }
    
    //Getter Functions
    //Index
    public int GetAssignedIndex()
    {
        return _myAssignedIndex;
    }
    
    //Increase and decrease functions
    //Motivation
    public void IncreaseMotivation(float increaseBy)
    {
        motivation += increaseBy;
        motivation = Mathf.Clamp(motivation, 0f, 100f);
    }
    
    public void DecreaseMotivation(float decreaseBy)
    {
        motivation -= decreaseBy;
        motivation = Mathf.Clamp(motivation, 0f, 100f);
    }
    //Fun
    public void IncreaseFun(float increaseBy)
    { 
        fun += increaseBy;
        fun = Mathf.Clamp(fun, 0f, 100f);
    }
    
    public void DecreaseFun(float decreaseBy)
    {
        fun -= decreaseBy;
        fun = Mathf.Clamp(fun, 0f, 100f);
    }
    //Exhaustion
    public void IncreaseExhaustion(float increaseBy)
    {
        exhaustion += increaseBy;
        exhaustion = Mathf.Clamp(exhaustion, 0f, 100f);
    }
    
    public void DecreaseExhaustion(float decreaseBy)
    {
        exhaustion -= decreaseBy;
        exhaustion = Mathf.Clamp(exhaustion, 0f, 100f);
    }
    //Stress
    public void IncreaseStress(float increaseBy)
    {
        stress += increaseBy;
        stress = Mathf.Clamp(stress, 0f, 100f);
    }
    
    public void DecreaseStress(float decreaseBy)
    {
        stress -= decreaseBy;
        stress = Mathf.Clamp(stress, 0f, 100f);
    }
}
