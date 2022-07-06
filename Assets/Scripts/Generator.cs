using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public static Generator main;

    [SerializeField] private float _delayGenerateAsteroids;
    [SerializeField] private Vector2 _delayGenerateUFO;

    private Vector2 _screenSize;
    private int _numAsteroid;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        _numAsteroid = 2;
        _screenSize = new Vector2(Camera.main.orthographicSize * Screen.width / Screen.height, Camera.main.orthographicSize);
    }

    public void StartGenerate(int value)
    {
        _numAsteroid = value;

        StartCoroutine(GenerateAsteroidsCourotine());
        StartCoroutine(GenerateUFOCourotine());
    }
    public void GenerateAsteroids()
    {
        StartCoroutine(GenerateAsteroidsCourotine());
    }

    public IEnumerator GenerateAsteroidsCourotine()
    {
        yield return new WaitForSeconds(_delayGenerateAsteroids);

        for (int i = 0; i < _numAsteroid; i++)
        {
            int side = Random.Range(1, 5);
            Vector2 position;
            switch (side) 
            {
                case 1:
                    position = new Vector2(Random.Range(0f, 1f) * _screenSize.x * 2 - _screenSize.x, -_screenSize.y);
                    break;
                case 2:
                    position = new Vector2(Random.Range(0f, 1f) * _screenSize.x * 2 - _screenSize.x, _screenSize.y);
                    break;
                case 3:
                    position = new Vector2(_screenSize.x, Random.Range(0f, 1f) * _screenSize.y * 2 - _screenSize.y);
                    break;
                case 4:
                    position = new Vector2(-_screenSize.x, Random.Range(0f, 1f) * _screenSize.y * 2 - _screenSize.y);
                    break;
                default:
                    position = new Vector2(Random.Range(0f, 1f) * _screenSize.x * 2 - _screenSize.x, -_screenSize.y);
                    break;
            }


            ObjectsPull.main.asteroids[0].ActivateObject(position, 0);
        }
        _numAsteroid++;
    }

    public void GenerateUFO()
    {
        StartCoroutine(GenerateUFOCourotine());
    }

    public IEnumerator GenerateUFOCourotine()
    {
        yield return new WaitForSeconds(Random.Range(_delayGenerateUFO.x, _delayGenerateUFO.y));

        int side = Random.Range(0, 2) * 2 - 1;

        ObjectsPull.main.UFO.ActivateObject(new Vector2(side * _screenSize.x, Random.Range(0.2f, 0.8f) * _screenSize.y * 2 - _screenSize.y), 0).GetComponent<UFO>().SetDirectional(-side);
    }
}
