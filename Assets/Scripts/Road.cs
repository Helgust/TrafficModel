using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road
{
    private int _id;
    private Vector3 _pos;
    private int _laneCount;
    private List<Lane> _laneList = new List<Lane>();


    public Road(int id, Vector3 initPos, int laneCount)
    {
        SetId(id);
        SetPos(initPos);
        SetLaneCount(laneCount);
    }

    public void GenerateLane()
    {
        _laneList.Add(new Lane(_laneList.Count, new Vector3(0f, 0f)));
        DrawController.instance.DrawLane(_laneList[_laneList.Count - 1]);
    }


    public void DoStuff()
    {
        for (int i = 0; i < _laneList.Count; i++)
        {
            _laneList[i].DoStuff();
        }
    }

    public List<Lane> GetList()
    {
        return _laneList;
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

    public void SetLaneCount(int count)
    {
        this._laneCount = count;
    }

    public int GetLaneCount()
    {
        return _laneCount;
    }
}