using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


public class CarController : MonoBehaviour
{
    private int _id;
    private Vector3 _pos;
    public Text carText;
    private float _currentSpeed;
    private bool isDrawed;
    private float brakeLow = -1;
    private int count;
    private int coef = 10;

    public void CarInit(Car car)
    {
        SetId(car.GetId());
        SetCoord(car.GetPos());
        SetLength(GameManager.instance.lengthCar);
        SetHeight(GameManager.instance.heightCar);
        SetText(Math.Round(car.GetCurrentSpeed() / 10).ToString(CultureInfo.InvariantCulture));
        SetColor(Color.green);
    }

    private void Start()
    {
        count = 0;
        gameObject.GetComponentInChildren<Canvas>().sortingOrder = 5;
    }

    private void Update()
    {
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

    public Vector3 GetCoord()
    {
        return _pos;
    }

    public void SetCoord(Vector3 coord)
    {
        this._pos = coord;
        gameObject.transform.position = coord;
    }


    public void SetLength(float length)
    {
        gameObject.GetComponent<SpriteRenderer>().size =
            new Vector2(length, gameObject.GetComponent<SpriteRenderer>().size.y);
        gameObject.GetComponent<BoxCollider2D>().size =
            new Vector2(length, gameObject.GetComponent<BoxCollider2D>().size.y);
    }


    public void SetHeight(float height)
    {
        gameObject.GetComponent<SpriteRenderer>().size =
            new Vector2(gameObject.GetComponent<SpriteRenderer>().size.x, height);
        gameObject.GetComponent<BoxCollider2D>().size =
            new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x, height);
        carText.fontSize = (int) height - 4;
    }

    public Color GetColor()
    {
        return Color.magenta;
    }

    public void SetColor(Color new_color)
    {
        gameObject.GetComponent<SpriteRenderer>().color = new_color;
    }

    public void SetText(string newText)
    {
        carText.text = newText;
    }
}