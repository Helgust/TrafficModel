﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject errorWindow;
    public Text StartButtonText;
    public Text PauseButtonText;
    public InputField MinSpeed;
    public InputField MaxSpeed;
    public InputField MinInterval;
    public InputField MaxInterval;
    public InputField deltaVReduce;
    public InputField deltaTReduce;


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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void pressStart()
    {
        if (!GameManager.instance.isStart)
        {
            if(GetDataFromUI())
            {
                GameManager.instance.isStart = true;
                StartButtonText.text = "НОВЫЙ СТАРТ";
            }
        }
        else
        {
            GameManager.instance.isStart = false;
            StartButtonText.text = "СТАРТ";
        }
    }

    public void makePause()
    {
        if (!GameManager.instance.isPause)
        {
            Time.timeScale = 0;
            GameManager.instance.isPause = true;
            PauseButtonText.text = "ПРОДОЛЖИТЬ";
        }
        else
        {
            Time.timeScale = 1;
            GameManager.instance.isPause = false;
            PauseButtonText.text = "ПАУЗА";
        }
    }

    private bool GetDataFromUI()
    {
        if (MinSpeed.text != String.Empty
            && MaxSpeed.text != String.Empty
            && MinInterval.text != String.Empty
            && MaxInterval.text != String.Empty
            && deltaVReduce.text != String.Empty
            && deltaTReduce.text != String.Empty)
        {
            int _minSpeed = int.Parse(MinSpeed.text);
            int _maxSpeed = int.Parse(MaxSpeed.text);
            int _minInterval = int.Parse(MinInterval.text);
            int _maxInterval = int.Parse(MaxInterval.text);
            int _valueReducingSpeed = int.Parse(deltaVReduce.text);
            int _timeReducingSpeed = int.Parse(deltaTReduce.text);
            if (_minSpeed > _maxSpeed)
            {
                //todo error minV>maxV
                return false;
            }
            if (_minInterval > _maxInterval)
            {
                //todo error minInter>maxInter
                return false;
            }
            if (_minSpeed > _maxSpeed)
            {
                //todo error minV>maxV
                return false;
            }

            if ( _minSpeed == 0 )
            {
            // param is Zero
            return false;
            }
            if ( _maxSpeed == 0 )
            {
            // param is Zero
            return false;
            }
            if ( _minInterval == 0 )
            {
            // param is Zero
            return false;
            }
            if ( _maxInterval == 0 )
            {
            // param is Zero
            return false;
            }
            if ( _valueReducingSpeed == 0 )
            {
            // param is Zero
            return false;
            }
            if ( _timeReducingSpeed == 0 )
            {
            // param is Zero
            return false ;
            }
            GameManager.instance.minSpeed =_minSpeed;
            GameManager.instance.maxSpeed =_maxSpeed;
            GameManager.instance.minInterval =_minInterval;
            GameManager.instance.maxInterval =_maxInterval;
            GameManager.instance.valueReducingSpeed =_valueReducingSpeed;
            GameManager.instance.timeReducingSpeed =_timeReducingSpeed;
            return true;
        }
        else
        {
            Debug.Log("Empty");
            return false;
            //todo input filed is empty
        }
        
    }
}