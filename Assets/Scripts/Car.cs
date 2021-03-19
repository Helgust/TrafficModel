using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Car
{
    private int _id;
    private Vector3 _pos;
    private float _length, _height;
    private float _initialSpeed, _currentSpeed;
    private float _acceleration, _resist;
    private bool _accidentHappened;
    private bool _inDelay;
    private float _timeInAccident;
    private float _actualTimeReducingSpeed;
    private bool _clicked;
    private bool _drawed;
    private Color _color;


    private int count;
    private int _coef = 10;


    public Car(int id, float length, float height, Vector3 initCoord, float initialSpeed)
    {
        SetId(id);
        SetDrawed(false);
        SetLength(length);
        SetHeight(height);
        SetPos(initCoord);
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

    public void Move(float dt)
    {
        float currentSpeed = GetCurrentSpeed() + GetAcceleration() * dt;
        if (currentSpeed >= 0)
        {
            if (IsAccidentHappened() && IsInDelay())
            {
                SetColor(new Color(255 / 255f, 165 / 255f, 0 / 255f));
            }
            else if (IsAccidentHappened())
            {
                SetColor(Color.red);
            }
            else if (_inDelay)
            {
                SetColor(Color.yellow);
            }
            else
            {
                if (currentSpeed > GetCurrentSpeed())
                {
                    SetColor(Color.magenta);
                }
                else if ((GetCurrentSpeed() == 0) &&
                         (currentSpeed == GetCurrentSpeed()) && !IsAccidentHappened() &&
                         !IsInDelay())
                {
                    SetColor(Color.cyan);
                }
                else if (currentSpeed < GetCurrentSpeed())
                {
                    SetColor(Color.blue);
                }
                else if (currentSpeed == GetCurrentSpeed())
                {
                    SetColor(Color.green);
                }
            }

            SetCurrentSpeed(currentSpeed);
        }
        else
        {
            SetCurrentSpeed(0);
        }

        if (GetCurrentSpeed() * dt + GetAcceleration() * dt * dt / 2 >= 0)
            ShiftCarForward(Time.deltaTime);
    }

    private void ShiftCarForward(float dt)
    {
        var speed = (GetCurrentSpeed() * dt + GetAcceleration() * dt * dt / 2);
        this.SetPos(GetPos() + Vector3.right * speed);
    }

    public void ReduceSpeed(float acceleration, float dt)
    {
        //sets the acceleration according to the front car speed in case of braking
        if (GetCurrentSpeed() > 0)
        {
            SetAcceleration(acceleration);
        }
        else
        {
            SetCurrentSpeed(0);
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

    public void Check(float dspeed, float timeReducingSpeed)
    {
        if (DrawController.instance.mousePressed && !IsClicked())
        {
            if ((DrawController.instance.mousePoint.x <= GetPos().x + GetLength()/2f && DrawController.instance.mousePoint.x >= GetPos().x - GetHeight()/2f) &&
                (DrawController.instance.mousePoint.y <= GetPos().y + GetHeight()/2f && DrawController.instance.mousePoint.y > GetPos().y - GetHeight()/2f))
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

        if (!DrawController.instance.mousePressed && IsClicked())
            SetClicked(false);
    }


    //getters and setters

    public int GetId()
    {
        return _id;
    }

    public void SetId(int id)
    {
        this._id = id;
    }

    public void SetDrawed(bool flag)
    {
        _drawed = flag;
    }

    public bool GetDrawed()
    {
        return _drawed;
    }

    public Vector3 GetPos()
    {
        return _pos;
    }

    public void SetPos(Vector3 coord)
    {
        this._pos = coord;
    }

    public float GetLength()
    {
        return _length;
    }

    public void SetLength(float length)
    {
        this._length = length;
    }

    public float GetHeight()
    {
        return _height;
    }

    public void SetHeight(float height)
    {
        _height = height;
    }

    public float GetInitialSpeed()
    {
        return _initialSpeed;
    }

    public void SetInitialSpeed(float initialSpeed)
    {
        this._initialSpeed = initialSpeed * GameManager.instance.coef;
    }

    public float GetCurrentSpeed()
    {
        return _currentSpeed;
    }

    public void SetCurrentSpeed(float currentSpeed)
    {
        this._currentSpeed = currentSpeed;
    }

    public float GetAcceleration()
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

    public bool IsClicked()
    {
        return _clicked;
    }

    public void SetClicked(bool clicked)
    {
        this._clicked = clicked;
    }

    public Color GetColor()
    {
        return _color;
    }

    public void SetColor(Color new_color)
    {
        _color = new_color;
    }
}