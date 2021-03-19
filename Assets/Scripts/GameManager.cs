using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.U2D.Path;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //private List<Car> carList = new List<Car>();
    private List<Road> roadList = new List<Road>();
    
    public int minSpeed;
    public int maxSpeed;
    public int minInterval;
    public int maxInterval;
    public float valueReducingSpeed;
    public float timeReducingSpeed;

    private int roadCounter;
    private int laneCounter;
    
    public int coef = 10;
    public bool isStart;
    public bool isPause;
    public float lengthCar;
    public float heightCar;
    public int counter;

    public bool timerReached; 
    public float intervalTimer;

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
        valueReducingSpeed = 1f;
        timeReducingSpeed = 5f;
        lengthCar = 40;
        heightCar = 20;
        timerReached = false;
        intervalTimer = GameManager.getRnd.Next(GameManager.instance.minInterval, GameManager.instance.maxInterval);
    }


    private void GenerateRoad()
    {
        roadList.Add(new Road(roadList.Count, new Vector3(0, 0, 0), 1));
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
                for (int i = 0; i < roadList.Count; i++)
                {
                    Debug.Log("roadList.Count= "+roadList[i].GetPos());
                    DrawController.instance.DrawRoad(roadList[i]);
                    if (roadList[i].GetList().Count < laneCounter)
                    {
                        roadList[i].GenerateLane();
                    }
                }
            }

            for (int i = 0; i < roadList.Count; i++)
            {
                roadList[i].DoStuff();
            }
        }
        else
        {
            counter = 0;
            DrawController.instance.deleteAll();
            if (roadList.Count != 0)
            {
                foreach (var road in roadList)
                {
                    List<Lane>lanes =  road.GetList();
                    foreach (var lane in lanes)
                    {
                        lane.deleteList();
                    }
                    lanes.Clear();
                }
            }
            roadList.Clear();
        }
    }

    public void PreesExit()
    {
        DrawController.instance.deleteAll();
        if (roadList.Count != 0)
        {
            foreach (var road in roadList)
            {
                List<Lane>lanes =  road.GetList();
                foreach (var lane in lanes)
                {
                    lane.deleteList();
                }
                lanes.Clear();
            }
        }
        roadList.Clear();

        Application.Quit();
    }
}