using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawController : MonoBehaviour
{
    public static DrawController instance;

    public GameObject toInstantiateRoad;
    public GameObject toInstantiateLane;
    public GameObject toInstantiateCar;

    public bool mousePressed;
    public Vector3 mousePoint;
    private List<GameObject> _gameObjects = new List<GameObject>();
    private List<GameObject> _carGameObjects = new List<GameObject>();
    private Camera cam;

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

    public void deleteAll()
    {
        foreach (var VARIABLE in _gameObjects)
        {
            Destroy(VARIABLE);
        }

        foreach (var VARIABLE in _carGameObjects)
        {
            Destroy(VARIABLE);
        }

        _carGameObjects.Clear();
        _gameObjects.Clear();
    }

    void OnGUI()
    {
        mousePoint = cam.ScreenToWorldPoint(new Vector3(Event.current.mousePosition.x,
            Event.current.mousePosition.y, cam.nearClipPlane));
    }

    public List<GameObject> GetList()
    {
        return _carGameObjects;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePressed = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mousePressed = false;
        }
    }

    public void DrawRoad(Road road)
    {
        GameObject obj = Instantiate(toInstantiateRoad, road.GetPos(), Quaternion.identity);

        obj.GetComponent<SpriteRenderer>().size = new Vector2(Screen.width * 2f, road.GetLaneCount() * 60);
        _gameObjects.Add(obj);
        obj.SetActive(true);
    }

    public void DrawLane(Lane lane)
    {
        GameObject obj = Instantiate(toInstantiateLane, lane.GetPos(), Quaternion.identity);
        obj.GetComponent<SpriteRenderer>().size = new Vector2(Screen.width * 2f, 60f);
        obj.SetActive(true);
        _gameObjects.Add(obj);
    }

    public void DrawCar(Car car)
    {
        if (car.GetDrawed() == false)
        {
            GameObject obj = Instantiate(toInstantiateCar, car.GetPos(), Quaternion.identity);
            obj.GetComponent<CarController>().CarInit(car);
            obj.GetComponentInChildren<Canvas>().sortingOrder = 5;
            obj.SetActive(true);
            _carGameObjects.Add(obj);
        }
        else
        {
            for (int i = 0; i < _carGameObjects.Count; i++)
            {
                if (_carGameObjects[i].GetComponent<CarController>().GetId() == car.GetId())
                {
                    _carGameObjects[i].transform.position = car.GetPos();
                    _carGameObjects[i].GetComponent<SpriteRenderer>().color = car.GetColor();
                    _carGameObjects[i].GetComponent<CarController>().SetText(Math.Round(car.GetCurrentSpeed() / 10)
                        .ToString(CultureInfo.InvariantCulture));
                }
            }
        }
    }
}