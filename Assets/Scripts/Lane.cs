using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Lane : MonoBehaviour
{
    private int _id;
    private Vector3 _initCoord;
    public List<GameObject> carList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (var VARIABLE in carList)
        {
            if (VARIABLE.transform.position.x > GameManager.instance.ScreenAdjust*2f)
            {
                carList.Remove(VARIABLE);
                Destroy(VARIABLE);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckCar();
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
        gameObject.GetComponent<SpriteRenderer>().size = new Vector2(GameManager.instance.ScreenAdjust,2f);
    }
    public void LaneInit(int id,Vector3 initCoord)
    {
        SetId(id);
        SetCoord(initCoord);
    }
    private void CreateCar()
    {
        GameObject car = Instantiate(GameManager.instance.toInstantiateCar, new Vector3(0f, 0f, 0f), Quaternion.identity);
        car.GetComponent<Car>().CarInit(carList.Count, GameManager.instance.lengthCar, GameManager.instance.heightCar,
            new Vector3(-GameManager.instance.ScreenAdjust/2f,0f,0f), 5f);
        //GameManager.getRnd.Next(GameManager.instance.minSpeed, GameManager.instance.maxSpeed)
        car.transform.SetParent(gameObject.transform);
        car.SetActive(true);
        carList.Add(car);
    }

    private void CheckCar()
    {
        int listLen = carList.Count;
        Vector3 startPos = new Vector3(-GameManager.instance.ScreenAdjust/2f, 0f, 0f);
        if (carList.Count == 0)
        {
            CreateCar();
        }
        else
        {
            if ((carList[listLen - 1].transform.position - startPos).x > 3f)
            {
                //Debug.Log("LastCarPosX"+carList[listLen - 1].transform.position.x+" Diff="+(carList[listLen - 1].transform.position - startPos).x + " carlistCount="+carList.Count);
                CreateCar();
            }
            // else
            // {
            //     
            // }
        }
    }
}
