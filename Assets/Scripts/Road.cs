using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{

    private int _id;
    private Vector3 _initCoord;
    private int _laneCount;



    private void OnEnable()
    {
       GenerateLane();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateLane()
    {
        GameObject lane = Instantiate(GameManager.instance.toInstantiateLane, new Vector3(0, 0, 0f), Quaternion.identity);
        lane.GetComponent<Lane>().LaneInit(GameManager.instance.laneList.Count, new Vector3(0f,0f));
        lane.transform.SetParent(gameObject.transform);
        lane.SetActive(true);
        GameManager.instance.laneList.Add(lane);
    }
    
    public void RoadInit(int id,Vector3 initCoord, int laneCount)
    {
        SetId(id);
        SetCoord(initCoord);
        SetLaneCount(laneCount);
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
        gameObject.transform.position = coord;
    }
    
    private void SetLaneCount(int count)
    {
        this._laneCount = count;
        gameObject.GetComponent<SpriteRenderer>().size = new Vector2(Screen.width*2f, _laneCount*60);
    }
    
    private int GetLaneCount()
    {
        return _laneCount;
    }
    
    
}
