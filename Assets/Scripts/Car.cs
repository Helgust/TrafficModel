using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    private int _id;
    private Vector3 _initCoord;
    private float _length, _height;
    private float _initialSpeed, _currentSpeed;
    private float _acceleration, _resist;
    private bool _accidentHappened;
    private bool _inDelay;
    private float _timeInAccident;
    private float _actualTimeReducingSpeed;
    public Text carText;
    public bool clicked;
    private Color _color;

    private RaycastHit2D _hit;
    private float brakeLow = -1;
    private int count;
    private int coef = 10;

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

    public void Start()
    {
        count = 0;
        gameObject.GetComponentInChildren<Canvas>().sortingOrder = 5;
    }

    public void FixedUpdate()
    {
        //Length of the ray
        float laserLength = Mathf.Infinity;

        //Get the first object hit by the ray
        _hit = Physics2D.Raycast(gameObject.transform.position, //+ new Vector3(_length / 2f, 0, 0),
            Vector2.right,
            laserLength);


    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Crash");
        gameObject.transform.position = gameObject.transform.position + Vector3.right * 0.002f;
        //other.transform.gameObject.GetComponent<Car>().SetAccidentHappened(true);
        SetCurrentSpeed(0);
        SetAccidentHappened(true);
    }
    
    public void OnMouseDown()
    {
        if (!IsClicked())
        {
            SetActualTimeReducingSpeed(GetActualTimeReducingSpeed() + GameManager.instance.timeReducingSpeed);
            if (GetCurrentSpeed() - GameManager.instance.valueReducingSpeed >= 0f)
            {
                SetCurrentSpeed(GetCurrentSpeed() - GameManager.instance.valueReducingSpeed);
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

    public void OnMouseUp()
    {
        if (IsClicked())
        {
            SetClicked(false);
        }
    }

    public void Update()
    {
        if (!GameManager.instance.isPause)
        {
            if (GetId() == 1)
            {
                Debug.Log("Dist" + _hit.distance);
            }
            carText.text = Math.Round(GetCurrentSpeed()/10).ToString(CultureInfo.InvariantCulture);
            UpdateSpeeds(Time.deltaTime);
            Check();
            Move(Time.deltaTime);
        }
    }

    private void Check()
    {
        if (_inDelay)
        {
            if (GetActualTimeReducingSpeed() <= 0)
            {
                SetActualTimeReducingSpeed(0);
                SetInDelay(false);
            }
            else
            {
                SetActualTimeReducingSpeed(GetActualTimeReducingSpeed() - Time.deltaTime);
            }
        }

        if (_accidentHappened)
        {
            if (GetCurrentSpeed() == 0)
            {
                SetAcceleration(0);
            }

            if (Mathf.Round(GetTimeInAccident()) >= GameManager.instance.timeReducingSpeed)
            {
                if ((GetId() == 0) || (GetId() > 0 &&
                                       (_hit.distance > GetLength())))
                {
                    SetAccidentHappened(false);
                    SetTimeInAccident(0);
                }
            }

            SetTimeInAccident(GetTimeInAccident() + Time.deltaTime);
        }
    }

    private void UpdateSpeeds(float dt)
    {
        if (_hit.collider != null)
        {
            Car carNext = _hit.transform.gameObject.GetComponent<Car>();

            if (!carNext.IsAccidentHappened() || !carNext.IsInDelay())
            {
                if (_hit.distance <= 3 * carNext.GetLength())
                {
                    ReduceSpeed(-10*GameManager.instance.coef,Time.deltaTime);
                    return;
                }
                
            }

            if (!carNext.IsAccidentHappened())
            {
                if (GetCurrentSpeed() < GetInitialSpeed())
                {
                    IncreaseSpeed(10*GameManager.instance.coef, dt);
                }
            }
        }

        if (!IsAccidentHappened() && !IsInDelay())
        {
            if (GetCurrentSpeed() < GetInitialSpeed())
            {
                IncreaseSpeed(10*GameManager.instance.coef, dt);
            }
        }
    }

    private void Move(float dt)
    {
        if (GetCurrentSpeed() + GetAcceleration() * dt >= 0)
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
                if (GetCurrentSpeed() + GetAcceleration() * dt > GetCurrentSpeed())
                {
                    SetColor(Color.magenta);
                }
                else if ((GetCurrentSpeed() == 0) &&
                         (GetCurrentSpeed() + GetAcceleration() * dt == GetCurrentSpeed()) && !IsAccidentHappened() &&
                         !IsInDelay())
                {
                    SetColor(Color.black);
                }
                else if (GetCurrentSpeed() + GetAcceleration() * dt < GetCurrentSpeed())
                {
                    SetColor(Color.blue);
                }
                else if (GetCurrentSpeed() + GetAcceleration() * dt == GetCurrentSpeed())
                {
                    SetColor(Color.green);
                }
            }

            SetCurrentSpeed(GetCurrentSpeed() + GetAcceleration() * dt);
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
        var o = gameObject;
        var speed = (GetCurrentSpeed() * dt + GetAcceleration() * dt * dt / 2);
        o.transform.position = o.transform.position + Vector3.right * speed;
    }

    public void ReduceSpeed(float acceleration ,float dt)
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
        gameObject.GetComponent<SpriteRenderer>().size =
            new Vector2(length, gameObject.GetComponent<SpriteRenderer>().size.y);
        gameObject.GetComponent<BoxCollider2D>().size =
            new Vector2(length, gameObject.GetComponent<BoxCollider2D>().size.y);
    }

    public float GetHeight()
    {
        return _height;
    }

    private void SetHeight(float height)
    {
        gameObject.GetComponent<SpriteRenderer>().size =
            new Vector2(gameObject.GetComponent<SpriteRenderer>().size.x, height);
        gameObject.GetComponent<BoxCollider2D>().size =
            new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x, height);
        carText.fontSize = (int)height -4;
    }

    public float GetInitialSpeed()
    {
        return _initialSpeed;
    }

    private void SetInitialSpeed(float initialSpeed)
    {
        this._initialSpeed = initialSpeed*GameManager.instance.coef;
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

    private void SetColor(Color new_color)
    {
        _color = new_color;
        gameObject.GetComponent<SpriteRenderer>().color = new_color;
    }
}