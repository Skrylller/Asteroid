using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private int _asteroidNum;
    [SerializeField] private Vector2 _speedRange;
    public Vector2 speedRange { get{ return _speedRange; } private set { } }

    [SerializeField] private int _scoreForDestroy;
    [SerializeField] private AudioClip _destroyClip;

    private Vector2 _moveDirection;

    private int _angle;
    public float speed { get; private set; }

    private void OnEnable()
    {
        _angle = Random.Range(0, 360);
        speed = Random.Range(_speedRange.x, _speedRange.y);
        SetDirectional(speed, _angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet")
        {
            collision.GetComponent<Bullet>().Deactivate();
            CreateNewAsteroids();
            DestroyObject();
        }

        if (collision.tag == "Player" || collision.tag == "UFO")
        {
            DestroyObject();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetDirectional(float speed, int angle)
    {
        _angle = angle;

        float xSpeed = speed * Mathf.Sin(angle * -1 * Mathf.Deg2Rad);
        float ySpeed = speed * Mathf.Cos(angle * Mathf.Deg2Rad);

        _moveDirection = new Vector2(xSpeed, ySpeed);
    }

    public void CreateNewAsteroids()
    {
        if (ObjectsPull.main.asteroids.Count > _asteroidNum + 1)
        {
            float speed = Random.Range(_speedRange.x, _speedRange.y);
            Asteroid asteroid;
            asteroid = ObjectsPull.main.asteroids[_asteroidNum + 1].ActivateObject(transform.position, 0).GetComponent<Asteroid>();
            asteroid.SetDirectional(asteroid.speed, _angle + 45);
            asteroid = ObjectsPull.main.asteroids[_asteroidNum + 1].ActivateObject(transform.position, 0).GetComponent<Asteroid>();
            asteroid.SetDirectional(asteroid.speed, _angle - 45);
        }
    }

    public void DestroyObject()
    {
        GameplayController.main.PlusScore(_scoreForDestroy);
        UIController.main.UpdateText();

        gameObject.SetActive(false);
        SoundController.main.PlayClip(_destroyClip);

        if (ObjectsPull.main.CheckAllAscteroids())
        {
            Generator.main.GenerateAsteroids();
        }
    }

    private void Move()
    {
        transform.Translate(_moveDirection.x * Time.fixedDeltaTime, _moveDirection.y * Time.fixedDeltaTime, 0, Space.World);
    }

}
