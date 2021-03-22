using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane
{
    private int _id;
    private Vector3 _pos;
    private List<Car> carList = new List<Car>();
    private float _brakeLow = -1;

    public Lane(int id, Vector3 pos)
    {
        SetId(id);
        SetPos(pos);
    }


    public Car GetCar(int i)
    {
        return carList[i];
    }

    public void deleteList()
    {
        carList.Clear();
    }

    public void DoStuff()
    {
        if (carList.Count != 0)
        {
            for (int i = 0; i < carList.Count; i++)
            {
                Car currentCar = GetCar(i);
                updateSpeeds(i - 1, i, Time.deltaTime);
                currentCar.Check(GameManager.instance.valueReducingSpeed, GameManager.instance.timeReducingSpeed);
                currentCar.Move(Time.deltaTime);
                DrawController.instance.DrawCar(currentCar);
                currentCar.SetDrawed(true);

                if (currentCar.IsAccidentHappened())
                {
                    if (currentCar.GetCurrentSpeed() == 0)
                        currentCar.SetAcceleration(0);
                    if (Math.Floor(currentCar.GetTimeInAccident()) >= GameManager.instance.timeReducingSpeed)
                    {
                        if ((i == 0) || (i > 0 &&
                                         (GetCar(i - 1).GetPos().x - currentCar.GetPos().x - currentCar.GetLength() >
                                          3 * currentCar.GetLength())))
                        {
                            currentCar.SetAccidentHappened(false);
                            currentCar.SetTimeInAccident(0);
                        }
                    }

                    currentCar.SetTimeInAccident(currentCar.GetTimeInAccident() + Time.deltaTime);
                }

                if (currentCar.IsInDelay())
                {
                    if (currentCar.GetActualTimeReducingSpeed() <= 0)
                    {
                        currentCar.SetActualTimeReducingSpeed(0);
                        currentCar.SetInDelay(false);
                    }

                    currentCar.SetActualTimeReducingSpeed(currentCar.GetActualTimeReducingSpeed() -
                                                          Time.deltaTime * 10);
                }
            }
        }

        CheckCar();
        if (!GameManager.instance.timerReached)
        {
            if (GameManager.instance.intervalTimer > 0)
            {
                GameManager.instance.intervalTimer -= Time.deltaTime;
            }
            else
            {
                GameManager.instance.intervalTimer = 0;
                GameManager.instance.timerReached = true;
            }
        }
        else
        {
            GameManager.instance.intervalTimer =
                GameManager.getRnd.Next(GameManager.instance.minInterval, GameManager.instance.maxInterval);
            GameManager.instance.timerReached = false;
        }
    }


    // private void UpdateSpeeds(int id,float dt)
    // {
    //     if (!IsAccidentHappened() && !IsInDelay())
    //     {
    //         if (GetCurrentSpeed() < GetInitialSpeed())
    //         {
    //             IncreaseSpeed(10 * GameManager.instance.coef, dt);
    //         }
    //     }
    // }
    //
    private void updateSpeeds(int carFrontNumber, int carNextNumber, float dt)
    {
        Car carNext = GetCar(carNextNumber);
        if (carNextNumber != 0)
        {
            Car carFront = GetCar(carFrontNumber);
            float dist = carFront.GetPos().x + carFront.GetLength() / 2 - carNext.GetPos().x - carNext.GetLength() / 2;
            if (!carNext.IsAccidentHappened() && !carNext.IsInDelay())
            {
                if (dist <= 3 * carNext.GetLength() && dist > carNext.GetLength())
                {
                    carNext.ReduceSpeed(-10 * GameManager.instance.coef, dt);
                    return;
                }
            }

            if (carNext.GetCurrentSpeed() > 0)
            {
                if (dist <= carNext.GetLength())
                {
                    isAccident(carFrontNumber, carNextNumber);
                    return;
                }
            }
        }

        if (!carNext.IsAccidentHappened() && !carNext.IsInDelay())
        {
            if (carNext.GetCurrentSpeed() < carNext.GetInitialSpeed())
            {
                //float acl = _brakeLow * (-1);
                carNext.IncreaseSpeed(10 * GameManager.instance.coef, dt);
            }
        }
    }

    //"isAccident" changes the speeds and accelerations of cars in case of accident
    private void isAccident(int carFrontN, int carNextN)
    {
        Car carFront = GetCar(carFrontN);
        Car carNext = GetCar(carNextN);
        int next = carNextN, front = carFrontN;
        int carsInAccident = 1;
        carNext.SetAccidentHappened(true);
        carFront.SetAccidentHappened(true);
        while (front >= 0 && carFront.IsAccidentHappened() &&
               (carFront.GetPos().x + carFront.GetLength() / 2 - carNext.GetPos().x - carNext.GetLength() / 2 <=
                carNext.GetLength()))
        {
            next--;
            front--;
            carsInAccident++;
            carNext = GetCar(next);
            if (front >= 0)
                carFront = GetCar(front);
        }

        next = carNextN;
        front = carFrontN;
        carNext = GetCar(next);
        carFront = GetCar(front);
        carNext.SetCurrentSpeed(0);
        carNext.SetAcceleration(0);
        while (front >= 0 && carFront.IsAccidentHappened() &&
               (carFront.GetPos().x + carFront.GetLength() / 2 - carNext.GetPos().x - carNext.GetLength() / 2 <=
                carNext.GetLength()))
        {
            carFront.SetCurrentSpeed(0);
            carFront.SetAcceleration(0);
            next--;
            front--;
            carNext = GetCar(next);
            if (front >= 0)
                carFront = GetCar(front);
        }
    }

    private void CreateCar()
    {
        carList.Add(new Car(carList.Count, GameManager.instance.lengthCar, GameManager.instance.heightCar,
            new Vector3(-3f, 0f, 0f),
            GameManager.getRnd.Next(GameManager.instance.minSpeed, GameManager.instance.maxSpeed)));
    }

    private void CheckCar()
    {
        if (carList.Count == 0)
        {
            CreateCar();
        }

        if (GameManager.instance.timerReached && carList[carList.Count - 1].GetPos().x > GameManager.instance.lengthCar)
        {
            CreateCar();
        }
    }


    public void SetId(int id)
    {
        this._id = id;
    }

    public void GetId(int id)
    {
        this._id = id;
    }

    public Vector3 GetPos()
    {
        return _pos;
    }

    public void SetPos(Vector3 pos)
    {
        this._pos = pos;
    }
}