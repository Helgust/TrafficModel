using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //private List<Car> carList = new List<Car>();
    public List<GameObject> roadList = new List<GameObject>();
    public List<GameObject> laneList = new List<GameObject>();
    
    public GameObject toInstantiateRoad;
    public GameObject toInstantiateLane;
    public GameObject toInstantiateCar;

    public  int minSpeed;
    public  int maxSpeed;
    public  int minInterval;
    public  int maxInterval;
    private static int valueReducingSpeed;
    private static int timeReducingSpeed;

    private static float intervalCounter;
    
    private static int roadCounter;
    private static int laneCounter;

    public float lengthCar = 40;
    public float heightCar = 40;

    private static int parametersNumber = 6;
    private static int parametersCounter = 0;

    private static bool start = false;
    private static bool pause = false;

    public float ScreenAdjust = Screen.width*2f;
    public static System.Random getRnd = new System.Random();

    float timer = 0;
    bool timerReached = false;

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
        ScreenAdjust = Screen.width/32f;
        //toInstantiate = GameObject.Find("CarTemp");
    }

    void Start()
    {
        minSpeed = 5;
        maxSpeed = 10;
        roadCounter = 1;
        laneCounter = 1;
        Begin();
    }

    private void Begin()
    {
        GenerateRoad();
    }
    private void GenerateRoad()
    {
        GameObject road = Instantiate(toInstantiateRoad, new Vector3(0, 0, 0f), Quaternion.identity);
        road.GetComponent<Road>().RoadInit(roadList.Count, new Vector3(0f, 0f, 0f), 1);
        //road.transform.localScale = new Vector3(ScreenAdjust,2,1);
        road.SetActive(true);
        roadList.Add(road);
    }
    // private void GenerateCar()
    // {
    //     GameObject car = Instantiate(toInstantiate, new Vector3(-10, 0, 0f), Quaternion.identity);
    //     car.GetComponent<Car>().CarInit(carList.Count, lengthCar, heightCar,new Vector3(-10f,0f), getRnd.Next(minSpeed, maxSpeed));
    //     car.SetActive(true);
    //     carList.Add(car);
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}