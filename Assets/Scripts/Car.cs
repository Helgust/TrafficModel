using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Car : MonoBehaviour
{
    private int _id;
    private Vector3 _initCoord;
    private float _length, _height;
    private float _initialSpeed, _currentSpeed;
    private float _acceleration;
    private bool _accidentHappened;
    private bool _inDelay;
    private float _timeInAccident;
    private float _actualTimeReducingSpeed;
    public bool clicked;
    private Color _color;

    // public Car(int id, float length, float height, float coordX, float coordY, float initialSpeed)
    // {
    //     SetId(id);
    //     SetLength(length);
    //     SetHeight(height);
    //     SetCoordX(coordX);
    //     SetCoordY(coordY);
    //     SetInitialSpeed(initialSpeed);
    //     SetCurrentSpeed(initialSpeed);
    //     SetAcceleration(0);
    //     SetAccidentHappened(false);
    //     SetInDelay(false);
    //     SetTimeInAccident(0);
    //     SetClicked(false);
    //     SetColor(Color.green);
    //     SetActualTimeReducingSpeed(0);
    // }

    public void CarInit(int id, float length, float height, Vector3 initCoord, float initialSpeed)
    {
        SetId(id);
        SetLength(length);
        SetHeight(height);
        SetCoord(initCoord);
        SetInitialSpeed(initialSpeed);
        SetCurrentSpeed(initialSpeed);
        SetAcceleration(0);
        SetAccidentHappened(false);
        SetInDelay(false);
        SetTimeInAccident(0);
        SetClicked(false);
        SetColor(Color.green);
        SetActualTimeReducingSpeed(0);
    }
    
    
    public void OnMouseDown()
    {
        if (!IsClicked())
        {
            Debug.Log("!23");
            SetClicked(true);
            
        }
    }

    public void OnMouseUp()
    {
        if (IsClicked())
        {
            SetClicked(false);
        }
    }


    public void Update()
    {
        var o = gameObject;
        o.transform.position = o.transform.position + new Vector3(_initialSpeed, 0, 0) * Time.deltaTime;
        if (gameObject.transform.position.x > 100f)
        {
            Destroy(gameObject);
        }
    }


    public void Move(float dt)
    {
        //changes the coordinate,speed and color of the car in accordinance with specified acceleration
        if (GetCurrentSpeed() + GetAcceleration() * dt >= 0)
        {
            if (IsAccidentHappened() && IsInDelay())
            {
                _color = new Color(1.0f, 0.45f, 0.0f);
            }
            else if (IsAccidentHappened())
            {
                _color = Color.red;
            }
            else if (_inDelay)
            {
                _color = Color.yellow;
            }
            else
            {
                if (GetCurrentSpeed() + GetAcceleration() * dt > GetCurrentSpeed())
                {
                    _color = Color.magenta;
                }
                else if (GetCurrentSpeed() + GetAcceleration() * dt == GetCurrentSpeed())
                {
                    _color = Color.green;
                }
                else if (GetCurrentSpeed() + GetAcceleration() * dt < GetCurrentSpeed())
                {
                    _color = Color.blue;
                }
            }

            SetCurrentSpeed(GetCurrentSpeed() + GetAcceleration() * dt);
        }
        else
        {
            SetCurrentSpeed(0);
        }

        if (GetCurrentSpeed() * dt + GetAcceleration() * dt * dt / 2 >= 0)
            ShiftCarForward(dt);
    }

    public void Check(bool mousePressed, float mouseX, float mouseY, float dspeed, int timeReducingSpeed)
    {
        //checks whether the pushing of the mouse was on the car
        if (mousePressed && !IsClicked())
        {
            if (mouseX <= GetCoord().x + GetLength() && mouseX >= GetCoord().x &&
                mouseY <= GetCoord().y + GetHeight() && mouseY > GetCoord().y)
            {
                SetActualTimeReducingSpeed(GetActualTimeReducingSpeed() + timeReducingSpeed);
                if (GetCurrentSpeed() - dspeed >= 0)
                {
                    SetCurrentSpeed(GetCurrentSpeed() - dspeed);
                }
                else
                {
                    SetCurrentSpeed(0);
                }

                SetColor(Color.yellow);
                SetAcceleration(0);
                SetClicked(true);
                SetInDelay(true);
            }
        }

        if (!mousePressed && IsClicked())
            SetClicked(false);
    }

    public void ReduceSpeed(float acceleration, float speedFront, float dt)
    {
        //sets the acceleration according to the front car speed in case of braking
        if (GetCurrentSpeed() + acceleration * dt >= speedFront)
        {
            SetAcceleration(acceleration);
        }
        else
        {
            SetCurrentSpeed(speedFront);
            SetAcceleration(0);
        }
    }

    public void IncreaseSpeed(float acceleration, float dt)
    {
        //sets the acceleration according to the car initial speed in case of acceleration
        if (GetCurrentSpeed() + acceleration * dt < GetInitialSpeed())
        {
            SetAcceleration(acceleration);
        }
        else
        {
            SetCurrentSpeed(GetInitialSpeed());
            SetAcceleration(0);
        }
    }

    private void ShiftCarForward(float dt)
    {
        float px = (float) 3.7938105; //pixels in one mm
        //SetCoord(GetCoord().x + (GetCurrentSpeed() * dt + GetAcceleration() * dt * dt / 2) * px,0);
    }
    //getters and setters

    public int GetId()
    {
        return _id;
    }

    private void SetId(int id)
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
        gameObject.transform.position = coord;
    }
    
    

    public float GetLength()
    {
        return _length;
    }

    private void SetLength(float length)
    {
        this._length = length;
    }

    public float GetHeight()
    {
        return _height;
    }

    private void SetHeight(float height)
    {
        this._height = height;
    }

    public float GetInitialSpeed()
    {
        return _initialSpeed;
    }

    private void SetInitialSpeed(float initialSpeed)
    {
        this._initialSpeed = initialSpeed;
    }

    public float GetCurrentSpeed()
    {
        return _currentSpeed;
    }

    public void SetCurrentSpeed(float currentSpeed)
    {
        this._currentSpeed = currentSpeed;
    }

    private float GetAcceleration()
    {
        return _acceleration;
    }

    public void SetAcceleration(float acceleration)
    {
        this._acceleration = acceleration;
    }

    public bool IsAccidentHappened()
    {
        return _accidentHappened;
    }

    public void SetAccidentHappened(bool accidentHappened)
    {
        this._accidentHappened = accidentHappened;
    }

    public bool IsInDelay()
    {
        return _inDelay;
    }

    public void SetInDelay(bool inDelay)
    {
        this._inDelay = inDelay;
    }


    public float GetTimeInAccident()
    {
        return _timeInAccident;
    }

    public void SetTimeInAccident(float timeInAccident)
    {
        this._timeInAccident = timeInAccident;
    }

    public float GetActualTimeReducingSpeed()
    {
        return _actualTimeReducingSpeed;
    }

    public void SetActualTimeReducingSpeed(float actualTimeReducingSpeed)
    {
        this._actualTimeReducingSpeed = actualTimeReducingSpeed;
    }

    private bool IsClicked()
    {
        return clicked;
    }

    private void SetClicked(bool clicked)
    {
        this.clicked = clicked;
    }

    public Color GetColor()
    {
        return _color;
    }

    private void SetColor(Color color)
    {
        _color = color;
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }
}