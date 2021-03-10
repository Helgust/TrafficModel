using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Lane : MonoBehaviour
{
    private int _id;
    private Vector3 _initCoord;
    public List<GameObject> carList = new List<GameObject>();
    
    private bool timerReached;
    private float intervalTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        timerReached = false;
        intervalTimer = GameManager.getRnd.Next(GameManager.instance.minInterval, GameManager.instance.maxInterval);
        
    }


    // Update is called once per frame
    void Update()
    {
        CheckCar();
        if (!timerReached)
        {
            if (intervalTimer > 0)
            {
                intervalTimer -= Time.deltaTime;
            }
            else
            {
                intervalTimer = 0;
                timerReached = true;
            }
        }
        else
        {
            intervalTimer = GameManager.getRnd.Next(GameManager.instance.minInterval, GameManager.instance.maxInterval);
            timerReached = false;
        }
    }

    private void SetId(int id)
    {
        this._id = id;
    }

    private void GetId(int id)
    {
        this._id = id;
    }

    public Vector3 GetCoord()
    {
        return _initCoord;
    }

    private void SetCoord(Vector3 coord)
    {
        this._initCoord = coord;
        gameObject.GetComponent<SpriteRenderer>().size = new Vector2(Screen.width*2f, 60f);
    }

    public void LaneInit(int id, Vector3 initCoord)
    {
        SetId(id);
        SetCoord(initCoord);
    }

    private void CreateCar()
    {
        GameObject car = Instantiate(GameManager.instance.toInstantiateCar, new Vector3(-100f, 0f, 0f),
            Quaternion.identity);
        car.GetComponent<Car>().CarInit(carList.Count, GameManager.instance.lengthCar, GameManager.instance.heightCar,
            new Vector3(-3f, 0f, 0f), GameManager.getRnd.Next(GameManager.instance.minSpeed,GameManager.instance.maxSpeed));
        car.transform.SetParent(gameObject.transform);
        car.SetActive(true);
        carList.Add(car);
    }

    private void CheckCar()
    {
        if (carList.Count == 0)
        {
            CreateCar();
        }
        if (timerReached && carList[carList.Count-1].transform.position.x > GameManager.instance.lengthCar)
       {
           CreateCar();
       }
       
    }
}