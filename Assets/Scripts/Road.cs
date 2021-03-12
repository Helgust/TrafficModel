using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    private int _id;
    private Vector3 _initCoord;
    private int _laneCount;
    private List<GameObject> laneList = new List<GameObject>();


    private void OnEnable()
    {
        GenerateLane();
    }

    // Start is called before the first frame update

    // Update is called once per frame

    private void GenerateLane()
    {
        GameObject lane = Instantiate(GameManager.instance.toInstantiateLane, new Vector3(0, 0, 0f),
            Quaternion.identity);
        lane.GetComponent<Lane>().LaneInit(laneList.Count, new Vector3(0f, 0f));
        lane.transform.SetParent(gameObject.transform);
        lane.SetActive(true);
        laneList.Add(lane);
    }

    public void RoadInit(int id, Vector3 initCoord, int laneCount)
    {
        SetId(id);
        SetCoord(initCoord);
        SetLaneCount(laneCount);
    }

    public void SetId(int id)
    {
        this._id = id;
    }

    public void GetId(int id)
    {
        this._id = id;
    }

    public Vector3 GetCoord()
    {
        return _initCoord;
    }

    public void SetCoord(Vector3 coord)
    {
        this._initCoord = coord;
        gameObject.transform.position = coord;
    }

    public void SetLaneCount(int count)
    {
        this._laneCount = count;
        gameObject.GetComponent<SpriteRenderer>().size = new Vector2(Screen.width * 2f, _laneCount * 60);
    }

    public int GetLaneCount()
    {
        return _laneCount;
    }
}