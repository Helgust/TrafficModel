using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //private List<Car> carList = new List<Car>();
    private List<GameObject> roadList = new List<GameObject>();

    public GameObject toInstantiateRoad;
    public GameObject toInstantiateLane;
    public GameObject toInstantiateCar;

    public int minSpeed;
    public int maxSpeed;
    public int minInterval;
    public int maxInterval;
    public int valueReducingSpeed;
    public int timeReducingSpeed;

    private int roadCounter;
    private int laneCounter;
    
    public int coef = 10;
    public bool isStart;
    public bool isPause;
    public float lengthCar;
    public float heightCar;
    public int counter;

    public static System.Random getRnd = new System.Random();
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
        //toInstantiate = GameObject.Find("CarTemp");
    }

    private void Start()
    {
        counter = 0;
        isStart = false;
        isPause = false;
        minSpeed = 4;
        maxSpeed = 5;
        minInterval = 1;
        maxInterval = 5;
        roadCounter = 1;
        laneCounter = 1;
        valueReducingSpeed = 1;
        timeReducingSpeed = 5;
        lengthCar = 40;
        heightCar = 20;
    }


    private void GenerateRoad()
    {
        GameObject road = Instantiate(toInstantiateRoad, new Vector3(0f, 0f, 0f), Quaternion.identity);
        road.GetComponent<Road>().RoadInit(roadList.Count, new Vector3(0, 0, 0), 1);
        road.SetActive(true);
        roadList.Add(road);
    }


    // Update is called once per frame
    private void Update()
    {
        if (isStart)
        {
            counter++;
            if (counter <= roadCounter)
            {
                GenerateRoad();
            }
        }
        else
        {
            counter = 0;
            foreach (var r in roadList)
            {
                Destroy(r);
            }
        }
    }

    public void PreesExit()
    {
        if (roadList.Count != 0)
        {
            foreach (var VARIABLE in roadList)
            {
                Destroy(VARIABLE);
            }
        }

        Application.Quit();
    }
}